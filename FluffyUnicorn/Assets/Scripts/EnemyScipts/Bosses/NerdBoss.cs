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
        StayAway();
  	}

    void StayAway()
    {
        NerdTran_.position = new Vector3(Player_.transform.position.x + moveAwayDist_, NerdTran_.position.y, Player_.transform.position.z);
        /*Vector3 toPlayer = Player_.transform.position - NerdTran_.position;
        toPlayer = toPlayer.normalized;

        if(toPlayer.x >= 1.0f)
        {
            NerdTran_.position = new Vector3(NerdTran_.position.x + moveAwayDist_, NerdTran_.position.y, Player_.transform.position.z);
        }*/
        
    }
}
