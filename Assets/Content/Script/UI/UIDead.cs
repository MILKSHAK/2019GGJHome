using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDead : MonoBehaviour
{

    void OnEnable() {
        EventBus.Subscribe<EventType>(OnEvent);
    }

    void OnDisable() {
        EventBus.Unsubscribe<EventType>(OnEvent);
    }

    void OnEvent(EventType type) {
        if (type == EventType.PlayerDestroy)
            StartCoroutine(ActionDie());
    }

    IEnumerator ActionDie() {
        yield return new WaitForSeconds(1.0f);
        GetComponent<Animator>().SetTrigger("Die");
    }

}
