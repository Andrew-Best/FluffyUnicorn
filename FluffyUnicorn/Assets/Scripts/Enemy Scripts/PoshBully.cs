using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoshBully : MonoBehaviour 
{
    private GameObject Player_;
    //private SpawnEnemies EnemySpawner_;
    private EnemyBaseClass Bully_;
    private Animator PoshAnim_;
    public GameObject Fire_;

    //private List<GameObject> Bitches_ = new List<GameObject>();

    //private int MaxSpawn_;

    //private float Respawn_;
    //private float Vel_;
    private float TooClose_;
    private float SpinSpeed_;
    //private float MaxDist_;
    private float DistTimer_;

    private bool Die_;
    //private bool Start_;

    private Rigidbody2D Velocity_;
    private Transform PoshTrans_;

    private Vector3 XOffset_;
    private Vector3 StartPos_;
    private Vector3 SpawnPos_;

    private List<GameObject> targetPoints_ = new List<GameObject>();

	void Start () 
    {
        Player_ = GameObject.FindGameObjectWithTag("Player");
        //EnemySpawner_ = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<SpawnEnemies>();
        //Bully_ = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyBaseClass>();
        Velocity_ = gameObject.GetComponent<Rigidbody2D>();
        PoshAnim_ = gameObject.GetComponent<Animator>();
        PoshTrans_ = gameObject.transform;
        //Fire_ = GameObject.Find("FireMobile");

        //Respawn_ = 25.0f;
        //MaxSpawn_ = 1;
        //Vel_ = 0.0f;
        TooClose_ = 3.0f;
        SpinSpeed_ = 15.0f;
        DistTimer_ = 5.0f;
        //MaxDist_ = 1.0f;

        Die_ = false;
        //Start_ = false;

        for (int i = 0; i < 3; ++i)
        {
            targetPoints_.Add(GameObject.FindGameObjectWithTag("Targetpoint" + i));
        }        

        XOffset_ = new Vector3(0.0f, 0.0f, 0.0f);
        StartPos_ = new Vector3(0.0f, 0.0f, 0.0f);
        SpawnPos_ = new Vector3(12.0f, targetPoints_[0].transform.position.y, targetPoints_[1].transform.position.z);
        PoshTrans_.position = SpawnPos_;
       
	}
	
	
	void Update () 
    {
        PissOff();

        if(PoshTrans_.position.x <= -9.0f)
        {
            RecyclePosh();
        }

        if(Die_ == true)
        {
            DistTimer_ -= Time.deltaTime;
            Attack();
            Velocity_.velocity = new Vector2(0.0f, 0.0f);
            if (DistTimer_ <= 0.0f && Die_ == true)
            {
                Die_ = false;
                StopAttack();
                DistTimer_ = 5.0f;
            } 
        }
        else
        {
            Velocity_.velocity = new Vector2(-1.0f, 0.0f);
        }

	}

    void RecyclePosh()
    {
        int startPos = Random.Range(0, 3);
        if (startPos == 0)
        {
            StartPos_.y = targetPoints_[0].transform.position.y;
            StartPos_.z = targetPoints_[0].transform.position.z;
            
        }
        if (startPos == 1)
        {
            StartPos_.y = targetPoints_[1].transform.position.y;
            StartPos_.z = targetPoints_[1].transform.position.z;
           
        }
        if (startPos == 2)
        {
            StartPos_.y = targetPoints_[2].transform.position.y;
            StartPos_.z = targetPoints_[2].transform.position.z;
           
        }

        XOffset_ = new Vector3(12.0f, 0.0f, 0.0f);
        
        PoshTrans_.position = StartPos_ + XOffset_;
        
    }

    void PissOff()
    {
        float toPlayer = PoshTrans_.position.x - Player_.transform.position.x;
        if (toPlayer <= TooClose_)
        {
            PoshAnim_.SetBool("StepOff", true);   
        }
        else
        {
            PoshAnim_.SetBool("StepOff", false);  
        }
        if(toPlayer <= -0.1f)
        {
            PoshAnim_.SetBool("StepOff", false);  
        }
    }

    void Attack()
    {
        PoshAnim_.SetBool("Attack", true);
        Fire_.SetActive(true);
        PoshTrans_.Rotate(0.0f, SpinSpeed_, 0.0f);   
    }

    void StopAttack()
    {
        
        PoshAnim_.SetBool("Attack", false);
        Fire_.SetActive(false);
        PoshTrans_.Rotate(0.0f, 0.0f, 0.0f);
        PoshTrans_.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        Velocity_.velocity = new Vector2(-1.0f, 0.0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Die_ = true;
            //Start_ = true;
            Physics2D.IgnoreLayerCollision(23, 0, true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(23, 0, false);
        }
    }

    /*void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Physics2D.IgnoreLayerCollision(23, 0, false);
        }
    }*/
   
}
