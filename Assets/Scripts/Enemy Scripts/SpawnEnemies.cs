using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnEnemies : MonoBehaviour 
{
	public GameObject m_EnemyControl;
	public GameObject[] mSpawnPos; //List of possible spawn locaations
	public GameObject[] mEnemiesToSpawn; //List of possible enemies to spawn
	//public GameObject[] m_Tracks; //The three tracks an enemy can be anchored to

	public GameObject[] m_Bosses; //List of possible enemies to spawn

    public static bool bossSpawned_ = false;

    private Vector3 startPos_;
    private Vector3 xOffSet_;
    private Vector3 zOffSet_;
   

	void Start () 
	{
		m_EnemyControl = GameObject.FindGameObjectWithTag("EnemyController"); 
		mSpawnPos[0] = GameObject.FindGameObjectWithTag("ESRL");
		mSpawnPos[1] = GameObject.FindGameObjectWithTag("ESRM");
		mSpawnPos[2] = GameObject.FindGameObjectWithTag("ESRF");
       // m_Tracks = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyBaseClass>().m_TargetPoints;
		if(mSpawnPos.Length == 0)
		{
			Debug.LogError("Spawn Area needs spawn positions.");
		}
        xOffSet_ = new Vector3(11.0f, 0.0f, 0.0f);
        zOffSet_ = new Vector3(0.0f, 0.0f, 11.0f);
	}
	
	public void SpawnEnemyFunc(Vector3 zOffSet_, int type)
	{
       
		GameObject newEnemy = Objectpooler.Instance.GetObjectForType(mEnemiesToSpawn[type].name, true);//new enemy is created
        newEnemy.transform.position = startPos_ + xOffSet_ + zOffSet_; //mSpawnPos[row].transform.position; //the enemy's position is assigned the position at the selected row
		m_EnemyControl.GetComponent<EnemyControllerScript>().AddBullyToList(newEnemy);
        newEnemy.GetComponent<BullyScript>().InitEnemy(startPos_ + xOffSet_,  zOffSet_, newEnemy);

	}

    public void SpawnBoss(int zOffSet_, string BossName)
	{
		for(int i = 0; i < mEnemiesToSpawn.Length; ++i)
		{
            
			if(mEnemiesToSpawn[i].name == BossName)
			{
				GameObject newBoss = Objectpooler.Instance.GetObjectForType(mEnemiesToSpawn[i].name, true);//new enemy is created
                newBoss.transform.position = mSpawnPos[zOffSet_].transform.position; //the enemy's position is assigned the position at the selected row
				if (newBoss.name == "FattestBully")
				{
					//newBoss.GetComponent<FattestBully>().InitEnemy(mSpawnPos[zOffSet_].transform.position, zOffSet_, newBoss);
					m_EnemyControl.GetComponent<EnemyControllerScript>().AddBullyToList(newBoss);
                    bossSpawned_ = true;
				}
				else if (newBoss.name == "RefereeBully")
				{
					//newBoss.GetComponent<RefereeBully>().InitEnemy(mSpawnPos[zOffSet_].transform.position,zOffSet_, newBoss);
					m_EnemyControl.GetComponent<EnemyControllerScript>().AddBullyToList(newBoss);
                    bossSpawned_ = true;
				}
				else if (newBoss.name == "QueenBully")
				{
					//newBoss.GetComponent<QueenBully>().InitEnemy(mSpawnPos[zOffSet_].transform.position, zOffSet_, newBoss);
					m_EnemyControl.GetComponent<EnemyControllerScript>().AddBullyToList(newBoss);
                    bossSpawned_ = true;
				}
				else if (newBoss.name == "KingBully")
				{
					//newBoss.GetComponent<KingBully>().InitEnemy(mSpawnPos[zOffSet_].transform.position, zOffSet_, newBoss);
					m_EnemyControl.GetComponent<EnemyControllerScript>().AddBullyToList(newBoss);
                    bossSpawned_ = true;
				}
				else
				{
                    bossSpawned_ = false;
					Debug.Log("No boss.");
				}
			}
		}
	}
}