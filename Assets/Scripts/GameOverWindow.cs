using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    private static GameOverWindow instance;

    private void Awake()
    {
        instance = this;
        

        Scene CurrentScene = SceneManager.GetActiveScene();
        
        transform.Find("RetryButton").GetComponent<Button_UI>().ClickFunc = () =>
        {
            SceneManager.LoadScene(CurrentScene.name);
            //Loader.Load(Loader.Scene.Lvl1);
        };
        
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public static void ShowStatic()
    {
        instance.Show();
    }
}
