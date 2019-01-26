using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float _maxSpeed = 3;

	[SerializeField]
	private float _deaccelerateRate = -1;

	[SerializeField]
	private float _boostRate = -1;

	[SerializeField]
	private float _accelerateRate = 3;

	[SerializeField]
	private Transform _lazerPrefab;

	private Rigidbody2D _rigidbody;

	private GameManager _gameManager;

	public float _currentSpeed = 0;

	public Material energyMat;

	private bool _boosting = false;

	private bool _firing = false;

	private Transform _lazer;

	private Animator _lazerAnimator;

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		_lazer = Instantiate(_lazerPrefab, transform);
		_lazer.gameObject.SetActive(false);
		_lazerAnimator = _lazer.Find("Lazer").GetComponent<Animator>();

		energyMat = Instantiate(energyMat);
		transform.Find("_UIBar").GetComponent<SpriteRenderer>().material = energyMat;
		transform.Find("_UIBar2").GetComponent<SpriteRenderer>().material = energyMat;
	}

	private void OnEnable() {
		EventBus.Subscribe<EventType>(OnEvent);
	}

	private void OnDisable() {
		EventBus.Unsubscribe<EventType>(OnEvent);
	}

	private void OnEvent(EventType ev) {
		if (ev == EventType.PlayerDestroy) {
			StartCoroutine(ActionDestroy());
		}
	}

	IEnumerator ActionDestroy() {
		yield return null;
		Destroy(gameObject);
	}

	float _displayEnergy = .0f;

	void Update() {
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
		else if (!GameInput.Boost.Down)
		{
			if (_rigidbody.velocity.x > _gameManager.initialConstSpeed)
			{
				_gameManager.currentSpeed = _gameManager.currentSpeed - Time.fixedDeltaTime * _deaccelerateRate;
				force.x += -_boostRate;
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
		var nvel = _rigidbody.velocity;
		nvel.y = Mathf.Min(_maxSpeed, Mathf.Abs(nvel.y)) * Mathf.Sign(nvel.y);
		_rigidbody.velocity = nvel;
	}

	public void BoostStart()
	{
		_boosting = true;
		return;
	}

	public void BoostUpdate(ref Vector2 force)
	{
		if (_boosting && _gameManager.currentEnergy > 0 && _rigidbody.velocity.x < _maxSpeed)
		{
			_gameManager.currentSpeed = _gameManager.currentSpeed + Time.fixedDeltaTime * _gameManager.boostSpeed;
			force.x += _deaccelerateRate;
			_gameManager.currentEnergy -= _gameManager.energyCost * Time.fixedDeltaTime;
		}
		else
		{
			_boosting = false;
		}
		return;
	}

	public void BoostFinish()
	{
		_boosting = false;
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
		_gameManager.currentEnergy -= _gameManager.energyCost * Time.deltaTime;
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
