﻿using UnityEngine;
using System.Collections;

public class PepperDragon : BossBaseClass
{
    public GameObject m_Arm1;
    public GameObject m_Arm2;
    public GameObject m_Head;
    public int m_DestroyedArms;
    public string up;
    public string down;
    public string z;

    public GameObject m_PepperDragon;

    ///protected string layerNameAsIndex;
    //protected int layerIndex;
    protected string curLayerName;

    public string m_PlayerLayer;

    public void SwitchRow(GameObject head, GameObject hitArm)
    {
        head.GetComponent<PepperDragonHead>().m_ThisHeadStartYPos = m_Head.GetComponent<Rigidbody>().transform.position.y;
        hitArm.GetComponent<PepperDragonArm>().m_ThisArmStartPosY = hitArm.GetComponent<Rigidbody>().transform.position.y;

        string headStartLayer = head.gameObject.layer.ToString();
        string armStartLayer = hitArm.gameObject.layer.ToString();

        //int headStartLayerIndex = int.Parse(headStartLayer);
        //int armStartLayerIndex = int.Parse(armStartLayer);
        float verticalMove_ = 5.0f;

        //head.GetComponent<PepperDragonHead>().MoveToArmPos(hitArm, armStartLayerIndex, headStartLayerIndex);
        //hitArm.GetComponent<PepperDragonArm>().MoveToHeadPos(armStartLayerIndex, headStartLayerIndex);
    }

  
    protected void ChangeZPos()
    {
        float curVelX = this.gameObject.GetComponent<Rigidbody>().velocity.x;
        float curVelY = this.gameObject.GetComponent<Rigidbody>().velocity.y;
        float curZPos = this.gameObject.GetComponent<Rigidbody>().transform.position.z;
        Vector3 curVel = this.gameObject.GetComponent<Rigidbody>().velocity;

        if (curLayerName == "up")
        //if( up)
        {
            //If the layer is the BackRow, and the Z pos isn't the assigned backrow z pos
            if (curZPos != Constants.PEPPER_DRAGON_Z_POS_BACKROW)
            {
                //if the z pos is higher than the position it should be then
                if (curZPos > Constants.PEPPER_DRAGON_Z_POS_BACKROW)
                {
                    //assign the velocity to move the object lower until is reaches the designated position
                    curVel = new Vector3(curVelX, curVelY, -Constants.PEPPER_DRAGON_Z_VELOCITY);
                }
                else
                {
                    curVel = new Vector3(curVelX, curVelY, Constants.PEPPER_DRAGON_Z_VELOCITY);
                }
            }
        }
        if (curLayerName == "down")
        //if (curLayerName == "PDMidRow")
        // if(float == down)
        {
            if (curZPos != Constants.PEPPER_DRAGON_Z_POS_MIDROW)
            {
                if (curZPos > Constants.PEPPER_DRAGON_Z_POS_MIDROW)
                {
                    curVel = new Vector3(curVelX, curVelY, -Constants.PEPPER_DRAGON_Z_VELOCITY);
                }
                else
                {
                    curVel = new Vector3(curVelX, curVelY, Constants.PEPPER_DRAGON_Z_VELOCITY);
                }
            }
        }
        //if (curLayerName == "PDFrontRow")
        if (curLayerName == "z")
        {
            if (curZPos != Constants.PEPPER_DRAGON_Z_POS_FRONTROW)
            {
                if (curZPos > Constants.PEPPER_DRAGON_Z_POS_FRONTROW)
                {
                    curVel = new Vector3(curVelX, curVelY, -Constants.PEPPER_DRAGON_Z_VELOCITY);
                }
                else
                {
                    curVel = new Vector3(curVelX, curVelY, Constants.PEPPER_DRAGON_Z_VELOCITY);
                }
            }
        }
        this.gameObject.GetComponent<Rigidbody>().velocity = curVel;
    }

    private void SpewPepper()
    {

    }

    // Use this for initialization
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (m_DestroyedArms == 2)
        {
            //PepperDragon is defeated
            Destroy(this.gameObject);
        }
    }
}


