using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScroll : MonoBehaviour
{
    public float scrollSpeed = 3.0f;

    public bool hasLookahead {
        get {
            return transform.parent != null;
        }
    }

    const float LookaheadDistance = 15f;

    GameManager _gm;

    void Awake() {
		_gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (!_gm.started)
            return;

        if (hasLookahead) {
            if (transform.position.x > LookaheadDistance)
                return;
        }
        transform.localPosition += Vector3.left * scrollSpeed * Time.deltaTime;
    }

    void OnDrawGizmos() {
        if (hasLookahead) {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.left * LookaheadDistance);
        }
    }
}
