using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private bool m_IsRetreating = false;

    [SerializeField]
    private float m_TimeForSingleRetreat;

    [SerializeField]
    private float m_FlightSpeedFactor;
    [SerializeField]
    private float m_SpawnDistance;

    private Vector3 m_RetreatLocation;

    private float m_RetreatStartTime;

    private bool m_IsAlive = true;

    [SerializeField]
    private int m_HitPoints = 10;

    [SerializeField]
    private GameObject m_MainPlayer;

    [SerializeField]
    private Camera m_MainCamera;

    // Update is called once per frame
    void Update()
    {
        if (m_IsAlive && m_MainPlayer != null)
        {
            if(m_IsRetreating && (Time.fixedTime - m_RetreatStartTime < m_TimeForSingleRetreat))
            {
                //In retreat phase and time allocated for retreat has not elapsed
                transform.position = Vector3.Lerp(transform.position, m_RetreatLocation, Time.deltaTime / m_FlightSpeedFactor);
                transform.LookAt(m_RetreatLocation);
            }
            else
            {
                //Either the retreat phase is over or the enemy is not retreating
                m_IsRetreating = false;
                //Attemt to crash into the player
                transform.position = Vector3.MoveTowards(transform.position, m_MainPlayer.transform.position, Time.deltaTime * 40);
                transform.LookAt(m_MainCamera.transform);
            }
        }
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("MainCamera") && (m_IsRetreating == false))
        {
            beginTheRetreatPhase();
        }
    }

    private void beginTheRetreatPhase()
    {
        m_IsRetreating = true;

        Vector3 playerPos = m_MainCamera.transform.position;
        Vector3 playerDirection = m_MainCamera.transform.forward;
        Quaternion playerRotation = m_MainCamera.transform.rotation;

        Vector3 retreatDestination = playerPos + playerDirection * m_SpawnDistance;

        m_RetreatLocation = retreatDestination;
        m_RetreatStartTime = Time.fixedTime;
    }

    public GameObject MainPlayer { get => m_MainPlayer; set => m_MainPlayer = value; }
    public int HitPoints { get => m_HitPoints; set => m_HitPoints = value; }
    public bool IsAlive { get => m_IsAlive; set => m_IsAlive = value; }
    public Camera MainCamera { get => m_MainCamera; set => m_MainCamera = value; }
}
