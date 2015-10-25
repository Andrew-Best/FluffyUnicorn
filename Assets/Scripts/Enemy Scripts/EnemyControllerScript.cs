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
			enemySelector_ = (int)Random.Range(0, 4);//type of bully that will be spawned
			enemySpawnTimer_ -= Time.deltaTime;
			if (enemySpawnTimer_ <= 0)
			{
                enemySpawner.GetComponent<SpawnEnemies>().SpawnEnemyFunc(new Vector3(0, 0, 0), 0);
				enemySpawnTimer_ = Constants.ENEMY_SPAWN_TIMER_MAX;				
			}
		}
		tempBossTimer_ -= Time.deltaTime;
		if(tempBossTimer_ <= 0)
		{
			string BossName;

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
          //  enemySpawner.GetComponent<SpawnEnemies>().SpawnBoss(new Vector3(0, 0, 0), "RefereeBully");
            bossSpawner.GetComponent<SpawnEnemies>().SpawnBoss(new Vector3(0, 0, 0), BossName);//Index for Each Boss
			tempBossTimer_ = 1000;
		}
	}
}

/****************************************************************


******************************************************************/