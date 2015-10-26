using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NerdBossDrone : MonoBehaviour 
{

    private GameObject player_;
    private GameObject wall1_;
    private GameObject wall2_;
    private Transform nerdTrans_;

    private float moveSpeed_;
    private float dropThatShit_;
    private float HP_;
    
    private string junkName_;
    
    private List<GameObject> Junk_ = new List<GameObject>();

	void Start () 
    {
        player_ = GameObject.FindGameObjectWithTag("Player");
        moveSpeed_ = 0.02f;
        nerdTrans_ = gameObject.transform;
        dropThatShit_ = 5.0f;
        HP_ = Constants.DRONE_STAGE_ONE_HP;
        wall1_ = GameObject.FindGameObjectWithTag("Wall1");
        wall2_ = GameObject.FindGameObjectWithTag("Wall2");
        
        AddJunk();
       
	}

    void Update()
    {
        dropThatShit_ -= Time.deltaTime;
        
        if(dropThatShit_ <= 0.0f)
        {
            SpawnJunk();
            dropThatShit_ = 5.0f;
        }

        if(HP_ <= 0.0f)
        {
            //HP_ = Constants.DRONE_STAGE_ONE_HP;
            Destroy(wall1_);
            Destroy(wall2_);
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

        
    }
	
	void LateUpdate () 
    {
        Trackplayer();
	}

    void Trackplayer()
    {
        nerdTrans_.position = nerdTrans_.position + (new Vector3(player_.transform.position.x, nerdTrans_.position.y, player_.transform.position.z) - nerdTrans_.position) * moveSpeed_;
    }

    void AddJunk()
    {     
        GameObject m_Junk;
        for (int i = 0; i < 7; ++i)
        {
            junkName_ = "Beaker";
            m_Junk = ObjectPool.Instance.GetObjectForType(junkName_, true);
            Junk_.Add(m_Junk);
            m_Junk.transform.position = new Vector3(-100.0f, 0.0f, 0.0f);
        }
        for (int i = 0; i < 5; ++i)
        {
            junkName_ = "Box";
            m_Junk = ObjectPool.Instance.GetObjectForType(junkName_, true);
            Junk_.Add(m_Junk);
            m_Junk.transform.position = new Vector3(-100.0f, 0.0f, 0.0f);
        }
        for (int i = 0; i < 4; ++i)
        {
            junkName_ = "Pop";
            m_Junk = ObjectPool.Instance.GetObjectForType(junkName_, true);
            Junk_.Add(m_Junk);
            m_Junk.transform.position = new Vector3(-100.0f, 0.0f, 0.0f);
        }
    }

    void SpawnJunk()
    {
        int junkSelected = (int)Random.Range(0, 16);
        Junk_[junkSelected].transform.position = new Vector3(nerdTrans_.position.x, nerdTrans_.position.y - 4.0f, nerdTrans_.position.z);
    }

    void OnTriggerEnter(Collider collision)
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Junk")
        {
            HP_ -= 10.0f;
            Debug.Log("Hit!");
            collision.gameObject.transform.position = new Vector3(100.0f, 0.0f, 0.0f);
        }
    }

    void OnParticleCollision(GameObject collision)
    {
        //if (collision.collider.tag == "Pop")
        //{
            Debug.Log("POP!");
        //}
    }


   
}
