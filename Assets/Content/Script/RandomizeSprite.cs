using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomizeSprite : MonoBehaviour
{
    public Sprite[] sprites;

    SpriteRenderer _sr;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();        
        _sr.sprite = sprites[Random.Range(0, sprites.Length)];
        Destroy(this);
    }

}
