﻿using UnityEngine;
using System.Collections;

public class EnemyControllerScript : MonoBehaviour
{
//	public EnemyBaseClass enemyBaseClass;
	public GameObject enemySpawner;
	public float m_EnemySpawnTimer = 3.0f;
//	public GameObject bully;
//	public GameObject testEnemy;

	public int EnemyType = 0;
	// Use this for initialization
	void Start ()
	{
		/*EnemyType = Random.Range(1, 3);
		EnemyType = 1;
		if(EnemyType == 1)
		{
			bully.GetComponent<BullyScript>().SpawnEnemy(1, 1);
		}*/

		enemySpawner.GetComponent<SpawnEnemies>().SpawnEnemyFunc(1);

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}