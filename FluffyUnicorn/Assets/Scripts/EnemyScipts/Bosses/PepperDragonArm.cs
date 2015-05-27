using UnityEngine;
using System.Collections;

public class PepperDragonArm : PepperDragon
{
	public float m_ThisArmCurPosY;
	public float m_ThisArmDestPosY;//AKA the start Y of the head
	public float m_ThisArmStartPosY;

	private float timeUntilHandRises_; //time till hand rises
	private float raiseHandTimer_;
	private bool HandRaising_ = false;

	private bool TooLow_ = false;
	private bool PosBeingFixed_ = false;

	string OriginLayer;
	bool ArmIsMoving_;

	public GameObject m_DragonHead;

	public void FixPosition()
	{
		GetCurLayer();
		OriginLayer = curLayerName;
		this.gameObject.layer = 0;
		this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Constants.RAISE_HAND_VELX, Constants.RAISE_HAND_VELY);
		PosBeingFixed_ = true;		
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
				m_PepperDragon.GetComponent<BossBaseClass>().m_HP -= playerAttack.GetComponent<Projectile>().m_Damage;
				m_PepperDragon.GetComponent<PepperDragon>().SwitchRow(m_Head, this.gameObject);
			}
		}
	}

	// Use this for initialization
	void Start()
	{
		ArmIsMoving_ = false;
		m_Player = GameObject.FindGameObjectWithTag("Player");
		timeUntilHandRises_ = Random.Range(Constants.MIN_TIME_UNTIL_SLAP, Constants.MAX_TIME_UNTIL_SLAP);
	}

	// Update is called once per frame
	void Update()
	{
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
				TooLow_ = true;
				FixPosition();
			}
		}
		if (PosBeingFixed_)
		{
			if (this.GetComponent<Rigidbody2D>().transform.position.y > 30)
			{
				this.gameObject.layer = int.Parse(OriginLayer);
				this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
				PosBeingFixed_ = false;
				TooLow_ = false;
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
	}

}
