﻿using UnityEngine;
using System.Collections;

public class SpawnEnemies : MonoBehaviour 
{
	public GameObject[] mSpawnPos; //List of possible spawn locaations
	public GameObject[] mEnemiesToSpawn; //List of possible enemies to spawn
	public GameObject[] m_Tracks; //The three tracks an enemy can be anchored to

	public GameObject[] m_Bosses; //List of possible enemies to spawn

	void Start () 
	{
		mSpawnPos[0] = GameObject.FindGameObjectWithTag("ESRL");
		mSpawnPos[1] = GameObject.FindGameObjectWithTag("ESRM");
		mSpawnPos[2] = GameObject.FindGameObjectWithTag("ESRF");
		if(mSpawnPos.Length == 0)
		{
			Debug.LogError("Spawn Area needs spawn positions.");
		}
	}
	
	public void SpawnEnemyFunc(int row, int type)
	{
//		m_Tracks[0] = m_Tracks[row]; //Should make the track be the one passed in by the function
		GameObject newEnemy = Objectpooler.Instance.GetObjectForType(mEnemiesToSpawn[type].name, true);//new enemy is created
		newEnemy.transform.position = mSpawnPos[row].transform.position; //the enemy's position is assigned the position at the selected row
		if (newEnemy.name == "Bully")
		{			
			newEnemy.GetComponent<BullyScript>().InitEnemy(mSpawnPos[row].transform.position, row);
		}	
		else
		{
			newEnemy.GetComponent<BullyScript>().InitEnemy(mSpawnPos[row].transform.position, row);
		}
	}

	public void SpawnBoss(int BossIndex, int row)
	{

		GameObject newBoss = Objectpooler.Instance.GetObjectForType(m_Bosses[BossIndex].name, true);//new enemy is created
		newBoss.transform.position = mSpawnPos[row].transform.position; //the enemy's position is assigned the position at the selected row
		if (newBoss.name == "FattestBully")
		{
			newBoss.GetComponent<FattestBully>().InitEnemy(mSpawnPos[row].transform.position, row);
		}
		else
		{
			Debug.Log("No boss.");
		}

	}
}