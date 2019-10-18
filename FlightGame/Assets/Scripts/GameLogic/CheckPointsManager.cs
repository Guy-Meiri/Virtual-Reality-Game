using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointsManager : MonoBehaviour
{
    [SerializeField]
    private Light m_LightOfCurrentCheckPoint;

    [SerializeField]
    private GameObject[] m_CheckPoints;

    [SerializeField]
    private GameObject m_CheckPointArrow;

    private bool m_LevelCompleted = false;

    private int m_CurrentCheckPointIndex = 0;

    private AudioSource m_FailedToPassCheckPointSound;

    [SerializeField]
    private GameObject m_Player;

    private MainPlayerScript m_MainPlayerScript;

    [SerializeField]
    private GameObject m_EnemiesManagerObject;

    private EnemiesManager m_EnemiesManagerScript;

    void Start()
    {
        m_CurrentCheckPointIndex = 0;
        m_FailedToPassCheckPointSound = GetComponent<AudioSource>();
        m_MainPlayerScript = m_Player.GetComponent<MainPlayerScript>();
        m_EnemiesManagerScript = m_EnemiesManagerObject.GetComponent<EnemiesManager>();
    }

    void Update()
    {
        if (m_CurrentCheckPointIndex < m_CheckPoints.Length && m_CurrentCheckPointIndex >= 0)
        {
            m_CheckPointArrow.transform.LookAt(m_CheckPoints[m_CurrentCheckPointIndex].transform.position);
        }
    }

    public void OnCheckPointPassed(CheckPointScript i_CheckPointScript)
    {
        if (isCheckPointPassable(i_CheckPointScript))
        {
            i_CheckPointScript.PassedCheckPointSound.Play();
            StartCoroutine(delayedDeactivateCheckpoint(1f, i_CheckPointScript));
            m_MainPlayerScript.OnCheckPointPassed(i_CheckPointScript);

            if (SceneManager.GetActiveScene().name == "Level1")
            {
                m_EnemiesManagerScript.SpawnEnemyInSpawnLocation(m_CurrentCheckPointIndex);
            }
            m_CurrentCheckPointIndex++;


            if (m_CurrentCheckPointIndex < m_CheckPoints.Length)
            {
                m_LightOfCurrentCheckPoint.transform.position = m_CheckPoints[m_CurrentCheckPointIndex].transform.position;
            }
            else
            {
                m_LevelCompleted = true;
                m_MainPlayerScript.OnLevelCompleted();
            }

        }
        else
        {
            m_FailedToPassCheckPointSound.Play();
        }
    }

    private bool isCheckPointPassable(CheckPointScript i_CheckPointScript)
    {
        return i_CheckPointScript.IndexID == m_CurrentCheckPointIndex;
    }

    private IEnumerator delayedDeactivateCheckpoint(float i_Time, CheckPointScript i_CheckPointScript)
    {
        yield return new WaitForSeconds(i_Time);
        i_CheckPointScript.gameObject.SetActive(false);
    }

    public bool LevelCompleted { get => m_LevelCompleted; }
}
