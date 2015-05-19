using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossBaseClass : EnemyBaseClass
{
	public GameObject m_ThisBoss;
	public Vector2 m_Position;

	public GameObject[] m_SpawnPos; //List of possible spawn locaations

	public int m_Curstate;

	public string m_BossName;

	public int m_CurFrame;
	public int m_TotalFrames;

	public override void InitEnemy(Vector2 spawnPos, int row, GameObject newBully)
	{
		base.InitEnemy(spawnPos, row, m_ThisBoss);
		m_Player = GameObject.FindGameObjectWithTag("Player");
		m_EnemyController = GameObject.FindGameObjectWithTag("EnemyController");
		this.m_ThisBoss.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);//Set Boss Velocity to 0
		this.m_IsABoss = true;
	}

	void Update()
	{
		List<GameObject> enemiesInList = m_EnemyController.GetComponent<EnemyControllerScript>().m_Bullies;

		for (int i = 0; i < enemiesInList.Count; ++i)
		{
			if (enemiesInList[i].name == "KingBully")
			{
				enemiesInList[i].GetComponent<KingBully>().KingUpdate();
			}
			if (enemiesInList[i].name == "FattestBully")
			{
				enemiesInList[i].GetComponent<FattestBully>().FattestUpdate();
			}

			if(enemiesInList[i].GetComponent<BossBaseClass>().m_HP <= 0)
			{
				Destroy(enemiesInList[i]);
			}
		}
	}
}
