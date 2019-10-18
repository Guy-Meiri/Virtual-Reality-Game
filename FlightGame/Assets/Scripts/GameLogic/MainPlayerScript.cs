using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainPlayerScript : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_ExplosionFx;

    [SerializeField]
    private AudioSource m_CrashSoundFX;

    public event EventHandler ScoreChanged;
    public event EventHandler NumberOfLivesChanged;
    public event EventHandler GameOver;
    public event EventHandler PlayerCrashed;
    public event EventHandler LevelCompleted;

    private const int k_ExplosionForwardOffset = 8;
    [SerializeField]
    private int ScorePerEnemyHit = 10;
    [SerializeField]
    private int m_ScorePerCheckPoint = 15;

    [SerializeField]
    private int m_Score = 0;

    [SerializeField]
    private GameObject m_GameOverPostionObject;

    private float m_LastHitTime;
    private float m_LastCheckPointPassedTime;

    [SerializeField]
    private float m_MinimalTimeBetweenHits;
    [SerializeField]
    private float m_MinimalTimeBetweenCheckPoints;

    private bool m_IsGameOver = false;

    [SerializeField]
    private int m_LivesLeft;
    [SerializeField]
    private AudioSource m_MyGun;

    [SerializeField]
    private GameObject m_PlayerParentObject;

    [SerializeField]
    private int m_MovementSpeed;

    [SerializeField]
    private int m_Damage = 5;

    private int m_MaxNumberOfLives = 3;

    [SerializeField]
    private EnemiesManager m_EnemiesManagerScript;

    [SerializeField]
    AudioClip[] m_CrashSounds;

    public int Score { get => m_Score; set => m_Score = value; }
    public int LivesLeft { get => m_LivesLeft; set => m_LivesLeft = value; }
    public int MaxNumberOfLives { get => m_MaxNumberOfLives; }

    void Start()
    {
        loadPlayerStateFromLastLevel();
        m_LastHitTime = Time.fixedTime;
        m_LastCheckPointPassedTime = Time.fixedTime;
    }

    void Update()
    {
        moveForward();
        checkPlayerAim();
    }

    private void moveForward()
    {
        if (!m_IsGameOver)
        {
            m_PlayerParentObject.transform.position += transform.forward * m_MovementSpeed * Time.deltaTime;
        }
    }

    private void checkPlayerAim()
    {
        if (Input.anyKeyDown)
        {
            RaycastHit hit;

#if UNITY_EDITOR
            Ray myRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));

#else
            Ray myRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 3.5f, Screen.height / 1.85f));
#endif

            if (m_MyGun != null)
            {
                m_MyGun.Play();
            }

            if (Physics.Raycast(myRay, out hit))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.tag.Contains("enemy"))
                    {
                        shootEnemy(hit.collider.gameObject);
                    }
                }
            }
        }
    }

    private void shootEnemy(GameObject i_Enemy)
    {
        if (i_Enemy.GetComponent<EnemyBehavior>().IsAlive)
        {
            m_ExplosionFx.transform.position = i_Enemy.transform.position;
            m_ExplosionFx.Play();
            addToScore(ScorePerEnemyHit);
            m_EnemiesManagerScript.OnHit(i_Enemy, m_Damage);
        }
    }

    private void gameOver()
    {
        if (GameOver != null)
        {
            GameOver.Invoke(this, EventArgs.Empty);
        }

        m_PlayerParentObject.transform.position = m_GameOverPostionObject.transform.position;
        transform.rotation = m_GameOverPostionObject.transform.rotation;

        GlobalControl.Instance.LivesLeft = m_MaxNumberOfLives;
        GlobalControl.Instance.Score = 0;
    }

    private void addToScore(int i_ScoreToAdd)
    {
        m_Score += i_ScoreToAdd;

        if (ScoreChanged != null)
        {
            ScoreChanged.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("enemy") && (Time.fixedTime - m_LastHitTime > m_MinimalTimeBetweenHits))
        {
            m_ExplosionFx.transform.position = transform.position + transform.forward * k_ExplosionForwardOffset;
            m_ExplosionFx.Play();

            EnemyBehavior enemy = collision.gameObject.GetComponent<EnemyBehavior>();
            if (enemy.IsAlive)
            {
                doWhenCrashing();
            }
        }
        else if (collision.gameObject.tag.Contains("terrain"))
        {
            m_PlayerParentObject.transform.position += Vector3.up * 50;
            m_PlayerParentObject.transform.position += Vector3.back * 50;

            notifyPlayerCrash();

            if (Time.fixedTime - m_LastHitTime > m_MinimalTimeBetweenHits)
            {
                doWhenCrashing();
            }
        }
    }

    private void doWhenCrashing()
    {
        m_CrashSoundFX.clip = m_CrashSounds[(int)UnityEngine.Random.Range(0, m_CrashSounds.Length - 1)];
        m_CrashSoundFX.volume = 0.3f;
        m_CrashSoundFX.Play();
        m_LastHitTime = Time.fixedTime;
        removeLife();
    }
     
    private void removeLife()
    {
        if (LivesLeft > 0)
        {
            LivesLeft--;
            notifyNumberOfLivesChange();
        }
        if (LivesLeft <= 0)
        {
            m_IsGameOver = true;
            gameOver();
        }
    }

    private void notifyNumberOfLivesChange()
    {
        if (NumberOfLivesChanged != null)
        {
            NumberOfLivesChanged.Invoke(this, EventArgs.Empty);
        }
    }

    private void notifyPlayerCrash()
    {
        if (PlayerCrashed != null)
        {
            PlayerCrashed.Invoke(this, EventArgs.Empty);
        }
    }

    public void OnCheckPointPassed(CheckPointScript i_CheckPointScript)
    {
        addToScore(m_ScorePerCheckPoint);
    }

    public void OnLevelCompleted()
    {
        savePlayerStateForNextLevel();
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            if (LevelCompleted != null)
            {
                LevelCompleted.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            m_IsGameOver = true;
            gameOver();
        }
    }

    public void AddLife()
    {
        if (m_LivesLeft < m_MaxNumberOfLives)
        {
            m_LivesLeft++;
            notifyNumberOfLivesChange();
        }
    }

    private void loadPlayerStateFromLastLevel()
    {
        m_LivesLeft = GlobalControl.Instance.LivesLeft;
        notifyNumberOfLivesChange();

        m_Score = 0;
        addToScore(GlobalControl.Instance.Score);
    }

    private void savePlayerStateForNextLevel()
    {
        Debug.Log("Saving state");
        GlobalControl.Instance.LivesLeft = m_LivesLeft;
        GlobalControl.Instance.Score = m_Score;
    }
}
