using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : Singleton<SceneLoader>
{
    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
