using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreWindow : MonoBehaviour
{
    private TMP_Text scoreText;
    private void Awake()
    {
        scoreText = transform.Find("scoreText").GetComponent<TMP_Text>();
    }

    private void Update()
    {
        int skor = GameHandler.GetScore();
        scoreText.text = skor.ToString();
    }
}
