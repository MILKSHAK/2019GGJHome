using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuStart : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);
        yield return new WaitUntil(() => Input.anyKeyDown);
        
        SceneManager.LoadScene("PreScene");
    }

}
