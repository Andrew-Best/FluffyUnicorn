using UnityEngine;
using System.Collections;

public class NerdBoss : MonoBehaviour
{
    private GameObject Player_;
    private Transform NerdTran_;

    private float Speed_;
    private float moveAwayDist_;

    private Rigidbody2D NerdRig_;
	
	void Start () 
    {
        Player_ = GameObject.FindGameObjectWithTag("Player");
        NerdTran_ = gameObject.transform;
        moveAwayDist_ = 3.0f;
        Speed_ = 2.0f;
        NerdRig_ = gameObject.GetComponent<Rigidbody2D>();
        
	}
	
	void Update () 
    {
        
  	}

    
}
