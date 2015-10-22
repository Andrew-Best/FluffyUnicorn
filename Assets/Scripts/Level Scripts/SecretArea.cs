using UnityEngine;
using System.Collections;

public class SecretArea : MonoBehaviour
{
    #region public
    public enum EventType
    {
        ENEMIESDEAD = 0,    //all enemies are dead
        ENOUGHMONEY         //have enough money 
    };
    /// <summary>The type of event for this area</summary>
    public EventType m_EventType;
    /// <summary>Spawner that is associated with this secret area</summary>
    public SpawnEnemies m_EnemySpawner;
    /// <summary>Number that represents which enemy to spawn</summary>
    public int m_EnemyType;
    /// <summary>How much money you need to unlock the door if the event type is ENOUGHMONEY</summary>
    public int m_DoorCost;
    /// <summary>Does this area trigger enemies?</summary>
    public bool m_TriggerEnemies = true;
    #endregion

    #region private
    /// <summary>Game Controller Script</summary>
    private GameController gc_;
    /// <summary>Player object</summary>
    private GameObject player_;
    /// <summary>Player Controller script</summary>
    private PlayerController pController_;
    /// <summary>Camera object</summary>
    private GameObject camera_;
    /// <summary>Array of enemies</summary>
    private ArrayList enemyArray_;
    /// <summary>Player Data script</summary>
    private PlayerData playerData_;
    /// <summary>Door which is the entrance</summary>
    private GameObject entrance_;
    /// <summary>Door which is the exit</summary>
    private GameObject exit_;
    /// <summary>Whether the door is unlocked or not</summary>
    private bool unlockDoor_ = false;
    /// <summary>Offset the player by this Vector when moving them to a secret area</summary>
    private Vector3 doorOffset_;
    /// <summary>If the player has not visited this will be false, if they have it will be true and never change back</summary>
    private bool playerVisited_ = false;
    /// <summary>Whether the screen should be fading</summary>
    private bool fade_ = false;
    private float timeBetweenFade_ = 1.0f;
    private float fadeTimer_;
    private bool enterArea_;
    private bool exitArea_;
    #endregion

	void Start ()
    {
        player_ = GameObject.Find("Player");
        camera_ = Camera.main.gameObject;
        playerData_ = player_.GetComponent<PlayerData>();
        pController_ = player_.GetComponent<PlayerController>();
        gc_ = Camera.main.GetComponent<GameController>();

        entrance_ = gameObject.transform.FindChild("Enter").gameObject;
        exit_ = gameObject.transform.FindChild("Exit").gameObject;

        doorOffset_ = new Vector3(0.0f, 0.0f, 2.0f);
	}
	
	void Update () 
    {
        RemoveEnemies();
        CanUnlockArea();
        UpdateFade();
	}

    void UpdateFade()
    {
        if(fade_)
        {
            pController_.enabled = false;
            if (!gc_.m_ScreenFader.m_FadeBlackComplete)
            {
                gc_.m_ScreenFader.EndScene();
            }
            else if(gc_.m_ScreenFader.m_FadeBlackComplete && !gc_.m_ScreenFader.m_FadeClearComplete)
            {
                if(enterArea_)
                {
                    player_.transform.position = exit_.transform.position - doorOffset_;
                    player_.GetComponent<PlayerController>().InSecretArea = true;
                }
                else if(exitArea_)
                {
                    player_.transform.position = entrance_.transform.position - doorOffset_;
                }
                if(fadeTimer_ < timeBetweenFade_)
                {
                    fadeTimer_ += Time.deltaTime;
                }
                if(fadeTimer_ >= timeBetweenFade_)
                {
                    gc_.m_ScreenFader.FadeClear();
                }
            }
            else if(gc_.m_ScreenFader.m_FadeClearComplete && gc_.m_ScreenFader.m_FadeBlackComplete)
            {
                fade_ = false;
                gc_.m_ScreenFader.ResetValues();
                fadeTimer_ = 0.0f;
                pController_.enabled = true;
            }
        }
    }

    void CanUnlockArea()
    {
        if (enemyArray_ != null && m_TriggerEnemies)
        {
            //all enemies are dead
            if (m_EventType == EventType.ENEMIESDEAD && enemyArray_.Count == 0)
            {
                //unlock area
                unlockDoor_ = true;
            } 
        }
        //have the required amount of money to unlock the door
        else if (m_EventType == EventType.ENOUGHMONEY && playerData_.m_Currency >= m_DoorCost)
        {
            unlockDoor_ = true;
        }
    }

    #region Add and Remove enemies
    void SetValues()
    {
        //add the amount of enemies being spawned to a List
        for(int i = 0; i < m_EnemySpawner.mEnemiesToSpawn.Length; ++i)
        {
            enemyArray_.Add(m_EnemySpawner.mEnemiesToSpawn[i]);
        }
    }

    void RemoveEnemies()
    {
        if(m_EnemySpawner != null)
        {
            GameObject tempBully;
            //Loop through the spawner and if any enemy is dead remove the enemy at that index from the List of enemies 
            for (int i = 0; i < m_EnemySpawner.mEnemiesToSpawn.Length; ++i)
            {
                tempBully = m_EnemySpawner.mEnemiesToSpawn[i];
                if (tempBully.GetComponent<BullyScript>().m_IsDead)
                {
                    enemyArray_.RemoveAt(i);
                }
            }
        }  
    }
    #endregion

    #region Collision

    public void Collision(string name)
    {
        if(name == "Enter")
        {
            if (playerVisited_ == false)
            {
                //initial trigger
                if (m_TriggerEnemies)
                {
                    m_TriggerEnemies = false;
                    //loop through the spawner's length and spawn how ever many enemies are in the containier 
                    for (int i = 0; i < m_EnemySpawner.mEnemiesToSpawn.Length; ++i)
                    {
                        //m_EnemySpawner.SpawnEnemyFunc(m_Row, i);
                    }
                    SetValues();    //after everything is spawned add the enemies to a list so you can monitor who is alive and determine when to unlock the door
                }
                //if the player touched the secret area and it is unlocked, move to the secret level
                else if (unlockDoor_)
                {
                    player_.GetComponent<Rigidbody>().velocity = new Vector2(0.0f, 0.0f);
                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        gc_.m_ScreenFader.EndScene();
                        EnterArea();
                    }
                }
            }
        }

        if(name == "Exit")
        {
            if(Input.GetKeyUp(KeyCode.E))
            {
                ExitArea();
            }
        }
    }

    public void EnterArea()
    {
        enterArea_ = true;
        playerVisited_ = true;
        fade_ = true;
    }

    public void ExitArea()
    {
        //player_.transform.position = entrance_.transform.position - doorOffset_;
        exitArea_ = true;
        player_.GetComponent<PlayerController>().InSecretArea = false;
        fade_ = true;
    }
    #endregion
}
