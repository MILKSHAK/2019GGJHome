using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float _maxSpeed = 3;

	[SerializeField]
	private float _deaccelerateRate = 1;

	[SerializeField]
	private Vector2 _constantForce = new Vector2(-1, 0);

	private Rigidbody2D _rigidbody;

	private Vector2 _currentVelocity = new Vector2(0, 0);

	private GameManager _gameManager;

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	private void Update()
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
	}

}
