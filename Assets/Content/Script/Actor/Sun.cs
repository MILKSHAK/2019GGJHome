using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

    public float conoralTriggerIntervalMin;
    public float conoralTriggerIntervalMax;

    public float conoralDelayAfterPre;

    public GameObject prefabConoralPre;

    public GameObject prefabConoral;

    public Transform conoralCenter;

    GameManager _gameManager; 
    
    void Start() {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(ActionConoral());
    }

    IEnumerator ActionConoral() {
        while (true) {
            yield return new WaitForSeconds(
                Random.Range(conoralTriggerIntervalMin, conoralTriggerIntervalMax)
            );
            print("???");

            var rot = Quaternion.Euler(0, 90, 0);
            Instantiate(prefabConoralPre, conoralCenter.transform.position, rot);

            yield return new WaitForSeconds(conoralDelayAfterPre);

            Instantiate(prefabConoral, conoralCenter.transform.position, rot);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!_gameManager.isDead) {
            print("Dead!!" + other);
            EventBus.Post(EventType.PlayerDestroy);
        }
    }

}
