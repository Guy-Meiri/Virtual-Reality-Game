using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//This script manages the UI aspect of the Top 10 Menu
public class TopTenMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject m_MainMenuObject;

    [SerializeField]
    private GameObject[] m_Rows;

    private TopTen m_TopTenScript = new TopTen(); //The Top 10 logic that manages the top 10 scores

    void Start()
    {
        populateUIScoreGrid();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            exitTop10Menu();
        }
    }

    private void exitTop10Menu()
    {
        m_MainMenuObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void populateUIScoreGrid()
    {
        m_TopTenScript.loadSavedScores();

        for (int i = 0; i < m_TopTenScript.MaxNumberOfHighScores; i++)
        {
            GameObject newRow = m_Rows[i];
            Text[] tArr = newRow.GetComponentsInChildren<Text>();
            TopTen.HighScoreItem currentItem = m_TopTenScript.GetHighScoreItemAt(i);
            foreach (Text t in tArr)
            {
                if (t.name == "Index")
                {
                    t.text = (i + 1).ToString();
                }
                else if (t.name == "Name" && !String.IsNullOrEmpty(currentItem.Name))
                {
                    t.text = currentItem.Name;
                }
                else if (t.name == "Time" && !String.IsNullOrEmpty(currentItem.Time))
                {
                    t.text = currentItem.Time;
                }
                else if (t.name == "Score")
                {
                    t.text = currentItem.Score.ToString();
                }
            }
        }
    }
}