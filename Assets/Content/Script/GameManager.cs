using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public float initialBoostSpeed; // 引擎加速

	public float initialConstSpeed; // 受太阳吸引速度

	public float initialEnergy;

	public float energyCost;

	public float energyRecovery;

	public float currentEnergy;

	private float _runningTime;

	private float _distanceToSun;

	private float _boostSpeed;

	private void Start()
	{
		EventBus.Subscribe<EventType>(OnEvent);
		SetupGame();
	}

	private void SetupGame()
	{
		currentEnergy = initialEnergy;
	}

	private void OnEvent(EventType eventType)
	{
		if (eventType == EventType.PickupEnergy)
		{
			OnPickEnergy();
		}
		else if (eventType == EventType.HitObstacle)
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
