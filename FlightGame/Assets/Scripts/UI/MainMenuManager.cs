using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_HowToPlayMenu;

    [SerializeField]
    private GameObject m_Top10Menu;

    [SerializeField]
    private Button m_StartButton;

    [SerializeField]
    private Button m_HowToPlayButton;

    [SerializeField]
    private Button m_Top10Button;

    [SerializeField]
    private Button m_ExitButton;

    private Image m_StartButtonImage;
    private Image m_HowToPlayButtonImage;
    private Image m_Top10ButtonImage;
    private Image m_ExitButtonImage;

    private AudioSource m_StartButtonAudioSource;
    private AudioSource m_HowToPlayButtonAudioSource;
    private AudioSource m_Top10ButtonAudioSource;
    private AudioSource m_ExitButtonAudioSource;

    void Start()
    {
        m_StartButtonImage = m_StartButton.GetComponent<Image>();
        m_HowToPlayButtonImage = m_HowToPlayButton.GetComponent<Image>();
        m_Top10ButtonImage = m_Top10Button.GetComponent<Image>();
        m_ExitButtonImage = m_ExitButton.GetComponent<Image>();

        m_StartButtonAudioSource = m_StartButton.GetComponent<AudioSource>();
        m_HowToPlayButtonAudioSource = m_HowToPlayButton.GetComponent<AudioSource>();
        m_Top10ButtonAudioSource = m_Top10Button.GetComponent<AudioSource>();
        m_ExitButtonAudioSource = m_ExitButton.GetComponent<AudioSource>();
    }

    void Update()
    {
        RaycastHit hit;
#if UNITY_EDITOR
        Ray myRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
#else
        Ray myRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 3.5f, Screen.height / 1.85f));
#endif
        clearAllButtons();

        if (Physics.Raycast(myRay, out hit))
        {
            if (hit.collider != null)
            {
                checkButtonChoice(hit.collider.gameObject);
            }
        }
    }

    private void checkButtonChoice(GameObject i_GameObject)
    {
        if (i_GameObject.tag.Contains("start"))
        {
            m_StartButtonImage.color = Color.cyan;

            if (Input.anyKeyDown)
            {
                m_StartButtonAudioSource.Play();
                SceneManager.LoadScene("Level1");
            }
        }
        else if (i_GameObject.tag.Contains("howToPlay"))
        {
            m_HowToPlayButtonImage.color = Color.cyan;

            if (Input.anyKeyDown)
            {
                m_HowToPlayButtonAudioSource.Play();
                m_HowToPlayMenu.SetActive(true);
                gameObject.SetActive(false);
            }
        }
        else if (i_GameObject.tag.Contains("top10"))
        {
            m_Top10ButtonImage.color = Color.cyan;

            if (Input.anyKeyDown)
            {
                m_Top10ButtonAudioSource.Play();
                m_Top10Menu.SetActive(true);
                gameObject.SetActive(false);
            }
        }
        else if (i_GameObject.tag.Contains("exit"))
        {
            m_ExitButtonImage.color = Color.cyan;

            if (Input.anyKeyDown)
            {
                m_ExitButtonAudioSource.Play();
                Application.Quit();
            }
        }
    }

    private void clearAllButtons()
    {
        m_StartButtonImage.color = Color.white;
        m_HowToPlayButtonImage.color = Color.white;
        m_Top10ButtonImage.color = Color.white;
        m_ExitButtonImage.color = Color.white;
    }
}
