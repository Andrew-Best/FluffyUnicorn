using UnityEngine;
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
	private int rowSelector_; //the track the enemy is going to be placed on

	public void AddBullyToList(GameObject bully)
	{
		m_Bullies.Add(bully);
	}

	void Start()
	{
		enemySpawnTimer_ = Constants.ENEMY_SPAWN_TIMER_MAX;
		tempBossTimer_ = Constants.ENEMY_SPAWN_TIMER_MAX;

		m_MaxAllowableBulliesOnScreenForLevel = 5; //For testing purposes only
	}

	// Update is called once per frame
	void Update()
	{

		if (m_BulliesOnScreen < m_MaxAllowableBulliesOnScreenForLevel)
		{
			enemySelector_ = (int)Random.Range(0, 4);//type of bully that will be spawned
			rowSelector_ = Random.Range(0, 2); //the track the enemy is going to be placed on

			enemySpawnTimer_ -= Time.deltaTime;
			if (enemySpawnTimer_ <= 0)
			{
				enemySpawner.GetComponent<SpawnEnemies>().SpawnEnemyFunc(rowSelector_, enemySelector_);
				enemySpawnTimer_ = Constants.ENEMY_SPAWN_TIMER_MAX;
				m_BulliesOnScreen++;
				
			}
		}
		tempBossTimer_ -= Time.deltaTime;
		if(tempBossTimer_ <= 0)
		{
			string BossName;

			int BossSelectorRange = Random.Range(0, 10);
			int BossSelector=0;

			if (BossSelectorRange > 5)
			{
				//BossSelector = FattestBullyIndex;
				BossName = "FattestBully";
			}
			else
			{
				//BossSelector = KingBullyIndex;
				BossName = "KingBully";
			}

//			enemySpawner.GetComponent<SpawnEnemies>().SpawnBoss(rowSelector_, BossSelector, BossName);//Index for Each Boss
			tempBossTimer_ = 1000;
		}
	}
}

/****************************************************************


******************************************************************/