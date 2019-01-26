using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDead : MonoBehaviour
{
    void Start()
    {
        EventBus.Subscribe<EventType>(ev => {
            GetComponent<Animator>().SetTrigger("Die");
        });
    }

}
