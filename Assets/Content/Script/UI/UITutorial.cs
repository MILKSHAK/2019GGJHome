using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITutorial : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);
        yield return new WaitUntil(() => GameInput.Shoot.Down);
        GameObject.Find("GameManager").GetComponent<GameManager>().started = true;
        Destroy(gameObject);
    }

}
