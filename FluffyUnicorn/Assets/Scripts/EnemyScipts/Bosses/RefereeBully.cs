﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefereeBully : BossBaseClass
{
	public GameObject[] m_RefStartPos;
	List<GameObject> m_JockHorde = new List<GameObject>();
	GameObject newJock;
	public GameObject m_EnemySpawner;
    public EnemyBaseClass m_EnemyBase;

	private int maxJockCount_;
	private int direction_;

	private float horizontalSpeedMax;
	private float horizontalSpeedMin;
	private float verticalSpeedMax;
	private float verticalSpeedMin;

	private float lifeSpan_;
	private float curLifeTime_;

	private float timeUntilNextCharge_ = 8.0f;
	public const float DEFAULT_TIME_UNTIL_CHARGE = 8.0f;

    private bool firstStrike_ = true;

    private GameObject newEnemy;
    public List<GameObject> m_SpawnPoints = new List<GameObject>();
    private float travelTime_ = 1.5f;
    public GameObject m_player;
    private bool isRight_ = false;
    private bool isLeft_ = false;
    private bool isMoving_ = false;
    public Transform m_RefTran;
    private float wait_ = 0.0f;
    private float distance = 0.0f;
    private List<float> lastDistance_ = new List<float>();
   
	// Use this for initialization
	void Start()
	{
		lifeSpan_ = Constants.HORDE_LIFESPAN;
		curLifeTime_ = 0.0f;
		maxJockCount_ = Constants.HORDE_SIZE;
		m_EnemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner"); 
	}

	public override void InitEnemy(Vector2 spawnPos, int row, GameObject newBully)
	{
		base.InitEnemy(spawnPos, row, newBully);

		m_Position = m_ThisBoss.GetComponent<Rigidbody2D>().position;

		m_HP = Constants.REFEREE_BULLY_HP;

		m_Curstate = 0;
		m_BossName = "Referee Bully";
		m_CurFrame = 0;
		this.m_CurRow = row;
		//m_TotalFrames = this.GetComponent<Animator>().framesInAnim;
	}

	// Update is called once per frame
	void Update()
	{
        FacePlayer();
        if(firstStrike_ == true)
        {
            ChargeTheField();
            firstStrike_ = false;
        }
        timeUntilNextCharge_ -= Time.deltaTime;
		if (timeUntilNextCharge_ <= 0.0f)
		{
            MoveRef();
            RecycleBullies();
            timeUntilNextCharge_ = 12.0f;
		}

        for (int i = 0; i < m_JockHorde.Count; ++i)
		{
			m_JockHorde[i].GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(horizontalSpeedMin, horizontalSpeedMax), Random.Range(verticalSpeedMin, verticalSpeedMax));
            //error check for null jocks
            if(m_JockHorde[i] == null)
            {
                m_JockHorde.Remove(m_JockHorde[i]);
            }
		}

        if (gameObject.transform.position.x <= distance)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            isMoving_ = false;
        }

       //travelTime_ = 1 * Time.deltaTime;

		curLifeTime_+= Time.deltaTime;
        wait_ += Time.deltaTime;

		if(curLifeTime_ >= lifeSpan_)
		{
			for (int i = 0; i < m_JockHorde.Count; ++i )
			{
                m_JockHorde[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
			}
            curLifeTime_ = 0.0f;	
		}

	}

    void MoveRef()
    {
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

        gameObject.transform.position = new Vector2(gameObject.transform.position.x, m_EnemyBase.m_TargetPoints[row].transform.position.y);

        if(wait_ >= 1.0 && isMoving_ == false)
        {
            distance = (float)Random.Range(3.0f, -4.0f);
            lastDistance_.Add(distance);
            Debug.Log(distance);
            if (lastDistance_[lastDistance_.Count - 1] >= distance)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1.0f, 0.0f);
            }
            
            if (lastDistance_[lastDistance_.Count - 1] <= distance)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-1.0f, 0.0f);
            }
            isMoving_ = true;
            //transform.Translate(distance * travelTime_ * Time.deltaTime, 0.0f, 0.0f, Space.Self);
            
            wait_ = 0.0f;
           
        }
        else
        {
            isMoving_ = false;
        }

       
        /*transform.position += (m_SpawnPoints[SpawnSpot].transform.position - gameObject.transform.position) * travelTime_ * Time.deltaTime;     
        //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1.0f, 1.0f);
        //gameObject.transform.position = m_SpawnPoints[SpawnSpot].transform.position; 
        
        //gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, m_SpawnPoints[SpawnSpot].transform.position, travelTime_ / 150);
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, m_SpawnPoints[SpawnSpot].transform.position, 300 * Time.deltaTime);
        if( gameObject.transform.position == m_SpawnPoints[SpawnSpot].transform.position)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            Debug.Log("move achieved");
           
        }*/
    }

    void FacePlayer()
    {
        Vector3 toPlayer = gameObject.transform.position - m_Player.transform.position;
        //transform.LookAt(m_Player.transform.position);
        //transform.Rotate(new Vector3(0, -90, 0), Space.Self);
        if (toPlayer.x > 0.0f && isRight_ == false)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            isRight_ = true;
            isLeft_ = false;
        }
        else if (toPlayer.x < 0.0f && isLeft_ == false)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= 1;
            transform.localScale = theScale;
            isLeft_ = true;
            isRight_ = false;
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
                if (bullyName == "JockBully")
                {
                    for (int j = 0; j < maxJockCount_; ++j)
                    {
                        newEnemy = Objectpooler.Instance.GetObjectForType(bullyName, true);//new enemy is created
                        newEnemy.transform.position = m_RefStartPos[StartPos].transform.position; //the enemy's position is assigned the position at the selected row

                        m_JockHorde.Add(newEnemy);
                        newEnemy.GetComponent<BullyScript>().InitEnemy(m_RefStartPos[StartPos].transform.position, row, newEnemy);
                    }

                }
            }
            if (StartPos == 0 || StartPos == 6 || StartPos == 5)
            {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
            if (StartPos == 2 || StartPos == 7 || StartPos == 3)
            {
                Vector3 theScale = transform.localScale;
                theScale.x *= 1;
                transform.localScale = theScale;
            }
	}

    //reuses the bullies by simple moving them back to a new position since their off the screen by the new spawn time, no point in destroying/making new ones
    void RecycleBullies()
    {
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

        for (int i = 0; i < m_EnemySpawner.GetComponent<SpawnEnemies>().mEnemiesToSpawn.Length; ++i)
        {
            string bullyName = m_EnemySpawner.GetComponent<SpawnEnemies>().mEnemiesToSpawn[i].name;
            if (bullyName == "JockBully")
            {
                for (int j = 0; j < m_JockHorde.Count; ++j)
                {
                    m_JockHorde[j].transform.position = gameObject.transform.position;
                   // m_JockHorde[j].transform.position = m_RefStartPos[StartPos].transform.position;
                    m_JockHorde[j].GetComponent<BullyScript>().InitEnemy(gameObject.transform.position, row, newEnemy);

                    if (StartPos == 0 || StartPos == 6 || StartPos == 5)
                    {
                        Vector3 theScale = m_JockHorde[j].transform.localScale;
                        theScale.x *= -1;
                        m_JockHorde[j].transform.localScale = theScale;
                    }
                    if (StartPos == 2 || StartPos == 7 || StartPos == 3)
                    {
                        Vector3 theScale = m_JockHorde[j].transform.localScale;
                        theScale.x *= 1;
                        m_JockHorde[j].transform.localScale = theScale;
                    }
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