using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreStart : MonoBehaviour
{
    public bool canStart = false;

    void Update()
    {
        if (canStart && Input.anyKeyDown) {
            SceneManager.LoadScene("GameScene");
        }
    }
}
