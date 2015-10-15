using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoshBully : MonoBehaviour 
{
    private GameObject player_;
    //private SpawnEnemies enemySpawner_;
    private EnemyBaseClass bully_;
    private Animator poshAnim_;
    public GameObject fire_;

    //private List<GameObject> bitches_ = new List<GameObject>();

    //private int maxSpawn_;

    //private float respawn_;
    //private float vel_;
    private float tooClose_;
    private float spinSpeed_;
    //private float maxDist_;
    private float distTimer_;

    private bool die_;
    //private bool start_;

    private Rigidbody2D velocity_;
    private Transform poshTrans_;

    private Vector3 xOffset_;
    private Vector3 startPos_;
    private Vector3 spawnPos_;

    private List<GameObject> targetPoints_ = new List<GameObject>();

	void Start () 
    {
        player_ = GameObject.FindGameObjectWithTag("Player");
        //enemySpawner_ = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<SpawnEnemies>();
        //bully_ = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyBaseClass>();
        velocity_ = gameObject.GetComponent<Rigidbody2D>();
        poshAnim_ = gameObject.GetComponent<Animator>();
        poshTrans_ = gameObject.transform;
        //fire_ = GameObject.Find("FireMobile");

        //respawn_ = 25.0f;
        //maxSpawn_ = 1;
        //vel_ = 0.0f;
        tooClose_ = 3.0f;
        spinSpeed_ = 15.0f;
        distTimer_ = 5.0f;
        //maxDist_ = 1.0f;

        die_ = false;
        //start_ = false;

        for (int i = 0; i < 3; ++i)
        {
            targetPoints_.Add(GameObject.FindGameObjectWithTag("Targetpoint" + i));
        }        

        xOffset_ = new Vector3(0.0f, 0.0f, 0.0f);
        startPos_ = new Vector3(0.0f, 0.0f, 0.0f);
        spawnPos_ = new Vector3(12.0f, targetPoints_[0].transform.position.y, targetPoints_[1].transform.position.z);
        poshTrans_.position = spawnPos_;
       
	}
	
	
	void Update () 
    {
        PissOff();

        if(poshTrans_.position.x <= -9.0f)
        {
            RecyclePosh();
        }

        if(die_ == true)
        {
            distTimer_ -= Time.deltaTime;
            Attack();
            velocity_.velocity = new Vector2(0.0f, 0.0f);
            if (distTimer_ <= 0.0f && die_ == true)
            {
                die_ = false;
                StopAttack();
                distTimer_ = 5.0f;
            } 
        }
        else
        {
            velocity_.velocity = new Vector2(-1.0f, 0.0f);
        }

	}

    void RecyclePosh()
    {
        int startPos = Random.Range(0, 3);
        if (startPos == 0)
        {
            startPos_.y = targetPoints_[0].transform.position.y;
            startPos_.z = targetPoints_[0].transform.position.z;
            
        }
        if (startPos == 1)
        {
            startPos_.y = targetPoints_[1].transform.position.y;
            startPos_.z = targetPoints_[1].transform.position.z;
           
        }
        if (startPos == 2)
        {
            startPos_.y = targetPoints_[2].transform.position.y;
            startPos_.z = targetPoints_[2].transform.position.z;
           
        }

        xOffset_ = new Vector3(12.0f, 0.0f, 0.0f);
        
        poshTrans_.position = startPos_ + xOffset_;
        
    }

    void PissOff()
    {
        float toPlayer = poshTrans_.position.x - player_.transform.position.x;
        if (toPlayer <= tooClose_)
        {
            poshAnim_.SetBool("StepOff", true);   
        }
        else
        {
            poshAnim_.SetBool("StepOff", false);  
        }
        if(toPlayer <= -0.1f)
        {
            poshAnim_.SetBool("StepOff", false);  
        }
    }

    void Attack()
    {
        poshAnim_.SetBool("Attack", true);
        fire_.SetActive(true);
        poshTrans_.Rotate(0.0f, spinSpeed_, 0.0f);   
    }

    void StopAttack()
    {
        
        poshAnim_.SetBool("Attack", false);
        fire_.SetActive(false);
        poshTrans_.Rotate(0.0f, 0.0f, 0.0f);
        poshTrans_.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        velocity_.velocity = new Vector2(-1.0f, 0.0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            die_ = true;
            //start_ = true;
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
