using UnityEngine;
using System.Collections;

public class SpawnEnemies : MonoBehaviour 
{
	public GameObject[] mSpawnPos;
	public GameObject[] mEnemiesToSpawn;
	public int m_CurRow;


	// Use this for initialization
	void Start () 
	{
		if(mSpawnPos.Length == 0)
		{
			Debug.LogError("Spawn Area needs spawn positions.");
		}
		SpawnEnemyFunc(1);
	}
	
	public void SpawnEnemyFunc(int row)
	{
		m_CurRow = row;
			for (int i = 0; i < mEnemiesToSpawn.Length; ++i)
			{
				GameObject newEnemy = Objectpooler.Instance.GetObjectForType(mEnemiesToSpawn[i].name, true);
				newEnemy.transform.position = mSpawnPos[i].transform.position;
				Debug.Log(mSpawnPos[i].transform.position);
			}
	}


}


/*
	//Old code
	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.gameObject.tag == "Player")
		{
			for(int i =1; i < mEnemiesToSpawn.Length; ++i)
			{
				GameObject newEnemy = Objectpooler.Instance.GetObjectForType(mEnemiesToSpawn[i].name, true);
				newEnemy.transform.position = mSpawnPos[i].transform.position;
				Debug.Log(mSpawnPos[i].transform.position);
			}
			GetComponent<Collider2D>().enabled= false;
		}
	}
*/