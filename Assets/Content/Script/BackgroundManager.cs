using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
	[SerializeField]
	private float _scrollSpeed;

	[SerializeField]
	private Transform[] _backgrounds;

	[SerializeField]
	private Vector3 _scrollUpdate = new Vector3(21.5f, 0, 0);

	[SerializeField]
	private Vector3 _updatePoint = new Vector3(-23f, 0, 0);

	void Start() {
		EventBus.Subscribe<EventType>(ev => {
			if (ev == EventType.PlayerDestroy) {
				Destroy(this);
			}
		});
	}

	private void Update()
	{
		float scrollUpdate = _scrollSpeed * Time.deltaTime;
		transform.Translate(new Vector3(_scrollSpeed, 0, 0));
		UpdateBGScroll();
	}

	private void UpdateBGScroll()
	{
		foreach (Transform bgTrans in _backgrounds)
		{
			if (bgTrans.position.x <= _updatePoint.x)
			{
				bgTrans.Translate(bgTrans.position + _scrollUpdate);
			}
		}
		return;
	}

}
