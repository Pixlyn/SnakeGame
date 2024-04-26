using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class MainMenuWindow : MonoBehaviour
{
    private enum Sub
    {
        Main,
        HowToPlay,
        Levels
    }
    private void Awake()
    {
        transform.Find("MainMenuSub").GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        transform.Find("HowToPlaySub/HowToPlay").GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        transform.Find("LevelsSub/Levels").GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        
        // Main Menu Buttons
        
        transform.Find("MainMenuSub/HowToPlayButton").GetComponent<Button_UI>().ClickFunc = () => ShowSub(Sub.HowToPlay);
        
        transform.Find("MainMenuSub/PlayButton").GetComponent<Button_UI>().ClickFunc = () => ShowSub(Sub.Levels);

        transform.Find("MainMenuSub/QuitButton").GetComponent<Button_UI>().ClickFunc = () => Application.Quit();
        
        // How To Play Buttons
        
        transform.Find("HowToPlaySub/HowToPlay/ToMenu").GetComponent<Button_UI>().ClickFunc = () => ShowSub(Sub.Main);
        
        // Levels Buttons
        
        transform.Find("LevelsSub/Levels/LvlBtn1").GetComponent<Button_UI>().ClickFunc = () => Loader.Load(Loader.Scene.Lvl1);
        
        transform.Find("LevelsSub/Levels/LvlBtn2").GetComponent<Button_UI>().ClickFunc = () => Loader.Load(Loader.Scene.Lvl2);
        
        transform.Find("LevelsSub/Levels/ToMenu").GetComponent<Button_UI>().ClickFunc = () => ShowSub(Sub.Main);
        
        ShowSub(Sub.Main);
    }

    private void ShowSub(Sub sub)
    {
        transform.Find("MainMenuSub").gameObject.SetActive(false);
        transform.Find("HowToPlaySub").gameObject.SetActive(false);
        transform.Find("LevelsSub").gameObject.SetActive(false);

        switch (sub)
        {
            case Sub.Main:
                transform.Find("MainMenuSub").gameObject.SetActive(true);
                break;
            case Sub.HowToPlay:
                transform.Find("HowToPlaySub").gameObject.SetActive(true);
                break;
            case Sub.Levels:
                transform.Find("LevelsSub").gameObject.SetActive(true);
                break;
        }
    }
}
