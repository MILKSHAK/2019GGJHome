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

    [Header("Dynamic Effects")]
    public float idleLength;

    public float boostLength;

    public float boostSpeedFactor;

    float speedFactor = 1.0f;

    float alphaFactor = 0.0f;

    const float BlendTime = 0.2f;

    List<GameObject> pool = new List<GameObject>();

    GameManager _gameManager; 

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
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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

    void Update() {
        bool boost = GameInput.Boost.Pressing;
        bool dead = _gameManager.isDead;
        if (dead) {
            boost = false;
        }
        
        float targetSpeedFactor = boost ? boostSpeedFactor : 1.0f;
        float targetLength = boost ? boostLength : idleLength;
        float targetAlpha = boost ? 0.9f : 0.5f;

        if (dead) targetLength *= 0.2f;

        speedFactor = Mathf.MoveTowards(speedFactor, targetSpeedFactor, 8.0f * Time.deltaTime);
        length = Mathf.MoveTowards(length, targetLength, 1.0f * Time.deltaTime);
        alphaFactor = Mathf.MoveTowards(alphaFactor, targetAlpha, Time.deltaTime * 0.5f);
    }

    IEnumerator UpdateEntry(GameObject instance, float spd, float lengthScl) {
        var sr = instance.GetComponent<SpriteRenderer>();
            var speedAlphaScl = Mathf.Lerp(0.5f, 1.0f, ((spd - this.spd) / this.spd) / spdRF);
        while (true) {
            var nscl = instance.transform.localScale;
            nscl.x = length * lengthScl;
            instance.transform.localScale = nscl;

            instance.transform.position -= Vector3.right * spd * speedFactor * Time.deltaTime;
            if (instance.transform.position.x < -edge) {
                pool.Add(instance);
                yield break;
            }

            var color = new Color(1.0f, 1.0f, 1.0f, alphaFactor * speedAlphaScl);
            sr.color = color;

            yield return null;
        }
    }

}
