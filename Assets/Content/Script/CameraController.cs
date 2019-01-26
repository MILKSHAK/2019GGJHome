using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private Camera _camera;

	[SerializeField]
	private float cameraSizeSmall = 4;

	[SerializeField]
	private float cameraSizeNormal = 5;

	[SerializeField]
	private float cameraSizeBoost = 4.7f;

	[SerializeField]
	private float camFarPosX = 1;

	[SerializeField]
	private float camNearPosX = -5;

	private float camBoostSpeed = 0.1f;

	private bool boosting = false;

	private GameObject player;

	private float boostCamRate = 1;

	private void Start()
	{
		_camera = GetComponent<Camera>();
		EventBus.Subscribe<EventType>(OnEvent);
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void OnEvent(EventType ev)
	{
		if (ev == EventType.PlayerBoostStart)
		{
			boosting = true;
		}
		else if (ev == EventType.PlayerBoostEnd)
		{
			boosting = false;
		}
		return;
	}

	private void Update()
	{
		if (player == null)
		{
			return;
		}
		float currX = player.transform.position.x;
		float maxCamDistance = camFarPosX - camNearPosX;
		float maxCamSizeDiff = cameraSizeNormal - cameraSizeSmall;
		float distCamRate = Mathf.Clamp(currX - camNearPosX, 0, maxCamDistance) / maxCamDistance;

		boostCamRate += (boosting ? -1 : 1) * camBoostSpeed * Time.deltaTime;
		boostCamRate = Mathf.Clamp(boostCamRate, 0, 1);

		float fixedSize = maxCamSizeDiff * distCamRate * boostCamRate;
		_camera.orthographicSize = cameraSizeSmall + fixedSize;
	}
}
