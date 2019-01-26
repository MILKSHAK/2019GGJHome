using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
	Energy
}

[RequireComponent(typeof(Collider2D))]
public class Pickup : MonoBehaviour
{
	public PickupType _pickupType;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			if (_pickupType == PickupType.Energy)
			{
				EventBus.Post<EventType>(EventType.PickupEnergy);
			}

			Destroy(gameObject);
		}
	}
}
