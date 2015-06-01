using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NerdBossDrone : MonoBehaviour 
{

    private GameObject Player_;
    private Transform NerdTrans_;

    private float MoveSpeed_;
    private float DropThatShit_;
    
    private string junkName_;
    
    private List<GameObject> Junk_ = new List<GameObject>();

	void Start () 
    {
        Player_ = GameObject.FindGameObjectWithTag("Player");
        MoveSpeed_ = 0.02f;
        NerdTrans_ = gameObject.transform;
        DropThatShit_ = 5.0f;
        
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
        junkName_ = "Beaker";
        GameObject m_Junk;
        for (int i = 0; i < 19; ++i)
        {
            m_Junk = ObjectPool.Instance.GetObjectForType(junkName_, true);
            Junk_.Add(m_Junk);
            m_Junk.transform.position = new Vector2(-100.0f, 0.0f);
        }
    }

    void SpawnJunk()
    {
        int junkSelected = (int)Random.Range(0, 15);
        Junk_[junkSelected].transform.position = new Vector3(NerdTrans_.position.x, NerdTrans_.position.y - 1.0f, NerdTrans_.position.z);
    }
   
}
