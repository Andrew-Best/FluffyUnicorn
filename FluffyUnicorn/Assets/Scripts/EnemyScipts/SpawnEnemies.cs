using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnEnemies : MonoBehaviour 
{
	public GameObject m_EnemyControl;
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

		m_EnemyControl.GetComponent<EnemyControllerScript>().AddBullyToList(newEnemy);
		newEnemy.GetComponent<BullyScript>().InitEnemy(mSpawnPos[row].transform.position, row, newEnemy);

	}

	public void SpawnBoss(int row, string BossName)
	{
		for(int i = 0; i < mEnemiesToSpawn.Length; ++i)
		{
			if(mEnemiesToSpawn[i].name == BossName)
			{
				GameObject newBoss = Objectpooler.Instance.GetObjectForType(mEnemiesToSpawn[i].name, true);//new enemy is created
				newBoss.transform.position = mSpawnPos[row].transform.position; //the enemy's position is assigned the position at the selected row
				if (newBoss.name == "FattestBully")
				{
					newBoss.GetComponent<FattestBully>().InitEnemy(mSpawnPos[row].transform.position, row, newBoss);
					m_EnemyControl.GetComponent<EnemyControllerScript>().AddBullyToList(newBoss);
				}
				else if (newBoss.name == "KingBully")
				{
					newBoss.GetComponent<KingBully>().InitEnemy(mSpawnPos[row].transform.position, row, newBoss);
					m_EnemyControl.GetComponent<EnemyControllerScript>().AddBullyToList(newBoss);
				}
				else
				{
					Debug.Log("No boss.");
				}
			}
		}

		

	}
}