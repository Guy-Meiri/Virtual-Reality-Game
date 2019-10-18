using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemiesManager : MonoBehaviour
{
    private HashSet<GameObject> m_EnemySet = new HashSet<GameObject>();

    [SerializeField]
    AudioClip[] m_BombSounds;

    [SerializeField]
    private ParticleSystem m_ExplosionFx;

    [SerializeField]
    private AudioClip m_BulletHitSoundFX;

    [SerializeField]
    private float m_SpawnDistance;

    [SerializeField]
    private Camera m_MainCamera;

    [SerializeField]
    private GameObject m_EnemyPrefab;

    [SerializeField]
    private Vector3 m_MinSpawnDistance;
    [SerializeField]
    private Vector3 m_MaxSpawnDistance;

    [SerializeField]
    private GameObject[] m_EnemyPrefabs;

    [SerializeField]
    private GameObject m_EnemyHolder;

    [SerializeField]
    private GameObject[] m_SpawnLocations;

    private float m_LastSpawnTime;

    [SerializeField]
    private float m_TimeBetweenEnemySpawns;

    [SerializeField]
    private GameObject m_MainPlayer;

    [SerializeField]
    private GameObject m_ExtraLifeManagerObject;

    private ExtraLifeManager m_ExtraLifeManagerScript;

    [SerializeField]
    private float m_HeartDropProbability = 0.1f;

    private int m_SpawnLocationIndex = 0;

    [SerializeField]
    private float m_SecondEnemySpawnProbability;

    void Start()
    {
        m_LastSpawnTime = Time.fixedTime;
        m_ExtraLifeManagerScript = m_ExtraLifeManagerObject.GetComponent<ExtraLifeManager>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level2")
        {
            if (Time.fixedTime - m_LastSpawnTime > m_TimeBetweenEnemySpawns)
            {
                spawnEnemyInFrontOfPlayer();
            }
        }
    }

    private void spawnEnemyInFrontOfPlayer()
    {
        Vector3 playerPos = m_MainCamera.transform.position;
        Vector3 playerDirection = m_MainCamera.transform.forward;
        Quaternion playerRotation = m_MainCamera.transform.rotation;

        Vector3 spawnLocationRandomOffset = new Vector3(Random.Range(m_MinSpawnDistance.x, m_MaxSpawnDistance.x), Random.Range(m_MinSpawnDistance.y, m_MaxSpawnDistance.y), Random.Range(m_MinSpawnDistance.z, m_MaxSpawnDistance.z));

        Vector3 spawnPos = playerPos + playerDirection * m_SpawnDistance + spawnLocationRandomOffset;

        GameObject newEnemy = Instantiate(m_EnemyPrefabs[(int)Random.Range(0, 3.99f)], spawnPos, Quaternion.Euler(m_MainPlayer.transform.forward * -1), m_EnemyHolder.transform);

        newEnemy.GetComponent<EnemyBehavior>().MainPlayer = m_MainCamera.gameObject;
        newEnemy.GetComponent<EnemyBehavior>().MainCamera = m_MainCamera;
        m_LastSpawnTime = Time.fixedTime;
        m_EnemySet.Add(newEnemy);
    }

    public void StopAllEnemies()
    {
        foreach (GameObject enemy in m_EnemySet)
        {
            enemy.SetActive(false);
        }
    }

    public void OnHit(GameObject i_Enemy, int i_Damage)
    {
        if (m_EnemySet.Contains(i_Enemy))
        {
            EnemyBehavior enemyBehaviorScript = i_Enemy.transform.GetComponent<EnemyBehavior>();
            enemyBehaviorScript.HitPoints = enemyBehaviorScript.HitPoints - i_Damage;

            if (enemyBehaviorScript.HitPoints <= 0)
            {
                //The hit killed the enemy
                enemyBehaviorScript.IsAlive = false;
                playEnemySound(i_Enemy, m_BombSounds[(int)Random.Range(0, 2.99f)]);
                m_EnemySet.Remove(i_Enemy);
                handleDropFromEnemy(i_Enemy.transform.position, m_MainCamera.transform.rotation);
                Destroy(i_Enemy, 0.5f);
            }
            else
            {
                //The hit didn't kill the enemy - Play a bullet hitting metal sound effect
                playEnemySound(i_Enemy, m_BulletHitSoundFX);
            }
        }
    }

    private void playEnemySound(GameObject i_Enemy, AudioClip i_AudioClip)
    {
        AudioSource enemyAudioSource = i_Enemy.GetComponent<AudioSource>();
        enemyAudioSource.clip = i_AudioClip;
        enemyAudioSource.volume = 0.17f;
        enemyAudioSource.Play();
    }

    private void handleDropFromEnemy(Vector3 i_Position, Quaternion i_Rotation)
    {
        if (Random.Range(0f, 1f) <= m_HeartDropProbability)
        {
            m_ExtraLifeManagerScript.InstantiateHeartAtPosition(i_Position);
        }
    }

    public void SpawnEnemyInSpawnLocation(int i_CheckPointPassedId)
    {
        //Spawn a new Enemy/ies every second CheckPoint that is passed
        if (i_CheckPointPassedId % 2 == 0)
        {
            instantiateSingleEnemy(m_SpawnLocations[m_SpawnLocationIndex].transform.position);

            if (Random.Range(0f, 1f) <= m_SecondEnemySpawnProbability)
            {
                instantiateSingleEnemy(m_SpawnLocations[m_SpawnLocationIndex].transform.position + new Vector3(1, Random.Range(10f, 20f), Random.Range(2f, 10f)));
            }
            m_SpawnLocationIndex++;
        }
    }

    private void instantiateSingleEnemy(Vector3 i_SpawnPosition)
    {
        GameObject newEnemy = Instantiate(m_EnemyPrefabs[(int)Random.Range(0, 3.99f)], i_SpawnPosition, Quaternion.Euler(m_MainPlayer.transform.forward * -1), m_EnemyHolder.transform);
        newEnemy.GetComponent<EnemyBehavior>().MainPlayer = m_MainCamera.gameObject;
        newEnemy.GetComponent<EnemyBehavior>().MainCamera = m_MainCamera;
        m_EnemySet.Add(newEnemy);
    }
}
