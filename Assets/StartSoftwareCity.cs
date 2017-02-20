﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class StartSoftwareCity : MonoBehaviour {

    private string[] welcomeScenes = { "Controller_WelcomeScene", "Gesten_WelcomeScene" };

    public void StartCity()
    {
        SceneManager.LoadSceneAsync("SoftwareCityScene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("EnvironmentScene", LoadSceneMode.Additive);

        int count = SceneManager.sceneCount;
        for (int i = 0; i < count; i++)
        {
            if (welcomeScenes.Contains(SceneManager.GetSceneAt(i).name))
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            }
        }
        
    }
}
