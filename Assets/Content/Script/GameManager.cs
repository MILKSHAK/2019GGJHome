using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public bool isDead { get; private set; }

	[NonSerialized]
	public DeathReason deathReason;

	public Material planetHeatMaterial;

	public GameObject explodePrefab;

	public float initialBoostSpeed; // 引擎加速

	[Header("受太阳吸引速度")]
	public float initialConstSpeed; // 受太阳吸引速度

	public float initialEnergy;

	public float energyCost;

	public float shootEnergyCost;

	public float energyRecovery;

	public float currentEnergy;

	public float pickUpEnergy;

	private float _runningTime;

	private float _distanceToSun;

	public float boostSpeed;

	public float currentSpeed;

	private void Start()
	{
		EventBus.Subscribe<EnumEventType>(OnEvent);
		SetupGame();
	}

	private void Update()
	{
		_runningTime = Time.time;

		if (Input.GetKeyDown(KeyCode.F5)) {
			SceneManager.LoadScene("GameScene");
		}
	}

	private void SetupGame()
	{
		currentEnergy = initialEnergy;
	}

	private void OnEvent(EnumEventType eventType)
	{
		if (eventType == EnumEventType.PickupEnergy)
		{
			OnPickEnergy();
		}
		else if (eventType == EnumEventType.HitObstacleSmall)
		{
			OnHitObstacle();
		}
		else if (eventType == EnumEventType.HitObstacleBig)
		{
			OnHitPlayer();
		}
		else if (eventType == EnumEventType.PlayerDestroy)
		{
			OnPlayerDead();
			isDead = true;
		}
		return;
	}

	private void OnPickEnergy()
	{
		currentEnergy += pickUpEnergy;
		return;
	}

	private void OnHitObstacle()
	{
		return;
	}

	private void OnHitPlayer()
	{
		return;
	}

	private void OnPlayerDead()
	{
		return;
	}
}
