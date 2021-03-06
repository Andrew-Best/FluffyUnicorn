﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyControllerScript : MonoBehaviour
{
	public GameObject enemySpawner;
	public GameObject bossSpawner;

	public List<GameObject> m_Bullies = new List<GameObject>();

	public int m_MaxAllowableBulliesOnScreenForLevel; //This can be assigned a default constant value + a modifier for each level
	public int m_BulliesOnScreen;

	private float enemySpawnTimer_;
	private float tempBossTimer_;

	private int enemySelector_;

    private Vector3 enemyPos;
    private bool bossSpawned_ = false;


	public void AddBullyToList(GameObject bully)
	{
		m_Bullies.Add(bully);
	}

	void Start()
	{
		enemySpawnTimer_ = Constants.ENEMY_SPAWN_TIMER_MAX;
		tempBossTimer_ = Constants.ENEMY_SPAWN_TIMER_MAX;

		m_MaxAllowableBulliesOnScreenForLevel = 2; //For testing purposes only
	}

	// Update is called once per frame
	void Update()
	{
		m_BulliesOnScreen = m_Bullies.Count;

		if (m_BulliesOnScreen < m_MaxAllowableBulliesOnScreenForLevel)
		{
			enemySelector_ = (int)Random.Range(0, 6);//type of bully that will be spawned
			enemySpawnTimer_ -= Time.deltaTime;
			if (enemySpawnTimer_ <= 0)
			{
                enemySpawner.GetComponent<SpawnEnemies>().SpawnEnemyFunc(enemyPos, 0);
				enemySpawnTimer_ = Constants.ENEMY_SPAWN_TIMER_MAX;				
			}
		}
        if (m_Bullies.Count > 6)
        {
            enemySpawner.GetComponent<SpawnEnemies>().SpawnBoss(enemyPos, "RefereeBully");
            
        }
        
		tempBossTimer_ -= Time.deltaTime;
		if(tempBossTimer_ <= 0 && !bossSpawned_)
		{
			string BossName;
            bossSpawned_ = true;
			int BossSelectorRange = Random.Range(0, 30);
			int BossSelector=0;

			if (BossSelectorRange < 11)
			{
				//BossSelector = FattestBullyIndex;
				BossName = "FattestBully";
			}
			else if(BossSelectorRange < 21)
			{
				//BossSelector = KingBullyIndex;
				BossName = "KingBully";
			}
			else
			{
				BossName = "RefereeBully";
			}
            enemySpawner.GetComponent<SpawnEnemies>().SpawnBoss(enemyPos, "RefereeBully");
            //bossSpawner.GetComponent<SpawnEnemies>().SpawnBoss(enemyPos, BossName);//Index for Each Boss
            
			tempBossTimer_ = 100;
		}
	}
}

/****************************************************************


******************************************************************/