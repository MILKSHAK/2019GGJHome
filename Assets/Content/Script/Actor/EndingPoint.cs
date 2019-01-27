using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingPoint : MonoBehaviour
{
    PlayerController _player;
    GameManager _gameManager;

    float _length = 0;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();;
        _length = transform.position.x;
    }

    bool _gameEnded = false;

    void Update()
    {
        if (!_player)
            return;
        var prog = Mathf.Clamp(1 - transform.position.x / _length, 0, 1);
        _gameManager.gameProgress = prog;
        if (prog == 1 && !_gameEnded) {
            _gameEnded = true;
            EventBus.Post(EnumEventType.EscapedSun);
        }
    }
}
