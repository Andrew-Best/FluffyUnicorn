﻿using UnityEngine;
using System.Collections;

public class SecretArea : MonoBehaviour
{
    #region public
    public SpawnEnemies m_EnemySpawner;     //drag the spawner that is associated with the secret area into this variable
    public int m_Row;                       //which row to spawn the enemies on 
    public int m_EnemyType;                 //number that represents which enemy to spawn
    public string m_SecretAreaName = "";    //name of scene yoiu want to move to 
    #endregion

    #region private
    private GameObject player_;
    private ArrayList enemyArray_;
    private bool triggerEnemies_ = true;
    private bool unlockDoor_ = false;
    #endregion

	void Start ()
    {
        player_ = GameObject.Find("Player");
   
	}
	
	void Update () 
    {
        RemoveEnemies();
        CanUnlockArea();
	}

    void CanUnlockArea()
    {
        if (enemyArray_ != null && enemyArray_.Count == 0)
        {
            //unlock area
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
    void OnTriggerEnter2D(Collider2D other)
    {
        //when the player collides with the secret area spawn the number of enemies specified by the variable m_NumEnemies
        if (other.tag == "Player")
        {
            //initial trigger
            if (triggerEnemies_)
            {
                triggerEnemies_ = false;
                //loop through the spawner's length and spawn how ever many enemies are in the containier 
                for (int i = 0; i < m_EnemySpawner.mEnemiesToSpawn.Length; ++i)
                {
                    m_EnemySpawner.SpawnEnemyFunc(m_Row, i);
                }
                SetValues();    //after everything is spawned add the enemies to a list so you can monitor who is alive and determine when to unlock the door
            }
            //if the player touched the secret area and it is unlocked, move to the secret level
            else if(unlockDoor_)
            {
                EnterSecretArea(m_SecretAreaName);
            }
        }
    }

    void EnterSecretArea(string areaName)
    {
        //load scene based on name 
        Application.LoadLevel(areaName);
    }
    #endregion
}
