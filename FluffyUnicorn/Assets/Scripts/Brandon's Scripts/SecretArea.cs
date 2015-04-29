using UnityEngine;
using System.Collections;

public class SecretArea : MonoBehaviour
{
    #region public
    public SpawnEnemies m_EnemySpawner;
    #endregion

    #region private
    private GameObject player_;
    private ArrayList enemyArray_;
    #endregion

	void Start ()
    {
        player_ = GameObject.Find("Player");
        SetValues();
	}
	
	void Update () 
    {
        RemoveEnemies();
	}

    void SetValues()
    {
        //add the amount of enemies being spawned to a List
        for(int i = 0; i < m_EnemySpawner.mEnemiesToSpawn.Length; ++i)
        {
            enemyArray_.Add(m_EnemySpawner.mEnemiesToSpawn[i]);
        }
    }

    void RemoveEnemies()
    {
        //Loop through the spawner and if any enemy is dead remove the enemy at that index from the List of enemies 
        for (int i = 0; i < m_EnemySpawner.mEnemiesToSpawn.Length; ++i)
        {
            GameObject tempBully = m_EnemySpawner.mEnemiesToSpawn[i];
            if(!tempBully.GetComponent<BullyScript>().m_IsDead)
            {
                enemyArray_.RemoveAt(i);
            }
        }
    }
}
