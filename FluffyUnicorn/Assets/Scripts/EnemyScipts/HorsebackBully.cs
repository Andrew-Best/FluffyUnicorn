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

    private int MaxSpawn_ = 1;

    private float Respawn_ = 10.0f;
    private float Reset_ = 5.0f;

    private Rigidbody2D Velocity_;

    private Vector2 Pos_;

    private bool FirstSpawn_ = true;
	
	void Start () 
    {
        Player_ = GameObject.FindGameObjectWithTag("Player");
        EnemySpawner_ = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<SpawnEnemies>();
        Velocity_ = gameObject.GetComponent<Rigidbody2D>();
        Bully_ = gameObject.GetComponent<BullyScript>();
	}
	
	void Update () 
    {
        Respawn_ -= Time.deltaTime;
        Reset_ -= Time.deltaTime;

        if (FirstSpawn_ == true)
        {
            SetJockys();
            Debug.Log("Set!");
            FirstSpawn_ = false;
        }
        FirstSpawn_ = false;

        if(Respawn_ <= 0.0f)
        {
            RecycleJockys();
            Respawn_ = 10.0f;
        }
        if(Reset_ <= 0.0f)
        {
            for (int i = 0; i < Horses_.Count; ++i)
            {
                Velocity_.velocity = new Vector2(0.0f, 0.0f);
            }
        }

        for(int i = 0; i < Horses_.Count; ++i)
        {
            Velocity_.velocity = new Vector2(3.0f, 0.0f);
        }
	
	}

    void SetJockys()
    {
        int row = 0;
        Vector2 StartPos = new Vector2(11.0f, 0.0f);
        for (int i = 0; i < EnemySpawner_.mEnemiesToSpawn.Length; ++i)
        {
            string bullyName = EnemySpawner_.mEnemiesToSpawn[i].name;
            if (bullyName == "Jocky Bully")
            {
                for (int j = 0; j < MaxSpawn_; ++j)
                {
                    newEnemy = Objectpooler.Instance.GetObjectForType(bullyName, true);//new enemy is created
                    newEnemy.transform.position = StartPos;
                    Horses_.Add(newEnemy);
                    //newEnemy.GetComponent<BullyScript>().InitEnemy(StartPos, row, newEnemy);
                }

            }
        }
    }

    void RecycleJockys()
    {
        int startPos = Random.Range(0, 2);
        int row = 0;
        if(startPos == 0)
        {
            row = 0;
        }
        if(startPos == 1)
        {
            row = 1;
        }
        if (startPos == 2)
        {
            row = 2;
        }

        for (int j = 0; j < Horses_.Count; ++j)
        {
            Horses_[j].transform.position = new Vector2(11.0f, Bully_.m_TargetPoints[row].transform.position.y);
        }
    }
}
