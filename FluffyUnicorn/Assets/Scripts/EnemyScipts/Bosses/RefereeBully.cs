using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefereeBully : BossBaseClass
{
	public GameObject[] m_RefStartPos;
	List<GameObject> m_JockHorde = new List<GameObject>();
	GameObject newJock;
	public GameObject m_EnemySpawner;

	private int maxJockCount_ = 15;
	private int direction_;

	private float horizontalSpeed;
	private float verticalSpeed;

	private float timeUntilNextCharge_;
	public const float DEFAULT_TIME_UNTIL_CHARGE = 10;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (timeUntilNextCharge_ > 0)
		{
			timeUntilNextCharge_ -= Time.deltaTime;
		}
		else
		{
			ChargeTheField();
			timeUntilNextCharge_ = DEFAULT_TIME_UNTIL_CHARGE;
		}
		for (int i = 0; i < m_JockHorde.Count; ++i)
		{
			m_JockHorde[i].GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalSpeed, verticalSpeed);
		}

	}

	void ChargeTheField()
	{	
		//Select the start position of the horde
		int StartPos = Random.Range(0, 7);
		int row = 0;

		if (StartPos == 0 || StartPos == 1 || StartPos == 2)
		{
			row = 0;
		}
		else if (StartPos == 6 || StartPos == 7)
		{
			row = 1;
		}
		else if (StartPos == 3 || StartPos == 4 || StartPos == 5)
		{
			row = 2;
		}

		int DirSelect = Random.Range(0, 3);
		if (DirSelect == 0)
		{
			horizontalSpeed = Constants.HORDE_CHARGE_LEFT_SPEED;
			verticalSpeed = 0;
		}
		if (DirSelect == 1)
		{
			horizontalSpeed = Constants.HORDE_CHARGE_RIGHT_SPEED;
			verticalSpeed = 0;
		}
		if (DirSelect == 2)
		{
			horizontalSpeed = 0;
			verticalSpeed = Constants.HORDE_CHARGE_UP_SPEED;
		}
		if (DirSelect == 3)
		{
			horizontalSpeed = 0;
			verticalSpeed = Constants.HORDE_CHARGE_DOWN_SPEED;
		}
		
		//Create the Jock Horde and fill the list
		for (int i = 0; i < m_EnemySpawner.GetComponent<SpawnEnemies>().mEnemiesToSpawn.Length; ++i)
		{
			string bullyName = m_EnemySpawner.GetComponent<SpawnEnemies>().mEnemiesToSpawn[i].name;
			if(bullyName == "JockBully")
			{
				for (int j = 0; j < maxJockCount_; ++j )
				{
					//m_EnemySpawner.GetComponent<SpawnEnemies>().SpawnEnemyFunc(row, i);
					GameObject newEnemy = Objectpooler.Instance.GetObjectForType(bullyName, true);//new enemy is created
					newEnemy.transform.position = m_RefStartPos[StartPos].transform.position; //the enemy's position is assigned the position at the selected row

					m_JockHorde.Add(newEnemy);
					newEnemy.GetComponent<BullyScript>().InitEnemy(m_RefStartPos[StartPos].transform.position, row, newEnemy);
				}
					
			}
//			
			
/*			m_JockHorde[i] = Objectpooler.Instance.GetObjectForType("JockBully", true);//new enemy is pulled from pool	
			m_JockHorde[i].transform.position = m_RefStartPos[StartPos].transform.position; //the enemy's position is assigned the position at the indexed position

			m_JockHorde[i].GetComponent<BullyScript>().InitEnemy(newJock.transform.position, row, newJock);//new enemy is initialized/spawned	
			//	SpawnJock(StartPos, row);			*/
		}

/*		public void SpawnEnemyFunc(int row, int type)
	{
		GameObject newEnemy = Objectpooler.Instance.GetObjectForType(mEnemiesToSpawn[type].name, true);//new enemy is created
		newEnemy.transform.position = mSpawnPos[row].transform.position; //the enemy's position is assigned the position at the selected row

		m_EnemyControl.GetComponent<EnemyControllerScript>().AddBullyToList(newEnemy);
		newEnemy.GetComponent<BullyScript>().InitEnemy(mSpawnPos[row].transform.position, row, newEnemy);

	}*/
	}

	private void SpawnJock(int StartPos, int row)
	{
		newJock = Objectpooler.Instance.GetObjectForType("JockBully", true);//new enemy is pulled from pool	
		newJock.transform.position = m_RefStartPos[StartPos].transform.position; //the enemy's position is assigned the position at the indexed position

		newJock.GetComponent<BullyScript>().InitEnemy(newJock.transform.position, row, newJock);//new enemy is initialized/spawned	
		
		//Fill the List with Jocks
		m_JockHorde.Add(newJock);
		Debug.Log(m_JockHorde.Count);
	}
}
