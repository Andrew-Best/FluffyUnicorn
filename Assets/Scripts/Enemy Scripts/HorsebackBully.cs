using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HorsebackBully : MonoBehaviour
{
    private GameObject player_;
    private PlayerController pController_;
    private SpawnEnemies enemySpawner_;
    private GameObject newEnemy;
    private BullyScript bully_;

    private List<GameObject> horses_ = new List<GameObject>();

    private int maxSpawn_;
    //private int currRow_;

    private float respawn_;
    private float reset_;
    private float vel_;

    private Rigidbody2D velocity_;

    private Vector3 xOffset_;
    private Vector3 startPos_;

    private bool firstSpawn_;
    //private bool turn_;
    //private bool frontOccupied_;
    //private bool middleOccupied_;
    //private bool backOccupied_;
	
	void Start () 
    {
        //player_ = GameObject.FindGameObjectWithTag("Player");
       // pController_ = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemySpawner_ = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<SpawnEnemies>();
        velocity_ = gameObject.GetComponent<Rigidbody2D>();
        bully_ = gameObject.GetComponent<BullyScript>();
        pController_ = gameObject.GetComponent<PlayerController>();
        respawn_ = 10.0f;
        reset_ = 10.0f;
        maxSpawn_ = 1;
        firstSpawn_ = true;
        //turn_ = false;
        xOffset_ = new Vector3(0.0f, 0.0f, 0.0f);
        startPos_ = new Vector3(0.0f, 0.0f, 0.0f);
   
	}
	
	void Update () 
    {
        respawn_ -= Time.deltaTime;
        //Debug.Log(Respawn_);
        reset_ -= Time.deltaTime;
        if(firstSpawn_ == true)
        {
            SetMoreJockys();
            firstSpawn_ = false;
        }
        if(respawn_ <= 0.0f)
        {
            //Turn_ = true;
            RecycleJockys();
            respawn_ = 10.0f;
        }
        else
        {
            //Turn_ = false;
        }
        if(reset_ <= 0.0f)
        {
            for (int i = 0; i < horses_.Count; ++i)
            {
                velocity_.velocity = new Vector2(0.0f, 0.0f);
            }
        }

        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);

        for(int i = 0; i < horses_.Count; ++i)
        {
            vel_ = Random.Range(-2.0f, -6.0f);
            horses_[i].GetComponent<Rigidbody2D>().velocity = new Vector2(vel_, 0.0f);
        }
       
        
	
	}

    void SetMoreJockys()
    {
        gameObject.SetActive(true);
        int row = 0;
        Vector2 StartPos = new Vector2(9.0f, 0.0f);
        for (int i = 0; i < enemySpawner_.mEnemiesToSpawn.Length; ++i)
        {
            string bullyName = enemySpawner_.mEnemiesToSpawn[i].name;
            if (bullyName == "Jocky Bully")
            {
                maxSpawn_ = Random.Range(1, 2);
                for (int j = 0; j < maxSpawn_; ++j)
                {
                    Objectpooler.Instance.GetObjectForType(bullyName, true);//new enemy is created
                    gameObject.transform.position = StartPos;
                    horses_.Add(gameObject);
                    gameObject.GetComponent<BullyScript>().InitEnemy(StartPos, new Vector3(0, 0, 0), gameObject);
                    
                }

            }
        }
    }

    void RecycleJockys()
    {
        int row = 0;
        int startPos = Random.Range(0, 3);
        if(startPos == 0)
        {
            startPos_.y = bully_.m_TargetPoints[0].transform.position.y;
            startPos_.z = bully_.m_TargetPoints[0].transform.position.z;
            //currRow_ = 0;
            Debug.Log(0);
        }
        if(startPos == 1)
        {
            startPos_.y = bully_.m_TargetPoints[1].transform.position.y;
            startPos_.z = bully_.m_TargetPoints[1].transform.position.z;
            //currRow_ = 1;
            Debug.Log(1);
            //MiddleOccupied_ = true;
        }
        if (startPos == 2)
        {
            startPos_.y = bully_.m_TargetPoints[2].transform.position.y;
            startPos_.z = bully_.m_TargetPoints[2].transform.position.z;
            //currRow_ = 2;
            Debug.Log(2);
        }

        float dist = Random.Range(10.0f, 15.0f);
        xOffset_ = new Vector3(dist, 0.0f, 0.0f);

        for (int j = 0; j < horses_.Count; ++j)
        {
            horses_[j].transform.position = startPos_ + xOffset_;
            horses_[j].GetComponent<BullyScript>().InitEnemy(startPos_ + xOffset_, new Vector3(0, 0, 0), gameObject);
            
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //TRACK BASED CODE
        if (collision.tag == "Player" )
        {
            player_.transform.position = new Vector3(player_.transform.position.x, bully_.m_TargetPoints[2].transform.position.y, bully_.m_TargetPoints[2].transform.position.z);
      
            //Physics2D.IgnoreLayerCollision(0, 18, true);
        }

       
           /* {
                Player_.transform.position = new Vector2(Player_.transform.position.x, Bully_.m_TargetPoints[0].transform.position.y);
                PC_.m_onFrontTrack = true;
                Physics2D.IgnoreLayerCollision(0, 18, true);
            }

            if(PC_.m_onFrontTrack == true)
            {
                Player_.transform.position = new Vector2(Player_.transform.position.x, Bully_.m_TargetPoints[1].transform.position.y);
                PC_.m_onMiddleTrack = true;
                Physics2D.IgnoreLayerCollision(0, 18, true);
            }

            if(PC_.m_onLastTrack == true)
            {
                Player_.transform.position = new Vector2(Player_.transform.position.x, Bully_.m_TargetPoints[1].transform.position.y);
                PC_.m_onMiddleTrack = true;
                Physics2D.IgnoreLayerCollision(0, 18, true);
            }*/
        //}
       
    } 

    /*void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Physics2D.IgnoreLayerCollision(0, 18, false);
        }
    }*/
}
