using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrackManager : MonoBehaviour
{
    private AudioSource m_MainTrack;
    
    void Start()
    {
        m_MainTrack = GetComponent<AudioSource>();
        m_MainTrack.Play();
    }
}
