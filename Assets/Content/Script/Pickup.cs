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

    public float speed = -1;

    private void Update()
    {
        transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (_pickupType == PickupType.Energy)
            {
                EventBus.Post<EnumEventType>(EnumEventType.PickupEnergy);
            }

            Destroy(gameObject);
        }
    }
}
