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

    public static bool bossSpawned_ = false;

    private Vector2 startPos_;
    private Vector2 xOffSet_;

	void Start () 
	{
		m_EnemyControl = GameObject.FindGameObjectWithTag("EnemyController"); 
		mSpawnPos[0] = GameObject.FindGameObjectWithTag("ESRL");
		mSpawnPos[1] = GameObject.FindGameObjectWithTag("ESRM");
		mSpawnPos[2] = GameObject.FindGameObjectWithTag("ESRF");
        m_Tracks = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyBaseClass>().m_TargetPoints;
		if(mSpawnPos.Length == 0)
		{
			Debug.LogError("Spawn Area needs spawn positions.");
		}
        xOffSet_ = new Vector2(11.0f, 0.0f);
	}
	
	public void SpawnEnemyFunc(int row, int type)
	{
        int startRow = Random.Range(0, 2);
        if (startRow == 0)
        {
            startPos_ = m_Tracks[0].transform.position;
        }
        if (startRow == 1)
        {
            startPos_ = m_Tracks[1].transform.position;
        }
        if (startRow == 2)
        {
            startPos_ = m_Tracks[2].transform.position;
        }

		GameObject newEnemy = Objectpooler.Instance.GetObjectForType(mEnemiesToSpawn[type].name, true);//new enemy is created
        newEnemy.transform.position = startPos_ + xOffSet_; //mSpawnPos[row].transform.position; //the enemy's position is assigned the position at the selected row

		m_EnemyControl.GetComponent<EnemyControllerScript>().AddBullyToList(newEnemy);
        newEnemy.GetComponent<BullyScript>().InitEnemy(startPos_ + xOffSet_, row, newEnemy);

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
                    bossSpawned_ = true;
				}
				else if (newBoss.name == "RefereeBully")
				{
					newBoss.GetComponent<RefereeBully>().InitEnemy(mSpawnPos[row].transform.position, row, newBoss);
					m_EnemyControl.GetComponent<EnemyControllerScript>().AddBullyToList(newBoss);
                    bossSpawned_ = true;
				}
				else if (newBoss.name == "QueenBully")
				{
					newBoss.GetComponent<QueenBully>().InitEnemy(mSpawnPos[row].transform.position, row, newBoss);
					m_EnemyControl.GetComponent<EnemyControllerScript>().AddBullyToList(newBoss);
                    bossSpawned_ = true;
				}
				else if (newBoss.name == "KingBully")
				{
					newBoss.GetComponent<KingBully>().InitEnemy(mSpawnPos[row].transform.position, row, newBoss);
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