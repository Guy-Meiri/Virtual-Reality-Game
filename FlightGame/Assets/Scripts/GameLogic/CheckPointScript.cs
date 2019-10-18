using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    [SerializeField]
    private int m_IndexID;

    private AudioSource m_PassedCheckPointSound;

    [SerializeField]
    private GameObject m_CheckPointManager;

    private CheckPointsManager m_CheckPointManagerScript;

    public AudioSource PassedCheckPointSound { get => m_PassedCheckPointSound; set => m_PassedCheckPointSound = value; }
    public int IndexID { get => m_IndexID; set => m_IndexID = value; }

    void Start()
    {
        PassedCheckPointSound = gameObject.GetComponent<AudioSource>();
        m_CheckPointManagerScript = m_CheckPointManager.GetComponent<CheckPointsManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("MainCamera"))
        {
            m_CheckPointManagerScript.OnCheckPointPassed(this);
        }
    }
}
