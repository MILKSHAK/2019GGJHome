﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraController : MonoBehaviour
{
	public PostProcessingProfile postProcess;

	private Camera _camera;

	public AnimationCurve camRatioOverPos;

	public float camRatioBoost;

	private bool boosting = false;

	private bool win = false;

	private GameObject player;

	private void Start()
	{
		_camera = GetComponent<Camera>();
		EventBus.Subscribe<EnumEventType>(OnEvent);
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void OnEvent(EnumEventType ev)
	{
		if (ev == EnumEventType.PlayerBoostStart)
		{
			boosting = true;
		}
		else if (ev == EnumEventType.PlayerBoostEnd)
		{
			boosting = false;
		}
		else if (ev == EnumEventType.EscapedSun)
		{
			win = true;
		}
		return;
	}

	float _camRatioBoost = 1.1f;

	private void Update()
	{
		if (player == null)
		{
			return;
		}

		if (win)
		{
			var npos = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * 5f);
			npos.z = -10;
			transform.position = npos;
			
			_camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, 4f, Time.deltaTime * 1f);
			UpdatePostProcessing();
			return;
		}

		float camRatioPos = camRatioOverPos.Evaluate(player.transform.position.x);
		float camRatioBoostTarget = boosting ? camRatioBoost : 1;

		_camRatioBoost = Mathf.MoveTowards(_camRatioBoost, camRatioBoostTarget, Time.deltaTime * 0.5f);

		_camera.orthographicSize = 5 * camRatioPos * _camRatioBoost;

		UpdatePostProcessing();
	}

	float _chromatic;

	void UpdatePostProcessing() {
		float targetChromatic = !win && boosting ? 1.0f : 0.05f;
		_chromatic = Mathf.MoveTowards(_chromatic, targetChromatic, Time.deltaTime * 0.8f);
		var settings = postProcess.chromaticAberration.settings;
		settings.intensity = _chromatic;
		postProcess.chromaticAberration.settings = settings;

		var s2 = postProcess.grain.settings;
		s2.intensity = _chromatic / 1.7f;
		postProcess.grain.settings = s2;
	}
}
