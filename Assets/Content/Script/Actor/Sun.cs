using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

    GameManager _gameManager; 
    
    void Start() {
        GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!_gameManager.isDead) {
            EventBus.Post(EventType.PlayerDestroy);
        }
    }

}
