using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgress : MonoBehaviour
{
    GameManager _gameManager;

    public RectTransform earth;

    public Text text;

    void Start()
    {
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        var p = (int) (100 * _gameManager.gameProgress);        
        earth.anchoredPosition = new Vector2(680.0f * p / 100, 0);
        text.text = p.ToString() + "%";
    }
}
