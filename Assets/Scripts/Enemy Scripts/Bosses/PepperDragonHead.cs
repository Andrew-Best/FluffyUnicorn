﻿using UnityEngine;
using System.Collections;

public class PepperDragonHead : PepperDragon 
{
	public float m_ThisHeadCurPosY;
	public float m_ThisHeadDestPosY;
	public float m_ThisHeadStartYPos;
    public float m_headVelocity;

	public GameObject m_ArmToMove;

	private bool headIsMoving_;

	public GameObject MoveToArmPos(GameObject hitArm, int armLayerIndex, int headLayerIndex)
	{
		this.gameObject.layer = armLayerIndex;
		m_ArmToMove = hitArm;
        headIsMoving_ = true;
		return m_ArmToMove;
	}

	// Use this for initialization
	void Start () 
	{
        headIsMoving_ = false;
		m_Player = GameObject.FindGameObjectWithTag("Player");
        m_headVelocity = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{

        m_headVelocity++;
		m_ThisHeadCurPosY = this.GetComponent<Rigidbody2D>().transform.position.y;
        if (headIsMoving_)
		{
			if (m_ThisHeadStartYPos > m_ArmToMove.GetComponent<PepperDragonArm>().m_ThisArmStartPosY)//the head is higher and must be lowered
			{
				if (m_ThisHeadCurPosY > m_ArmToMove.GetComponent<PepperDragonArm>().m_ThisArmStartPosY)
				{
                    this.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, -1.0f, 0.0f) * m_headVelocity;
				}
				else
				{
					this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    headIsMoving_ = false;
				}
			}
			else//the arm is lower and must be raised
			{
				if (m_ThisHeadCurPosY < m_ArmToMove.GetComponent<PepperDragonArm>().m_ThisArmStartPosY)
				{
                    this.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 1.0f, 0.0f);
				}
				else
				{
					this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    headIsMoving_ = false;
				}
			}
			ChangeZPos();
		}
		
	}
}
