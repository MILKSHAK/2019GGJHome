using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnTriggerEnter : MonoBehaviour
{
    public DeathReason deathReason;

    public GameObject effectPrefab;

    GameManager _gameManager; 

    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<PlayerController>() && !_gameManager.isDead && !_gameManager.isWin) {
            _gameManager.deathReason = deathReason;
            EventBus.Post(EnumEventType.PlayerDestroy);

            if (effectPrefab) {
                GameObject.Instantiate(effectPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
