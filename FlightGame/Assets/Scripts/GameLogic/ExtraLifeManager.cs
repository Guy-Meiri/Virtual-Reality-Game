using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExtraLifeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Player;

    private MainPlayerScript m_MainPlayerScript;

    private AudioSource m_ExtraLifeAudioSource;

    [SerializeField]
    private AudioClip m_HeartsFullAudioClip;

    [SerializeField]
    private AudioClip m_ExtraLifeAudioClip;

    [SerializeField]
    private GameObject m_HeartPrefab;

    void Start()
    {
        m_MainPlayerScript = m_Player.GetComponent<MainPlayerScript>();
        m_ExtraLifeAudioSource = GetComponent<AudioSource>();
    }

    public void OnExtraLifeTaken(ExtraLifeScript i_HeartScript)
    {
        if (m_MainPlayerScript.LivesLeft < m_MainPlayerScript.MaxNumberOfLives)
        {
            m_MainPlayerScript.AddLife();
            playExtraLifeSound(m_ExtraLifeAudioClip);
            Destroy(i_HeartScript.gameObject, 0.2f);
        }
        else
        {
            playExtraLifeSound(m_HeartsFullAudioClip);
        }
    }

    private void playExtraLifeSound(AudioClip i_AudioClip)
    {
        m_ExtraLifeAudioSource.clip = i_AudioClip;
        m_ExtraLifeAudioSource.Play();
    }

    public void InstantiateHeartAtPosition(Vector3 i_Position)
    {
        InstantiateHeartAtPosition(i_Position, m_HeartPrefab.transform.rotation);
    }

    public void InstantiateHeartAtPosition(Vector3 i_Position, Quaternion i_Rotation)
    {
        GameObject newHeart = Instantiate(m_HeartPrefab, i_Position, i_Rotation, gameObject.transform);
        ExtraLifeScript extraLifeScript = newHeart.GetComponent<ExtraLifeScript>();
        extraLifeScript.ExtraLifeManagerObject = gameObject;
    }
}
