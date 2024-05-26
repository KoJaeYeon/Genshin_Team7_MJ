using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void GameStart()
    {
        SoundManager.Instance.PlayMainBGM();
        SceneManager.LoadScene("MainGame");
    }
}
