using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDead : MonoBehaviour
{

    void OnEnable() {
        EventBus.Subscribe<EnumEventType>(OnEvent);
    }

    void OnDisable() {
        EventBus.Unsubscribe<EnumEventType>(OnEvent);
    }

    void OnEvent(EnumEventType type) {
        if (type == EnumEventType.PlayerDestroy)
            StartCoroutine(ActionDie());
    }

    IEnumerator ActionDie() {
        yield return new WaitForSeconds(1.0f);
        GetComponent<Animator>().SetTrigger("Die");
    }

}
