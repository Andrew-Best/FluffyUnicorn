using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefereeBully : BossBaseClass
{
    public GameObject[] m_RefStartPos;
    public List<GameObject> m_JockHorde = new List<GameObject>();
    public EnemyBaseClass m_EnemyBase;

    public AudioClip m_Whistle;

    private int maxJockCount_;
    private int direction_;

    private float horizontalSpeedMax;
    private float horizontalSpeedMin;
    private float verticalSpeedMax;
    private float verticalSpeedMin;
    private float lifeSpan_;
    private float curLifeTime_;
    private float wait_ = 0.0f;
    private float distance = 0.0f;
    private float timeUntilNextCharge_ = 8.0f;
        
    private SpawnEnemies EnemySpawner_;
    private GameObject newEnemy;
    private GameObject newJock;

    private List<float> lastDistance_ = new List<float>();
    private List<GameObject> targetPoints_ = new List<GameObject>();
   
    private bool isMoving_ = false;
    private bool firstStrike_ = true;
    
    private Animator refAnim_;
    private Rigidbody velocity_;
    private Rigidbody jockRigid_;

    
    void Start()
    {
        lifeSpan_ = Constants.HORDE_LIFESPAN;
        curLifeTime_ = 0.0f;
        maxJockCount_ = Constants.HORDE_SIZE;
        EnemySpawner_ = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<SpawnEnemies>();
        lastDistance_.Add(5.0f);
        refAnim_ = gameObject.GetComponent<Animator>();
        velocity_ = gameObject.GetComponent<Rigidbody>();

        for (int i = 0; i < 3; ++i)
        {
            targetPoints_.Add(GameObject.FindGameObjectWithTag("Targetpoint" + i));
        }    

    }

    public override void InitEnemy(Vector3 spawnPos, Vector3 zOffSet_, GameObject newBully)
    {
        base.InitEnemy(spawnPos, zOffSet_, newBully);

        m_Position = m_ThisBoss.transform.position;

        m_HP = Constants.REFEREE_BULLY_HP;

        m_Curstate = 0;
        m_BossName = "Referee Bully";
        m_CurFrame = 0;
      //  m_CurRow = row;
        //m_TotalFrames = this.GetComponent<Animator>().framesInAnim;
    }

    void Update()
    {
        if (firstStrike_ == true)
        {
            SetJocks();
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
            m_JockHorde[i].GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(horizontalSpeedMin, horizontalSpeedMax), Random.Range(verticalSpeedMin, verticalSpeedMax));
            //error check for null jocks
            if (m_JockHorde[i] == null)
            {
                m_JockHorde.Remove(m_JockHorde[i]);
            }

        }

        if (gameObject.transform.position.x <= distance && isMoving_ == true)
        {
            velocity_.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            refAnim_.SetBool("Moving", false);
        }

        curLifeTime_ += Time.deltaTime;
        wait_ += Time.deltaTime;

        //when the jocks time runs out offscreen, this stops them and then recyclebullies() moves them to where needed when called
        if (curLifeTime_ >= lifeSpan_)
        {
            for (int i = 0; i < m_JockHorde.Count; ++i)
            {
                m_JockHorde[i].GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
            }
            curLifeTime_ = 0.0f;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            velocity_.velocity = new Vector2(0.0f, 0.0f);
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

        if (wait_ >= 1.0 && isMoving_ == false)
        {
            refAnim_.SetBool("Moving", true);
            distance = (float)Random.Range(3.0f, -4.0f);

            if (distance == lastDistance_[lastDistance_.Count - 1])
            {
                distance = (float)Random.Range(3.0f, -4.0f);
            }
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, targetPoints_[row].transform.position.y, targetPoints_[row].transform.position.z);

            Debug.Log(distance);

            if (lastDistance_[lastDistance_.Count - 1] != null)
            {
                if (distance >= lastDistance_[lastDistance_.Count - 1])
                {
                    velocity_.velocity = new Vector3(1.5f, 0.0f, 0.0f); //moves right

                    Vector3 theScale = transform.localScale;
                    theScale.x *= -1;
                    transform.localScale = theScale;
                }
                else if (distance <= lastDistance_[lastDistance_.Count - 1])
                {
                    velocity_.velocity = new Vector3(-1.5f, 0.0f, 0.0f); //moves left
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
            refAnim_.SetBool("Moving", false);

        }

    }

    //spawns jocks offscreen ready to be used by the ref when RecycleBullies() is called
    void SetJocks()
    {
        int row = 0;
        Vector3 StartPos = new Vector3(11.0f, 0.0f, 0.0f);
        for (int i = 0; i < EnemySpawner_.mEnemiesToSpawn.Length; ++i)
        {
            string bullyName = EnemySpawner_.mEnemiesToSpawn[i].name;
            if (bullyName == "JockBully")
            {
                for (int j = 0; j < maxJockCount_; ++j)
                {
                    newEnemy = Objectpooler.Instance.GetObjectForType(bullyName, true);//new enemy is created
                    newEnemy.transform.position = StartPos;
                    m_JockHorde.Add(newEnemy);
                    newEnemy.GetComponent<BullyScript>().InitEnemy(StartPos, new Vector3(0, 0, 0), newEnemy);
                }

            }
        }
    }

    //reuses the bullies by simply moving them back to a new position since their off the screen by the new spawn time, no point in destroying/making new ones
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


        for (int j = 0; j < m_JockHorde.Count; ++j)
        {
            m_JockHorde[j].transform.position = gameObject.transform.position;
            // m_JockHorde[j].transform.position = m_RefStartPos[StartPos].transform.position;
            m_JockHorde[j].GetComponent<BullyScript>().InitEnemy(gameObject.transform.position, new Vector3(0, 0, 0), newEnemy);

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

    private void SelectDirection(int StartPos, int row)
    {
        if (StartPos == 0 || StartPos == 6 || StartPos == 5)
        {
            horizontalSpeedMin = Constants.HORDE_CHARGE_RIGHT_SPEED;
            verticalSpeedMin = 0;
            horizontalSpeedMax = Constants.HORDE_CHARGE_RIGHT_SPEED * Constants.HORDE_MAX_SPEED_MOD;
            verticalSpeedMax = 0;

        }

        if (StartPos == 2 || StartPos == 7 || StartPos == 3)
        {
            horizontalSpeedMin = Constants.HORDE_CHARGE_LEFT_SPEED;
            verticalSpeedMin = 0;
            horizontalSpeedMax = Constants.HORDE_CHARGE_LEFT_SPEED * Constants.HORDE_MAX_SPEED_MOD;
            verticalSpeedMax = 0;

        }

        if (StartPos == 1)
        {
            horizontalSpeedMin = 0;
            verticalSpeedMin = Constants.HORDE_CHARGE_DOWN_SPEED;
            horizontalSpeedMax = 0;
            verticalSpeedMax = Constants.HORDE_CHARGE_DOWN_SPEED * Constants.HORDE_MAX_SPEED_MOD;
        }
        if (StartPos == 4)
        {
            horizontalSpeedMin = 0;
            verticalSpeedMin = Constants.HORDE_CHARGE_UP_SPEED;
            horizontalSpeedMax = 0;
            verticalSpeedMax = Constants.HORDE_CHARGE_UP_SPEED * Constants.HORDE_MAX_SPEED_MOD;
        }
    }
}



