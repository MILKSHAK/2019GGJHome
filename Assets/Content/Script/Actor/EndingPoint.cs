using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingPoint : MonoBehaviour
{
    PlayerController _player;
    GameManager _gameManager;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();;
    }

    bool _gameEnded = false;

    void Update()
    {
        var prog = Mathf.Clamp(_player.transform.position.x / transform.position.x, 0, 1);
        _gameManager.gameProgress = prog;
        if (prog == 1 && !_gameEnded) {
            _gameEnded = true;
            EventBus.Post(EnumEventType.EscapedSun);
        }
    }
}
