using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLineGenerator : MonoBehaviour {

    public GameObject prefab;

    public float length, lengthRF;

    public float genWait, genWaitRF;

    public float genHeight;

    public float spd, spdRF;
    public float edge;


    const float BlendTime = 0.2f;

    List<GameObject> pool = new List<GameObject>();

    GameObject CreateInstance(Vector2 pos) {
        if (pool.Count == 0) {
            var ret = Instantiate(prefab, transform);
            ret.transform.position = pos;
            return ret;
        }

        GameObject go = pool[pool.Count - 1];
        go.transform.position = pos;
        pool.RemoveAt(pool.Count - 1);
        return go;
    }

    IEnumerator Start() {
        while (true) {
            var myLen = Random.Range(1, 1 + lengthRF) * length;
            var myY = Random.Range(-genHeight, genHeight);
            var instance = CreateInstance(transform.position + new Vector3(0, myY));

            var nscl = instance.transform.localScale;
            nscl.x = myLen;
            instance.transform.localScale = nscl;

            var myspd = Random.Range(1, 1 + spdRF) * spd;
            StartCoroutine(UpdateEntry(instance, myspd, Random.Range(1, 1 + lengthRF)));

            yield return new WaitForSeconds(Random.Range(1, 1 + genWaitRF) * genWait);
        }
    }

    IEnumerator UpdateEntry(GameObject instance, float spd, float lengthScl) {
        while (true) {
            var nscl = instance.transform.localScale;
            nscl.x = length * lengthScl;
            instance.transform.localScale = nscl;

            instance.transform.position += Vector3.right * spd * Time.deltaTime;
            if (instance.transform.position.x > edge) {
                pool.Add(instance);
                yield break;
            }

            yield return null;
        }
    }

}
