using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System;

public class GameOverMenuScript : MonoBehaviour
{
    [SerializeField]
    private Text m_Title;

    [SerializeField]
    private Text m_ScoreText;

    [SerializeField]
    private Text FinalScoreText;

    private TopTen m_Top10Script;

    void Start()
    {
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void SetTitle(string i_Title)
    {
        m_Title.text = i_Title;
    }

    public void OnEnable()
    {
        FinalScoreText.text = "Final " + m_ScoreText.text;
        m_Top10Script = new TopTen();
        m_Top10Script.UpdateHighScoreList(int.Parse(Regex.Replace(m_ScoreText.text, @"[^\d]", "")));
    }
}
