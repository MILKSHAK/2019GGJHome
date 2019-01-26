using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour {
    public float time;

    IEnumerator Start() {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

}
