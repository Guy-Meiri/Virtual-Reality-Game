using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopTen
{
    private const int k_NumberOfHighScores = 10;

    private readonly String[] r_RandomNames = { "Guy", "Abdul", "Ethan", "Omer", "Amit", "Arya" ,
        "Yossi", "Robert", "Shoshana", "Patricia", "Cikitita", "Tirtza" ,
        "Hedva", "Tikva" , "Simcha" , "Yafa" , "I_Hate_Moti" , "Moshe" ,"Itzik" ,"Sima" ,"Nechama", "Ruchama", "Latifa", "Larisa" };

    private HighScoreItem[] m_HighScores = new HighScoreItem[k_NumberOfHighScores + 1]; //Extra slot for the current score, for the sort

    public void loadSavedScores()
    {
        for (int i = 0; i <= k_NumberOfHighScores; i++)
        {
            int score = PlayerPrefs.GetInt(i.ToString() + "score");
            string time = PlayerPrefs.GetString(i.ToString() + "time");
            string name = PlayerPrefs.GetString(i.ToString() + "name");
            HighScoreItem currentHighScoreItem = new HighScoreItem(score, name, time);

            m_HighScores[i] = currentHighScoreItem;
        }
    }

    public void UpdateHighScoreList(int i_NewScore)
    {
        loadSavedScores();

        m_HighScores[k_NumberOfHighScores] = new HighScoreItem(i_NewScore, r_RandomNames[(int)UnityEngine.Random.Range(0, r_RandomNames.Length - 1)], DateTime.Now.ToString());

        Array.Sort(m_HighScores);

        for (int i = 0; i <= k_NumberOfHighScores; i++)
        {
            PlayerPrefs.SetInt(i.ToString() + "score", m_HighScores[i].Score);
            PlayerPrefs.SetString(i.ToString() + "time", m_HighScores[i].Time.ToString());
            PlayerPrefs.SetString(i.ToString() + "name", m_HighScores[i].Name);
        }
    }

    public int MaxNumberOfHighScores
    {
        get => k_NumberOfHighScores;
    }

    public HighScoreItem GetHighScoreItemAt(int i)
    {
        HighScoreItem res = null;

        if (i >= 0 && i < k_NumberOfHighScores)
        {
            res = m_HighScores[i];
        }

        return res;
    }

    public class HighScoreItem : IComparable
    {
        private int m_Score;
        private string m_Name;
        private System.String m_Time;

        public HighScoreItem(int i_Score, string i_Name, string i_Time)
        {
            this.m_Score = i_Score;
            this.m_Name = i_Name;
            this.m_Time = i_Time;
        }

        public int Score { get => m_Score; set => m_Score = value; }
        public string Name { get => m_Name; set => m_Name = value; }
        public String Time { get => m_Time; set => m_Time = value; }

        public int CompareTo(object obj)
        {
            HighScoreItem otherHighScore = obj as HighScoreItem;

            if (otherHighScore != null)
            {
                return (-1) * (m_Score - otherHighScore.Score); //For reverse sort!
            }
            else
            {
                return 0;
            }
        }
    }
}
