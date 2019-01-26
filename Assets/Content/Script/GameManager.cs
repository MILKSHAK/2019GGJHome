using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private float _initialBoostSpeed; // 引擎加速

	[SerializeField]
	private float _initialConstSpeed; // 受太阳吸引速度

	[SerializeField]
	private float _initialEnergy;

	private float _runningTime;

	private float _distanceToSun;

	private float _boostSpeed;

	private void Start()
	{
		EventBus.Subscribe<EventType>(OnEvent);
	}

	private void OnEvent(EventType eventType)
	{
		if (eventType == EventType.PickupEnergy)
		{
			OnPickEnergy();
		}
		else if (eventType == EventType.HitObject)
		{

		}
		else if (eventType == EventType.HitPlayer)
		{

		}
		else if (eventType == EventType.PlayerDestroy)
		{

		}
		return;
	}

	private void OnPickEnergy()
	{
		return;
	}
}
