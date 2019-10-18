using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HowToPlayScript : MonoBehaviour
{
    [SerializeField]
    private GameObject m_MainMenu;

    [SerializeField]
    private Button m_BackButton;
    private Image m_BackButtonImage;
    private AudioSource m_BackButtonAudioSource;

    void Start()
    {
        m_BackButtonImage = m_BackButton.GetComponent<Image>();
        m_BackButtonAudioSource = m_BackButton.GetComponent<AudioSource>();
    }

    void Update()
    {
        RaycastHit hit;
#if UNITY_EDITOR
        Ray myRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
#else
        Ray myRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 3.5f, Screen.height / 1.85f));
#endif
        m_BackButtonImage.color = Color.white;

        if (Physics.Raycast(myRay, out hit))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag.Contains("back"))
                {
                    m_BackButtonImage.color = Color.cyan;
                    if (Input.anyKeyDown)
                    {
                        m_BackButtonAudioSource.Play();
                        m_MainMenu.SetActive(true);
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
