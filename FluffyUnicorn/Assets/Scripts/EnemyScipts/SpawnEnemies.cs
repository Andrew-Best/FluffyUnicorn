using UnityEngine;
using System.Collections;

public class SpawnEnemies : MonoBehaviour 
{
	public GameObject[] mSpawnPos; //List of possible spawn locaations
	public GameObject[] mEnemiesToSpawn; //List of possible enemies to spawn
	public GameObject[] m_Tracks; //The three tracks an enemy can be anchored to

	void Start () 
	{
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
}