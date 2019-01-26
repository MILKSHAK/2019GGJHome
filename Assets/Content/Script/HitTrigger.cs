using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitType
{
	HitObject,
	HitPlayer,
}

[RequireComponent(typeof(Collider2D))]
public class HitTrigger : MonoBehaviour
{
	public HitType _hitType;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			if (_hitType == HitType.HitObject)
			{
				EventBus.Post<EventType>(EventType.HitObstacle);
			}
			else if (_hitType == HitType.HitPlayer)
			{
				EventBus.Post<EventType>(EventType.HitPlayer);
			}
		}
	}
}
