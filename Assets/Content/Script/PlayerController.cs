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
	private Transform _lazerPrefab;

	private Rigidbody2D _rigidbody;

	private GameManager _gameManager;

	public float _currentSpeed = 0;

	private bool _boosting = false;

	private GameObject _firedLazer;

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	private void OnEnable() {
		EventBus.Subscribe<EventType>(OnEvent);
	}

	private void OnDisable() {
		EventBus.Unsubscribe<EventType>(OnEvent);
	}

	private void OnEvent(EventType ev) {
		if (ev == EventType.PlayerDestroy) {
			Destroy(gameObject);
		}
	}

	private void FixedUpdate()
	{
		if (GameInput.MoveAxis.y != 0)
		{
			if (_rigidbody.velocity.sqrMagnitude <= _maxSpeed)
			{
				_rigidbody.AddForce(new Vector2(0, GameInput.MoveAxis.y));
			}
		}
		else
		{
			if (_rigidbody.velocity.y > 0)
			{
				_rigidbody.AddForce(new Vector2(0, -_deaccelerateRate));
			}
			else if (_rigidbody.velocity.y < 0)
			{
				_rigidbody.AddForce(new Vector2(0, _deaccelerateRate));
			}
		}

		if (GameInput.Boost.Down)
		{
			BoostStart();
		}
		else if (GameInput.Boost.Pressing)
		{
			BoostUpdate();
		}
		else if (GameInput.Boost.Up)
		{
			BoostFinish();
		}
		else
		{
			if (_rigidbody.velocity.x > _gameManager.initialConstSpeed)
			{
				_currentSpeed = _currentSpeed - Time.fixedDeltaTime * _deaccelerateRate;
				_rigidbody.AddForce(new Vector2(-_deaccelerateRate, 0));
			}
		}

		if (GameInput.Shoot.Down)
		{
			FireStart();
		}
		else if (GameInput.Shoot.Up)
		{
			FireFinish();
		}

		if (!_boosting)
		{
			_gameManager.currentEnergy += _gameManager.energyRecovery * Time.fixedDeltaTime;
		}
	}

	public void BoostStart()
	{
		_boosting = true;
		return;
	}

	public void BoostUpdate()
	{
		if (_boosting && _gameManager.currentEnergy > 0 && _rigidbody.velocity.x < _maxSpeed)
		{
			_currentSpeed = _currentSpeed + Time.fixedDeltaTime * _gameManager.initialBoostSpeed;
			_rigidbody.AddForce(new Vector2(_deaccelerateRate, 0));
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
		if (_firedLazer == null)
		{
			_firedLazer = Instantiate(_lazerPrefab).gameObject;
		}

		return;
	}

	public void FireFinish()
	{
		if (_firedLazer != null)
		{
			Destroy(_firedLazer);
			_firedLazer = null;
		}
		
		return;
	}
}
