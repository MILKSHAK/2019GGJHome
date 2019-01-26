using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private Camera _camera;

	private void Start()
	{
		_camera = GetComponent<Camera>();
	}
}
