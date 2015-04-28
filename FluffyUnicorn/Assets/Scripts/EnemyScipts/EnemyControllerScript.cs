using UnityEngine;
using System.Collections;

public class EnemyControllerScript : MonoBehaviour
{
	public GameObject enemySpawner;
	public float m_EnemySpawnTimer = 3.0f;

	public int EnemyType = 0;

	void Start ()
	{
		int enemySelector = (int) Random.Range(0, 5);
		int rowSelector = Random.Range(0, 3); //the track the enemy is going to be placed on

		enemySelector = 0;

		//public void SpawnEnemyFunc(int row, int type)
		enemySpawner.GetComponent<SpawnEnemies>().SpawnEnemyFunc(rowSelector, enemySelector);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
