using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitType
{
    HitSmall,
    HitBig,
}

[RequireComponent(typeof(Collider2D))]
public class HitTrigger : MonoBehaviour
{
    public HitType _hitType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (_hitType == HitType.HitSmall)
            {
                EventBus.Post<EnumEventType>(EnumEventType.HitObstacleSmall);
            }
            else if (_hitType == HitType.HitBig)
            {
                EventBus.Post<EnumEventType>(EnumEventType.HitObstacleBig);
            }
        }
    }
}
