using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenu,
        Loading,
        Lvl1,
        Lvl2
    }

    private static Action loaderCallbackAction;
    
    public static void Load(Scene scene)
    {
        // Set up the callback action that will be triggered after Loading scene is loaded...
        loaderCallbackAction = () =>
        {
            // Load target scene when the Loading scene is loaded...
            SceneManager.LoadScene(scene.ToString());
        };
        
        // Load loading scene ...
        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    public static void LoaderCallback()
    {
        if (loaderCallbackAction != null)
        {
            loaderCallbackAction();
            loaderCallbackAction = null;
        }
    }
}
