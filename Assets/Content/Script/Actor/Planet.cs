using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    bool _dead = false;

    GameManager _gameManger;

    void Start() {
        _gameManger = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        print("OnTriggerEnter2D " + other);
        if (!_dead && other.GetComponentInParent<Sun>()) {
            _dead = true;

            var mysr = GetComponent<SpriteRenderer>();

            var dup = new GameObject();
            dup.transform.SetParent(transform, false);
            var sr = dup.AddComponent<SpriteRenderer>();
            sr.material = _gameManger.planetHeatMaterial;
            sr.sprite = mysr.sprite;
            sr.sortingLayerID = mysr.sortingLayerID;
            sr.sortingOrder = mysr.sortingOrder + 1;
            StartCoroutine(ActionHeatBlendIn(sr));
        }
    }

    IEnumerator ActionHeatBlendIn(SpriteRenderer sr) {
        float elapsed = .0f;
        sr.color = new Color(1, 0.214f, 0, 0);
        while (elapsed < .3f) {
            var nc = sr.color;
            nc.a = Mathf.Min(1.0f, elapsed / .3f);
            sr.color = nc;

            elapsed += Time.deltaTime;
            
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        var instance = Instantiate(_gameManger.explodePrefab, transform.position, Quaternion.identity);
        var nscl = new Vector3(1.2f, 1.2f, 1);
        nscl *= Random.Range(0.8f, 1.0f);
        instance.transform.localScale = nscl;

        Destroy(gameObject);
    }

}
