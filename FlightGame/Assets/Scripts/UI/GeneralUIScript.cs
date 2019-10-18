using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralUIScript : MonoBehaviour
{
    [SerializeField]
    private GameObject m_InGameUI;

    [SerializeField]
    private GameObject m_NextTargetArrow;

    [SerializeField]
    private GameObject m_GameOver;

    [SerializeField]
    private GameObject m_GameOverCanvas;
    [SerializeField]
    private GameObject m_MainPlayer;

    [SerializeField]
    private GameObject m_EnemyManager;

    [SerializeField]
    private GameObject m_CheckPointsParentObject;

    private AudioSource m_GameCompletedAudioSource; //Default clip is of failure, changes to m_LevelCompletedSound on success
    [SerializeField]
    private AudioClip m_LevelCompletedSound;

    private GameOverMenuScript m_GameOverScript;
    private MainPlayerScript m_MainPlayerScript;
    private EnemiesManager m_EnemiesManagerScript;
    private CheckPointsManager m_CheckPointManagerScript;

    void Start()
    {
        m_GameOverScript = m_GameOverCanvas.GetComponent<GameOverMenuScript>();
        m_MainPlayerScript = m_MainPlayer.GetComponent<MainPlayerScript>();
        m_MainPlayerScript.GameOver += GameOver; //Subscribe as a listener to the GameOver event
        m_EnemiesManagerScript = m_EnemyManager.GetComponent<EnemiesManager>();
        m_CheckPointManagerScript = m_CheckPointsParentObject.GetComponent<CheckPointsManager>();
        m_GameCompletedAudioSource = GetComponent<AudioSource>();
    }

    public void GameOver(System.Object sender, System.EventArgs e)
    {
        m_NextTargetArrow.SetActive(false);
        m_InGameUI.SetActive(false);
        m_EnemiesManagerScript.StopAllEnemies();
        m_EnemyManager.SetActive(false);

        m_GameOver.SetActive(true);
        m_GameOverScript.enabled = true;

        if ((SceneManager.GetActiveScene().name == "Level2") && (m_CheckPointManagerScript.LevelCompleted))
        {
            m_GameCompletedAudioSource.clip = m_LevelCompletedSound;
        }

        //Play either the default failure sound or success sound (m_LevelCompletedSound)
        m_GameCompletedAudioSource.Play();
    }
}
