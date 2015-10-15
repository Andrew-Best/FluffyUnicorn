using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NerdBossDrone : MonoBehaviour 
{

    private GameObject Player_;
    private GameObject Wall1_;
    private GameObject Wall2_;
    private Transform NerdTrans_;

    private float MoveSpeed_;
    private float DropThatShit_;
    private float HP_;
    
    private string junkName_;
    
    private List<GameObject> Junk_ = new List<GameObject>();

	void Start () 
    {
        Player_ = GameObject.FindGameObjectWithTag("Player");
        MoveSpeed_ = 0.02f;
        NerdTrans_ = gameObject.transform;
        DropThatShit_ = 5.0f;
        HP_ = Constants.DRONE_STAGE_ONE_HP;
        Wall1_ = GameObject.FindGameObjectWithTag("Wall1");
        Wall2_ = GameObject.FindGameObjectWithTag("Wall2");
        
        AddJunk();
       
	}

    void Update()
    {
        DropThatShit_ -= Time.deltaTime;
        
        if(DropThatShit_ <= 0.0f)
        {
            SpawnJunk();
            DropThatShit_ = 5.0f;
        }

        if(HP_ <= 0.0f)
        {
            //HP_ = Constants.DRONE_STAGE_ONE_HP;
            Destroy(Wall1_);
            Destroy(Wall2_);
            gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        }

        
    }
	
	void LateUpdate () 
    {
        Trackplayer();
	}

    void Trackplayer()
    {
        NerdTrans_.position = NerdTrans_.position + (new Vector3(Player_.transform.position.x, NerdTrans_.position.y, Player_.transform.position.z) - NerdTrans_.position) * MoveSpeed_;
    }

    void AddJunk()
    {     
        GameObject m_Junk;
        for (int i = 0; i < 7; ++i)
        {
            junkName_ = "Beaker";
            m_Junk = ObjectPool.Instance.GetObjectForType(junkName_, true);
            Junk_.Add(m_Junk);
            m_Junk.transform.position = new Vector2(-100.0f, 0.0f);
        }
        for (int i = 0; i < 5; ++i)
        {
            junkName_ = "Box";
            m_Junk = ObjectPool.Instance.GetObjectForType(junkName_, true);
            Junk_.Add(m_Junk);
            m_Junk.transform.position = new Vector2(-100.0f, 0.0f);
        }
        for (int i = 0; i < 4; ++i)
        {
            junkName_ = "Pop";
            m_Junk = ObjectPool.Instance.GetObjectForType(junkName_, true);
            Junk_.Add(m_Junk);
            m_Junk.transform.position = new Vector2(-100.0f, 0.0f);
        }
    }

    void SpawnJunk()
    {
        int junkSelected = (int)Random.Range(0, 16);
        Junk_[junkSelected].transform.position = new Vector3(NerdTrans_.position.x, NerdTrans_.position.y - 4.0f, NerdTrans_.position.z);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Junk")
        {
            HP_ -= 10.0f;
            Debug.Log("Hit!");
            collision.gameObject.transform.position = new Vector2(100.0f, 0.0f);
        }
    }

    void OnParticleCollision2D(Collision2D collision)
    {
        //if (collision.collider.tag == "Pop")
        //{
            Debug.Log("POP!");
        //}
    }


   
}
