﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnEnemies : MonoBehaviour 
{
	public GameObject m_EnemyControl;
	public GameObject[] mEnemiesToSpawn; //List of possible enemies to spawn
	
	public GameObject[] m_Bosses; //List of possible enemies to spawn

    public static bool bossSpawned_ = false;

    private Vector3 startPos_;
    private Vector3 xOffSet_;
   // private Vector3 zOffSet_;
    private Vector3 enemyPos;
   

	void Start () 
	{
		m_EnemyControl = GameObject.FindGameObjectWithTag("EnemyController"); 

        xOffSet_ = new Vector3(11.0f, 0.0f, 0.0f);
      //  zOffSet_ = new Vector3(0.0f, 0.0f, 11.0f);
        enemyPos = new Vector3(0.0f, 2.0f, 0.0f);
	}

    public void SpawnEnemyFunc(Vector3 enemyPos, int type)
	{
       
		GameObject newEnemy = Objectpooler.Instance.GetObjectForType(mEnemiesToSpawn[type].name, true);//new enemy is created
        newEnemy.transform.position = startPos_ + xOffSet_ + enemyPos; //mSpawnPos[row].transform.position; //the enemy's position is assigned the position at the selected row
		m_EnemyControl.GetComponent<EnemyControllerScript>().AddBullyToList(newEnemy);
        newEnemy.GetComponent<BullyScript>().InitEnemy(startPos_ + xOffSet_,  enemyPos, newEnemy);
        

	}

    public void SpawnBoss(Vector3 enemyPos, string BossName)
	{
        for (int i = 0; i < mEnemiesToSpawn.Length; ++i)
		{
            
			if(mEnemiesToSpawn[i].name == BossName)
			{
				GameObject newBoss = Objectpooler.Instance.GetObjectForType(mEnemiesToSpawn[i].name, true);//new enemy is created
                

                newBoss.transform.position = startPos_ + xOffSet_ + enemyPos; 
				if (newBoss.name == "FattestBully")
				{
					newBoss.GetComponent<FattestBully>().InitEnemy(new Vector3(0.0f, 0.0f, 0.0f), enemyPos, newBoss);
					m_EnemyControl.GetComponent<EnemyControllerScript>().AddBullyToList(newBoss);
                    bossSpawned_ = true;
				}
				else if (newBoss.name == "RefereeBully")
				{
                    newBoss.GetComponent<RefereeBully>().InitEnemy(new Vector3(0.0f, 0.0f, 0.0f), enemyPos, newBoss);
					m_EnemyControl.GetComponent<EnemyControllerScript>().AddBullyToList(newBoss);
                    bossSpawned_ = true;
				}
				else if (newBoss.name == "QueenBully")
				{
                    newBoss.GetComponent<QueenBully>().InitEnemy(new Vector3(0.0f, 0.0f, 0.0f), enemyPos, newBoss);
					m_EnemyControl.GetComponent<EnemyControllerScript>().AddBullyToList(newBoss);
                    bossSpawned_ = true;
				}
				else if (newBoss.name == "KingBully")
				{
                    newBoss.GetComponent<KingBully>().InitEnemy(new Vector3(0.0f, 0.0f, 0.0f), enemyPos, newBoss);
					m_EnemyControl.GetComponent<EnemyControllerScript>().AddBullyToList(newBoss);
                    bossSpawned_ = true;
				}
				else
				{
                    bossSpawned_ = false;
					Debug.Log("No boss.");
				}
                //newBoss.GetComponent<BullyScript>().InitEnemy(startPos_ + xOffSet_, enemyPos, newBoss);
                //newBoss.GetComponent<FattestBully>().InitEnemy(startPos_ + xOffSet_, enemyPos, newBoss);
			}
		}
	}
}