using UnityEngine;
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
    private float travelTime_ = 1.5f;
    public GameObject m_player;
    private bool isRight_ = false;
    private bool isLeft_ = false;
    private bool isMoving_ = false;
    public Transform m_RefTran;
    private float wait_ = 0.0f;
    private float distance = 0.0f;
    private List<float> lastDistance_ = new List<float>();
    private bool DoneMoving_ = false;
    public AudioClip m_Whistle;
    private Animator RefAnim_;
	// Use this for initialization
	void Start()
	{
		lifeSpan_ = Constants.HORDE_LIFESPAN;
		curLifeTime_ = 0.0f;
		maxJockCount_ = Constants.HORDE_SIZE;
		m_EnemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner");
        lastDistance_.Add(5.0f);
        RefAnim_ = gameObject.GetComponent<Animator>();
       
	}

	/*public override void InitEnemy(Vector2 spawnPos, int row, GameObject newBully)
	{
		base.InitEnemy(spawnPos, row, newBully);

		m_Position = m_ThisBoss.transform.position;

		m_HP = Constants.REFEREE_BULLY_HP;

		m_Curstate = 0;
		m_BossName = "Referee Bully";
		m_CurFrame = 0;
	    m_CurRow = row;
		//m_TotalFrames = this.GetComponent<Animator>().framesInAnim;
	}*/

	void Update()
	{
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

        if (gameObject.transform.position.x <= distance && isMoving_ == true)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            RefAnim_.SetBool("Moving", false);
        }
        
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

        if(wait_ >= 1.0 && isMoving_ == false)
        {
            RefAnim_.SetBool("Moving", true);
            distance = (float)Random.Range(3.0f, -4.0f);
              
            if (distance == lastDistance_[lastDistance_.Count - 1])
            {
                distance = (float)Random.Range(3.0f, -4.0f);
            }
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, m_EnemyBase.m_TargetPoints[row].transform.position.y);
            
            Debug.Log(distance);
            
            if (lastDistance_[lastDistance_.Count - 1] != null)
            {
                if (distance >= lastDistance_[lastDistance_.Count - 1])
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1.5f, 0.0f); //moves right
                    
                    Vector3 theScale = transform.localScale;
                    theScale.x *= -1;
                    transform.localScale = theScale;
                }
                else if (distance <= lastDistance_[lastDistance_.Count - 1])
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-1.5f, 0.0f); //moves left
                    Vector3 theScale = transform.localScale;
                    theScale.x *= 1;
                    transform.localScale = theScale;
                }
            }
            isMoving_ = true;
            lastDistance_.Add(distance);
            wait_ = 0.0f;
        }
        else
        {
            isMoving_ = false;
            RefAnim_.SetBool("Moving", false);
           
        }
        //Vector2 newPos = new Vector2(distance, 0.0f);
        //gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, newPos, 3 * Time.deltaTime);
        //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-1.0f, 0.0f);
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
            
	}

    //reuses the bullies by simple moving them back to a new position since their off the screen by the new spawn time, no point in destroying/making new ones
    void RecycleBullies()
    {
        AudioSource.PlayClipAtPoint(m_Whistle, transform.position);
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
        /*if (StartPos == 0 || StartPos == 6 || StartPos == 5)
        {
            RefAnim_.SetBool("Right", true);
        }
        else
        {
            RefAnim_.SetBool("Right", false);
        }

        if (StartPos == 2 || StartPos == 7 || StartPos == 3)
        {
            RefAnim_.SetBool("Left", true);
        }
        else
        {
            RefAnim_.SetBool("Left", false);
        }
        StartPos = 8;*/
        
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



