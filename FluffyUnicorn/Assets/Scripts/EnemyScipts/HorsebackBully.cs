using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HorsebackBully : MonoBehaviour
{
    private GameObject Player_;
    private SpawnEnemies EnemySpawner_;
    private GameObject newEnemy;
    private BullyScript Bully_;

    private List<GameObject> Horses_ = new List<GameObject>();

    private int MaxSpawn_;

    private float Respawn_;
    private float Reset_;
    private float Vel_;

    private Rigidbody2D Velocity_;

    private Vector2 XOffset_;
    private Vector2 StartPos_;

    private bool FirstSpawn_;
    private bool Turn_;
	
	void Start () 
    {
        Player_ = GameObject.FindGameObjectWithTag("Player");
        EnemySpawner_ = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<SpawnEnemies>();
        Velocity_ = gameObject.GetComponent<Rigidbody2D>();
        Bully_ = gameObject.GetComponent<BullyScript>();

        Respawn_ = 10.0f;
        Reset_ = 10.0f;
        MaxSpawn_ = 1;
        FirstSpawn_ = true;
        Turn_ = false;
        //XOffset_ = new Vector2(11.0f, 0.0f);
        StartPos_ = new Vector2(0.0f, 0.0f);

	}
	
	void Update () 
    {
        Respawn_ -= Time.deltaTime;
        //Debug.Log(Respawn_);
        Reset_ -= Time.deltaTime;
        if(FirstSpawn_ == true)
        {
            SetMoreJockys();
            FirstSpawn_ = false;
        }
        if(Respawn_ <= 0.0f)
        {
            Turn_ = true;
            RecycleJockys();
            Respawn_ = 10.0f;
        }
        else
        {
            Turn_ = false;
        }
        if(Reset_ <= 0.0f)
        {
            for (int i = 0; i < Horses_.Count; ++i)
            {
                Velocity_.velocity = new Vector2(0.0f, 0.0f);
            }
        }

        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);

        for(int i = 0; i < Horses_.Count; ++i)
        {
            Vel_ = Random.Range(0.0f, -5.0f);
            Horses_[i].GetComponent<Rigidbody2D>().velocity = new Vector2(Vel_, 0.0f);
        }

        
	
	}

    void SetMoreJockys()
    {
        gameObject.SetActive(true);
        int row = 0;
        Vector2 StartPos = new Vector2(9.0f, 0.0f);
        for (int i = 0; i < EnemySpawner_.mEnemiesToSpawn.Length; ++i)
        {
            string bullyName = EnemySpawner_.mEnemiesToSpawn[i].name;
            if (bullyName == "Jocky Bully")
            {
                MaxSpawn_ = Random.Range(1, 2);
                for (int j = 0; j < MaxSpawn_; ++j)
                {
                    Objectpooler.Instance.GetObjectForType(bullyName, true);//new enemy is created
                    gameObject.transform.position = StartPos;
                    Horses_.Add(gameObject);
                    gameObject.GetComponent<BullyScript>().InitEnemy(StartPos, row, gameObject);
                    
                }

            }
        }
    }

    void RecycleJockys()
    {
        int row = 0;
        int startPos = Random.Range(0, 2);
        if(startPos == 0)
        {
            StartPos_.y = Bully_.m_TargetPoints[0].transform.position.y;
        }
        if(startPos == 1)
        {
            StartPos_.y = Bully_.m_TargetPoints[1].transform.position.y;
        }
        if (startPos == 2)
        {
            StartPos_.y = Bully_.m_TargetPoints[2].transform.position.y;
        }

        float dist = Random.Range(11.0f, 15.0f);
        XOffset_ = new Vector2(dist, 0.0f);

        for (int j = 0; j < Horses_.Count; ++j)
        {
            Horses_[j].transform.position = StartPos_ + XOffset_;
            Horses_[j].GetComponent<BullyScript>().InitEnemy(StartPos_ + XOffset_, row, gameObject);
            
        }
    }
}
