﻿using UnityEngine;
using System.Collections;

public class PepperDragonArm : PepperDragon
{
	public GameObject m_HandBarrier;

	public float m_ThisArmCurPosY;
	public float m_ThisArmDestPosY;//AKA the start Y of the head
	public float m_ThisArmStartPosY;

	private float timeUntilHandRises_; //time till hand rises
	private float raiseHandTimer_;
	private bool HandRaising_ = false;

	private bool PosBeingFixed_ = false;
	private bool SwipingAtPlayer = false;
	private Vector2 startPosOfSlam;

	bool ArmIsMoving_;
	bool MoveToRest_ = false;

	public GameObject m_DragonHead;

	public void FixPosition()
	{
		Vector2 fixedPos = new Vector2(this.transform.position.x, m_Head.transform.position.y + 45 );
		this.gameObject.GetComponent<Rigidbody2D>().transform.position = fixedPos;
		PosBeingFixed_ = false;
		SwipingAtPlayer = true;
	}

	public void MoveToHeadPos(int armLayerIndex, int headLayerIndex)
	{		
		ArmIsMoving_ = true;
		this.gameObject.layer =headLayerIndex;
	}

	public void ReadyTheSlaps()
	{
		this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Constants.RAISE_HAND_VELX, Constants.RAISE_HAND_VELY) ;
		HandRaising_ = true;
	}

	public void Slap()
	{
		startPosOfSlam = this.GetComponent<Rigidbody2D>().transform.position;
		this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Constants.SLAP_VELX*-1.0f, 0.0f);
	}

	public void SmackDown()
	{
		this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Constants.SMACK_DOWN_VELX, Constants.SMACK_DOWN_VELY);
		timeUntilHandRises_ = Random.Range(Constants.MIN_TIME_UNTIL_SLAP, Constants.MAX_TIME_UNTIL_SLAP);
	}

	void OnTriggerEnter2D(Collider2D playerAttack)
	{
		if (playerAttack.tag == "PlayerProjectile")
		{
			GetCurLayer();
		
			if (MatchPlayerRowToLayer(curLayerName))
			{
				--m_HP;
				m_PepperDragon.GetComponent<BossBaseClass>().m_HP -= playerAttack.GetComponent<Projectile>().m_Damage;
				m_PepperDragon.GetComponent<PepperDragon>().SwitchRow(m_Head, this.gameObject);
			}
		}
	}

	public void MoveBackToRestPos()
	{
		MoveToRest_ = true;
		
	}
	// Use this for initialization
	void Start()
	{
		ArmIsMoving_ = false;
		m_Player = GameObject.FindGameObjectWithTag("Player");
		timeUntilHandRises_ = Random.Range(Constants.MIN_TIME_UNTIL_SLAP, Constants.MAX_TIME_UNTIL_SLAP);
		m_HP = Constants.PEPPER_DRAGON_ARM_HP;
	}

	// Update is called once per frame
	void Update()
	{
		if(m_HP <= 0)
		{
			Destroy(this.gameObject);
			m_PepperDragon.GetComponent<PepperDragon>().m_DestroyedArms++;
		}
		if(MoveToRest_)
		{
			this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Constants.SLAP_VELX, 0);
			if(this.gameObject.GetComponent<Rigidbody2D>().transform.position.x >= startPosOfSlam.x)
			{
				MoveToRest_ = false;
				this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
			}
		}
		#region Smack Attack
		////////////////////////
		// Hand Smack //
		////////////////////////
		if (!HandRaising_)//if the hand is not raising (i.e if falling down or not moving)
		{
			timeUntilHandRises_ -= Time.deltaTime;//countdown until the next time the hand is raised
			if (timeUntilHandRises_ <= 0)
			{
				timeUntilHandRises_ = 0;//set timer to 0
				ReadyTheSlaps();//function that raises the hand. Changes the HandRaising bool to true
			}
		}

		if(HandRaising_)//After ReadyTheSlaps is called, If the hand is raising into the air
		{			
			raiseHandTimer_ += Time.deltaTime;//count up until the hand SMACKS down
			if (raiseHandTimer_ >= 6)
			{
				raiseHandTimer_ = 0;//set the timer to 0
				SmackDown();//Slam the hand into the ground
				HandRaising_ = false;//Change the boolean to clarify the hand should now be smacking the ground
			}
		}
		#endregion
		#region Part Pos Swap
		/////////////////////////////////////////////////////////////////////////
		// Arm and Head Pos Swap //
		/////////////////////////////////////////////////////////////////////////
		m_ThisArmCurPosY = this.GetComponent<Rigidbody2D>().transform.position.y;
		if(ArmIsMoving_)
		{
			if (m_ThisArmStartPosY > m_DragonHead.GetComponent<PepperDragonHead>().m_ThisHeadStartYPos)//the arm is higher and must be lowered
			{
				if (m_ThisArmCurPosY > m_DragonHead.GetComponent<PepperDragonHead>().m_ThisHeadStartYPos)
				{
					this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1.0f);
				}
				else
				{
					this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
					ArmIsMoving_ = false;
				}
			}
			else//the arm is lower and must be raised
			{
				if (m_ThisArmCurPosY < m_DragonHead.GetComponent<PepperDragonHead>().m_ThisHeadStartYPos)
				{
					this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 1.0f);
				}
				else
				{
					this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
					ArmIsMoving_ = false;
				}
			}
			ChangeZPos();
		}
		GetCurLayer();
		#endregion

		#region Position Fixer
		if (!PosBeingFixed_)
		{
			if (this.GetComponent<Rigidbody2D>().transform.position.y < -10)
			{
				FixPosition();
			}
		}
		if(SwipingAtPlayer)
		{
			if(this.gameObject.GetComponent<Rigidbody2D>().velocity == new Vector2(0, 0))
			{
				Slap();
				SwipingAtPlayer = false;
			}
			
		}
		#endregion
	}

	void OnCollisionEnter2D(Collision2D ground)
	{
		if(ground.gameObject.tag == "Track0")
		{
			this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);			
		}
		else if (ground.gameObject.tag == "Track1")
		{
			this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
		}
		else if (ground.gameObject.tag == "Track2")
		{
			this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
		}

		if(ground.gameObject.tag == "HandBarrier")
		{
			//Vector2 transformPos = this.gameObject.GetComponent<Rigidbody2D>().transform.position;
			MoveBackToRestPos();		
		}
	}

}