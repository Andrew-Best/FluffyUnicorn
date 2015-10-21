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
    /// <summary>Name of the door you want to move to</summary>
    public string m_SecretAreaName = "";
    /// <summary>Does this area trigger enemies?</summary>
    public bool m_TriggerEnemies = true;
    /// <summary>Name of this door</summary>
    public string m_DoorName;
    #endregion

    #region private
    /// <summary>Player object</summary>
    private GameObject player_;
    /// <summary>Array of enemies</summary>
    private ArrayList enemyArray_;
    /// <summary>Player Data script</summary>
    private PlayerData playerData_;
    /// <summary>Whether the door is unlocked or not</summary>
    private bool unlockDoor_ = false;
    /// <summary>Game Object for the door you want to go to</summary>
    private GameObject secretDoor_;
    /// <summary>Offset the player by this Vector when moving them to a secret area</summary>
    private Vector3 doorOffset_;
    /// <summary>If the player has not visited this will be false, if they have it will be true and never change back</summary>
    private bool playerVisited_ = false;
    #endregion

	void Start ()
    {
        player_ = GameObject.Find("Player");
        playerData_ = player_.GetComponent<PlayerData>();
        //playerController_ = player_.GetComponent<PlayerController>();
        secretDoor_ = GameObject.Find(m_SecretAreaName);
        doorOffset_ = new Vector3(0.0f, 0.0f, 1.0f);
	}
	
	void Update () 
    {
        RemoveEnemies();
        CanUnlockArea();
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
    void OnTriggerEnter(Collider other)
    {
        //when the player collides with the secret area spawn the number of enemies specified by the variable m_NumEnemies
        if (other.tag == "Player")
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
                    if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
                    {
                        EnterArea();
                    }
                }
            }
        }
    }

    public void EnterArea()
    {
        player_.transform.position = secretDoor_.transform.position - doorOffset_;
        playerVisited_ = true;
    }
    #endregion
}
