﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[Serializable]
public class SoundEffect
{
	public string name;
	public AudioClip clip;
}

public class GameManager : MonoBehaviour
{
	public bool isDead { get; private set; }

	public bool isWin { get; private set; }

	[NonSerialized]
	public DeathReason deathReason;

	[NonSerialized]
	public bool started = false;

	public Material planetHeatMaterial;

	public GameObject explodePrefab;

	public float initialBoostSpeed; // 引擎加速

	[Header("受太阳吸引速度")]
	public float initialConstSpeed; // 受太阳吸引速度

	public float initialEnergy;

	public float energyCost;

	public float shootEnergyCost;

	public float energyRecovery;

	[NonSerialized]
	public float gameProgress;

	public float currentEnergy;

	public float pickUpEnergy;

	private float _runningTime;

	private float _distanceToSun;

	public float boostSpeed;

	public float currentSpeed;

	private AudioSource _audioSource;

	[SerializeField]
	private SoundEffect[] _soundEffects;

	private List<SoundEffect> _soundEffectList;

	private void Start()
	{
		SetupGame();
	}

	void OnEnable() {
		EventBus.Subscribe<EnumEventType>(OnEvent);
	}

	void OnDisable() {
		EventBus.Unsubscribe<EnumEventType>(OnEvent);
	}

	private void Update()
	{
		_runningTime = Time.time;

		if (started) {
			if (GameInput.MainMenu.Down)
				SceneManager.LoadScene("MainMenu");
			if (GameInput.Start.Down)
				SceneManager.LoadScene("GameScene");
		}

		if (isDead && Input.anyKeyDown) {
			SceneManager.LoadScene("GameScene");
		}
	}

	private void SetupGame()
	{
		currentEnergy = initialEnergy;
		_audioSource = GetComponent<AudioSource>();
		_soundEffectList = new List<SoundEffect>(_soundEffects);
	}

	private void OnEvent(EnumEventType eventType)
	{
		if (eventType == EnumEventType.PickupEnergy)
		{
			OnPickEnergy();
		}
		else if (eventType == EnumEventType.HitObstacleSmall)
		{
			OnHitSmall();
		}
		else if (eventType == EnumEventType.HitObstacleBig)
		{
			OnHitBig();
		}
		else if (eventType == EnumEventType.PlayerDestroy)
		{
			OnPlayerDead();
			isDead = true;
		} else if (eventType == EnumEventType.EscapedSun)
		{
			OnPlayerWin();
			isWin = true;
		}
		return;
	}



	private void OnPickEnergy()
	{
		currentEnergy += pickUpEnergy;
		return;
	}

	private void OnHitSmall()
	{
		_audioSource.clip = _soundEffectList.Find(x => x.name == "HitSmall").clip;
		_audioSource.Play();
		return;
	}

	private void OnHitBig()
	{
		if (!_audioSource)
			return;
		_audioSource.clip = _soundEffectList.Find(x => x.name == "HitBig").clip;
		_audioSource.Play();
		return;
	}

	private void OnPlayerDead()
	{
		if (!_audioSource)
			return;
		_audioSource.clip = _soundEffectList.Find(x => x.name == "PlayerDead").clip;
		_audioSource.Play();
		return;
	}

	private void OnPlayerWin()
	{
		if (!_audioSource)
			return;
		_audioSource.clip = _soundEffectList.Find(x => x.name == "PlayerWin").clip;
		_audioSource.Play();
	}
}
