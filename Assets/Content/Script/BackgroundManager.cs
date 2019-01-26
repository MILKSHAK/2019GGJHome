using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
	[SerializeField]
	private float _scrollSpeed;

	[SerializeField]
	private Transform _background;

	private void Update()
	{
		float scrollUpdate = _scrollSpeed * Time.deltaTime;
		_background.Translate(new Vector3(_scrollSpeed * scrollUpdate, 0, 0));
	}

}
