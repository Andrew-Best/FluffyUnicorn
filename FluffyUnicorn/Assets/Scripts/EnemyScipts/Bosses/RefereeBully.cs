using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefereeBully : BossBaseClass
{
	public GameObject[] m_RefStartPos;
	List<GameObject> m_JockHorde = new List<GameObject>();
	GameObject newJock;
	public GameObject m_EnemySpawner;

	private int maxJockCount_;
	private int direction_;

	private float horizontalSpeedMax;
	private float horizontalSpeedMin;
	private float verticalSpeedMax;
	private float verticalSpeedMin;

	private float lifeSpan_;
	private float curLifeTime_;

	private float timeUntilNextCharge_;
	public const float DEFAULT_TIME_UNTIL_CHARGE = 10;

	// Use this for initialization
	void Start()
	{
		lifeSpan_ = Constants.HORDE_LIFESPAN;
		curLifeTime_ = 0;
		maxJockCount_ = Constants.HORDE_SIZE;
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
			m_JockHorde[i].GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(horizontalSpeedMin, horizontalSpeedMax), Random.Range(verticalSpeedMin, verticalSpeedMax));
		}

		curLifeTime_+= Time.deltaTime;

		if(curLifeTime_ >= lifeSpan_)
		{
			for (int i = 0; i < m_JockHorde.Count; ++i )
			{
				Destroy(m_JockHorde[i]);
				m_JockHorde.Remove(m_JockHorde[i]);
			}
				
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
		SelectDirection(StartPos, row);
	
		//Create the Jock Horde and fill the list
		for (int i = 0; i < m_EnemySpawner.GetComponent<SpawnEnemies>().mEnemiesToSpawn.Length; ++i)
		{
			string bullyName = m_EnemySpawner.GetComponent<SpawnEnemies>().mEnemiesToSpawn[i].name;
			if(bullyName == "JockBully")
			{
				for (int j = 0; j < maxJockCount_; ++j )
				{
					GameObject newEnemy = Objectpooler.Instance.GetObjectForType(bullyName, true);//new enemy is created
					newEnemy.transform.position = m_RefStartPos[StartPos].transform.position; //the enemy's position is assigned the position at the selected row

					m_JockHorde.Add(newEnemy);
					newEnemy.GetComponent<BullyScript>().InitEnemy(m_RefStartPos[StartPos].transform.position, row, newEnemy);
				}
					
			}
		}
	}

	private void SelectDirection(int StartPos, int row)
	{
		if(StartPos == 0 || StartPos == 6 || StartPos ==5)
		{
			horizontalSpeedMin = Constants.HORDE_CHARGE_RIGHT_SPEED;
			verticalSpeedMin = 0;
			horizontalSpeedMax = Constants.HORDE_CHARGE_RIGHT_SPEED * Constants.HORDE_MAX_SPEED_MOD;
			verticalSpeedMax = 0;
		}
		if(StartPos == 2 || StartPos == 7 || StartPos == 3)
		{
			horizontalSpeedMin = Constants.HORDE_CHARGE_LEFT_SPEED;
			verticalSpeedMin = 0;
			horizontalSpeedMax = Constants.HORDE_CHARGE_LEFT_SPEED * Constants.HORDE_MAX_SPEED_MOD;
			verticalSpeedMax = 0;
		}
		if(StartPos == 1)
		{
			horizontalSpeedMin = 0;
			verticalSpeedMin = Constants.HORDE_CHARGE_DOWN_SPEED;
			horizontalSpeedMax = 0;
			verticalSpeedMax = Constants.HORDE_CHARGE_DOWN_SPEED * Constants.HORDE_MAX_SPEED_MOD;
		}
		if(StartPos == 4)
		{
			horizontalSpeedMin = 0;
			verticalSpeedMin = Constants.HORDE_CHARGE_UP_SPEED;
			horizontalSpeedMax = 0;
			verticalSpeedMax = Constants.HORDE_CHARGE_UP_SPEED * Constants.HORDE_MAX_SPEED_MOD;
		}
	}
}

/*		int DirSelect = Random.Range(0, 3);
		if (DirSelect == 0)
		{
			horizontalSpeedMin = Constants.HORDE_CHARGE_LEFT_SPEED;
			verticalSpeedMin = 0;
			horizontalSpeedMax = Constants.HORDE_CHARGE_LEFT_SPEED * Constants.HORDE_MAX_SPEED_MOD;
			verticalSpeedMax = 0;
		}
		if (DirSelect == 1)
		{
			horizontalSpeedMin = Constants.HORDE_CHARGE_RIGHT_SPEED;
			verticalSpeedMin = 0;
			horizontalSpeedMax = Constants.HORDE_CHARGE_RIGHT_SPEED * Constants.HORDE_MAX_SPEED_MOD;
			verticalSpeedMax = 0;
		}
		if (DirSelect == 2)
		{
			horizontalSpeedMin = 0;
			verticalSpeedMin = Constants.HORDE_CHARGE_UP_SPEED;
			horizontalSpeedMax = 0;
			verticalSpeedMax = Constants.HORDE_CHARGE_UP_SPEED * Constants.HORDE_MAX_SPEED_MOD;
		}
		if (DirSelect == 3)
		{
			horizontalSpeedMin = 0;
			verticalSpeedMin = Constants.HORDE_CHARGE_DOWN_SPEED;
			horizontalSpeedMax = 0;
			verticalSpeedMax = Constants.HORDE_CHARGE_DOWN_SPEED * Constants.HORDE_MAX_SPEED_MOD;
		}*/