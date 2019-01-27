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
		if (type == EnumEventType.EscapedSun)
		{
			StartCoroutine(ActionWin());
		}
    }

    IEnumerator ActionDie() {
        yield return new WaitForSeconds(1.0f);
        GetComponent<Animator>().SetTrigger("Die");
    }

	IEnumerator ActionWin()
	{
		yield return new WaitForSeconds(7.0f);
		GetComponent<Animator>().SetTrigger("Win");
	}

}
