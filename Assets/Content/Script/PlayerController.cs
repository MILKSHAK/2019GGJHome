﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public GameObject explodePrefab;

	public GameObject energyExplodePrefab;

	[SerializeField]
	private Vector2 _maxSpeed = new Vector2(3, 3);

	[SerializeField]
	private float _deaccelerateRate = -1;

	[SerializeField]
	private float _boostRate = -1;

	[SerializeField]
	private float _boostDeaccelerateRate = 3;

	[SerializeField]
	private float _accelerateRate = 3;

	[SerializeField]
	private float _hitImpactSmall = 0.5f;

	[SerializeField]
	private float _hitImpactBig = 1;

	[SerializeField]
	private Transform _lazerPrefab;

	private Rigidbody2D _rigidbody;

	private GameManager _gameManager;

	public Material energyMat;

	private bool _boosting = false;

	private bool _firing = false;

	private Transform _lazer;

	private Animator _lazerAnimator;

	private Animator _animator;

	private AudioSource _audioSource;

	[SerializeField]
	private SoundEffect[] _soundEffects;

	private List<SoundEffect> _soundEffectList;

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_lazer = transform.Find("Lazer");
		_lazer.gameObject.SetActive(false);
		_lazerAnimator = _lazer.GetComponent<Animator>();
		_animator = GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource>();

		_soundEffectList = new List<SoundEffect>(_soundEffects);

		energyMat = Instantiate(energyMat);
		transform.Find("_UIBar").GetComponent<SpriteRenderer>().material = energyMat;
		transform.Find("_UIBar2").GetComponent<SpriteRenderer>().material = energyMat;
	}

	private void OnEnable() {
		EventBus.Subscribe<EnumEventType>(OnEvent);
	}

	private void OnDisable() {
		EventBus.Unsubscribe<EnumEventType>(OnEvent);
	}

	private void OnEvent(EnumEventType ev) {
		if (ev == EnumEventType.PlayerDestroy) {
			transform.Find("_UIBar").gameObject.SetActive(false);
			transform.Find("_UIBar2").gameObject.SetActive(false);
			if (_gameManager.deathReason == DeathReason.Burn) {
				_animator.SetTrigger("DieBurning");
			} else {
				StartCoroutine(ActionDestroyCollision());
			}
		}
		else if (ev == EnumEventType.HitObstacleSmall)
		{
			OnHitSmall();
		}
		else if (ev == EnumEventType.HitObstacleBig)
		{
			OnHitBig();
		}
		else if (ev == EnumEventType.EscapedSun)
		{
			StartCoroutine(ActionDestroyWin());
		}
	}

	private void OnHitBig()
	{
		_rigidbody.velocity = _rigidbody.velocity - new Vector2(_hitImpactBig, 0);
		// transform.Translate(new Vector3(-_hitImpactBig, 0, 0));
		// todo: 大撞击效果
		return;
	}

	private void OnHitSmall()
	{
		// todo: 小撞击效果
		_rigidbody.velocity = _rigidbody.velocity - new Vector2(_hitImpactSmall, 0);
		// transform.Translate(new Vector3(-_hitImpactSmall, 0, 0));
		return;
	}

	IEnumerator ActionDestroy() {
		yield return null;
		Instantiate(explodePrefab, transform.position, Quaternion.identity);
		yield return new WaitForSeconds(0.3f);
		Destroy(gameObject);
	}

	IEnumerator ActionDestroyWin() {
		yield return null;
		yield return new WaitForSeconds(20.0f);
		Destroy(gameObject);
	}

	IEnumerator ActionDestroyCollision() {
		yield return null;
		Instantiate(energyExplodePrefab, transform.position, Quaternion.identity);
		yield return new WaitForSeconds(0.3f);
		Destroy(gameObject);
	}

	float _displayEnergy = .0f;

	void Update() {
		if (!_gameManager.started)
			return;
		float energyRatio = _gameManager.currentEnergy / _gameManager.initialEnergy;
		_displayEnergy = Mathf.MoveTowards(_displayEnergy, energyRatio, Time.deltaTime * 1.0f);
		energyMat.SetFloat("_Progress", _displayEnergy);

		if (GameInput.Boost.Down)
		{
			BoostStart();
		}
		else if (GameInput.Boost.Up)
		{
			BoostFinish();
		}

		if (GameInput.Shoot.Down)
		{
			FireStart();
		}
		else if (GameInput.Shoot.Up)
		{
			FireFinish();
		}
	}

	private void FixedUpdate()
	{
		if (!_gameManager.started)
			return;

		Vector2 nvel;
		if (_gameManager.isDead || _gameManager.isWin) {
			nvel = _rigidbody.velocity;
			nvel = Vector2.MoveTowards(nvel, Vector2.zero, Time.deltaTime * 2f);
			_rigidbody.velocity = nvel;

			return;
		}

		Vector2 force = Vector2.zero;
		if (Mathf.Abs(GameInput.MoveAxis.y) > 0.1f)
		{
			force.y += GameInput.MoveAxis.y * _accelerateRate;
		}
		else
		{
			if (_rigidbody.velocity.y > 0)
			{
				force.y += -_deaccelerateRate;
			}
			else if (_rigidbody.velocity.y < 0)
			{
				force.y += _deaccelerateRate;
			}
		}

		if (GameInput.Boost.Pressing)
		{
			BoostUpdate(ref force);
		}
		else if (!GameInput.Boost.Pressing)
		{
			if (_rigidbody.velocity.x > _gameManager.initialConstSpeed)
			{
				_gameManager.currentSpeed = _gameManager.currentSpeed - Time.fixedDeltaTime * _deaccelerateRate;
				force.x += -_boostDeaccelerateRate;
			}
		}

		if (GameInput.Shoot.Pressing)
		{
			FireUpdate();
		}

		if (!_boosting && !_firing)
		{
			_gameManager.currentEnergy = Mathf.Min(_gameManager.initialEnergy, _gameManager.currentEnergy + _gameManager.energyRecovery * Time.fixedDeltaTime);
		}

		_rigidbody.AddForce(force);
		nvel = _rigidbody.velocity;
		nvel.x = Mathf.Min(_maxSpeed.x, nvel.x);
		nvel.y = Mathf.Min(_maxSpeed.y, Mathf.Abs(nvel.y)) * Mathf.Sign(nvel.y);
		_rigidbody.velocity = nvel;
	}

	public void BoostStart()
	{
		_boosting = true;
		EventBus.Post<EnumEventType>(EnumEventType.PlayerBoostStart);
		transform.Find("Booster").gameObject.SetActive(true);
		_audioSource.clip = _soundEffectList.Find(x => x.name == "Boost").clip;
		_audioSource.loop = true;
		_audioSource.Play();
		return;
	}

	public void BoostUpdate(ref Vector2 force)
	{
		if (_boosting && _gameManager.currentEnergy > 0)
		{
			_gameManager.currentSpeed = _gameManager.currentSpeed + Time.fixedDeltaTime * _gameManager.boostSpeed;
			force.x += _boostRate;
			_gameManager.currentEnergy = Mathf.Max(0, _gameManager.currentEnergy - _gameManager.energyCost * Time.fixedDeltaTime);
		}
		else
		{
			EventBus.Post(EnumEventType.PlayerBoostEnd);
			_boosting = false;
		}
		return;
	}

	public void BoostFinish()
	{
		if (_boosting)
			EventBus.Post(EnumEventType.PlayerBoostEnd);
		transform.Find("Booster").gameObject.SetActive(false);
		_boosting = false;
		_audioSource.Stop();
		return;
	}

	public void FireStart()
	{
		if (_lazerAnimator != null)
		{
			_firing = true;
			_lazer.gameObject.SetActive(true);
			_lazerAnimator.SetBool("Fire", true);
		}

		return;
	}

	private void FireUpdate()
	{
		_gameManager.currentEnergy = Mathf.Max(0, 
			_gameManager.currentEnergy - _gameManager.shootEnergyCost * Time.deltaTime);
		if (_gameManager.currentEnergy == 0)
			_firing = false;
		return;
	}

	public void FireFinish()
	{
		if (_lazerAnimator != null)
		{
			_firing = false;
			_lazer.gameObject.SetActive(false);
			_lazerAnimator.SetBool("Fire", false);
		}

		return;
	}
}
