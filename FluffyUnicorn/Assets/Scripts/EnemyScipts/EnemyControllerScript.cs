using UnityEngine;
using System.Collections;

public class EnemyControllerScript : MonoBehaviour
{
	public GameObject enemySpawner;
	public GameObject bossSpawner;

	public int m_MaxAllowableBulliesOnScreenForLevel; //This can be assigned a default constant value + a modifier for each level
	public int m_BulliesOnScreen;

	private float enemySpawnTimer_;

	private float tempBossTimer_;

	private int enemySelector_;
	private int rowSelector_; //the track the enemy is going to be placed on

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
			enemySelector_ = (int)Random.Range(0, 5);//type of bully that will be spawned
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
			enemySpawner.GetComponent<SpawnEnemies>().SpawnBoss(rowSelector_, 0);
			tempBossTimer_ = 1000;
		}
	}
}

/****************************************************************


******************************************************************/