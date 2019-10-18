using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class InGameUIScript : MonoBehaviour
{
    private MainPlayerScript m_MainPlayerScript;

    [SerializeField]
    private GameObject m_MainPlayer;

    [SerializeField]
    private Image[] m_Hearts;

    [SerializeField]
    private Text m_Score;

    [SerializeField]
    private GameObject m_CrashedAnimationObject;

    [SerializeField]
    private GameObject m_Level2TextObject;

    void Start()
    {
        m_MainPlayerScript = m_MainPlayer.GetComponent<MainPlayerScript>();
        m_MainPlayerScript.NumberOfLivesChanged += OnLivesChange;
        m_MainPlayerScript.ScoreChanged += OnScoreChange;
        m_MainPlayerScript.PlayerCrashed += OnPlayerCrash;
        m_MainPlayerScript.LevelCompleted += OnLevelComplete;
        m_Score.text = "Score: 0";
    }

    public void OnScoreChange(System.Object Sender, System.EventArgs e)
    {
        int i_NewScore = m_MainPlayerScript.Score;
        m_Score.text = "Score: " + i_NewScore;
    }

    public void OnLivesChange(System.Object Sender, System.EventArgs e)
    {
        int i_NewNumberOfLives = m_MainPlayerScript.LivesLeft;

        for (int i = 0; i < m_MainPlayerScript.MaxNumberOfLives; i++)
        {
            m_Hearts[i].color = Color.black;
        }
        for (int i = 0; (i < i_NewNumberOfLives && i < m_MainPlayerScript.MaxNumberOfLives); i++)
        {
            m_Hearts[i].color = Color.red;
        }
    }

    public void OnPlayerCrash(System.Object Sender, System.EventArgs e)
    {
        m_CrashedAnimationObject.SetActive(true);
        StartCoroutine(displayCrashText());
    }

    private IEnumerator displayCrashText()
    {
        yield return new WaitForSeconds(1f);
        m_CrashedAnimationObject.SetActive(false);
    }

    public void OnLevelComplete(System.Object Sender, System.EventArgs e)
    {
        m_Level2TextObject.SetActive(true);
        StartCoroutine(displayLevelCompletedAnimation());
    }

    private IEnumerator displayLevelCompletedAnimation()
    {
        yield return new WaitForSeconds(1f);
        m_Level2TextObject.SetActive(false);
        SceneManager.LoadScene("Level2");
    }
}
