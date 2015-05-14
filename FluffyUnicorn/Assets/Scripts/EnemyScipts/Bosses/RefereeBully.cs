using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefereeBully : BossBaseClass 
{
	public GameObject[] m_RefStartPos;
	List<GameObject> m_JockHorde = new List<GameObject>();

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
	void Update () 
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
		for(int i = 0; i < m_JockHorde.Count; ++i)
		{
			m_JockHorde[i].GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalSpeed, verticalSpeed);
		}

	}

	void ChargeTheField()
	{
		GameObject newJock = Objectpooler.Instance.GetObjectForType("JockBully", true);//new enemy is pulled from pool	
		//Select the start position of the horde
		int StartPos = Random.Range(0, 7);
		int row = 0;

		if(StartPos == 0 || StartPos == 1 || StartPos == 2)
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

		int DirSelect = Random.Range(0,3);
		if(DirSelect == 0)
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
		for (int i = 0; i < maxJockCount_; ++i)
		{
			newJock.GetComponent<BullyScript>().InitEnemy(newJock.transform.position, row, newJock);//new enemy is initialized/spawned	
			newJock.transform.position = m_RefStartPos[StartPos].transform.position; //the enemy's position is assigned the position at the indexed position
			

			//Fill the List with Jocks
			m_JockHorde.Add(newJock);
			Debug.Log(m_JockHorde.Count);
		}
		
		//public override void InitEnemy(Vector2 spawnPos, int row, GameObject newBully)
		
	}
}
