using UnityEngine;
using System.Collections;

public class FattestBully : BossBaseClass
{
	/****************/
	float tempTimer;
	float tempTimerResetVal = 5.0f;
	/**********/


	private float jumpForce_;
	private float rollSpeed_;
	public float m_JumpTimer;

	public bool inSky;
	public bool isFlying;

	public void Start()
	{
		InitEnemy(new Vector2(0, 0), 2);
	}

	public override void InitEnemy(Vector2 spawnPos, int row)
	{
		base.InitEnemy(spawnPos, row);
	//	m_Player = GameObject.FindGameObjectWithTag("Player");
		m_ThisBoss = this.gameObject;
		m_Position = m_ThisBoss.GetComponent<Rigidbody2D>().position;

		m_HP = Constants.FATTEST_BULLY_HP;
		jumpForce_ = Constants.FATTEST_BULLY_JUMP_FORCE;
		rollSpeed_ = Constants.FATTEST_BULLY_ROLL_SPEED;

		m_Curstate = 0;

		m_BossName = "Fattest Bully";

		m_CurFrame = 0;

		tempTimer = tempTimerResetVal;

		m_JumpTimer = Constants.FATTEST_BULLY_JUMP_TIMER;

		inSky = false;
		isFlying = false;
		
//		m_TotalFrames = this.GetComponent<Animator>().framesInAnim;
	}

	
	// Update is called once per frame
	public override void EnemyUpdate(GameObject ThisBoss) 
	{
		GetPlayerInfo(ThisBoss);

		if (ThisBoss.GetComponent<Rigidbody2D>().transform.position.y <= 0)
		{
			isFlying = false;
			this.inSky = false;

			this.GetComponent<Rigidbody2D>().transform.position = new Vector2(this.GetComponent<Rigidbody2D>().transform.position.x, 0);
		}

		Vector2 playerPos = new Vector2(m_Player.GetComponent<Rigidbody2D>().transform.position.x, m_Player.GetComponent<Rigidbody2D>().transform.position.y);
		if(!inSky)
		{
			tempTimer -= Time.deltaTime;//
		}
		
		if(tempTimer <= 0)
		{
			Fly(playerPos, ThisBoss.GetComponent<Rigidbody2D>().transform.position);
			inSky = true;
		}
	}

	public void Fly(Vector2 playerPos, Vector2 thisBossPos)
	{		
		//start the helicopter anim

		//after 2 seconds into the animation, FLY
		if(!isFlying)
		{
			m_JumpTimer -= Time.deltaTime; // temp var
		}
		
		if (m_JumpTimer <= 0)
		{
			m_JumpTimer = Constants.FATTEST_BULLY_JUMP_TIMER;
			this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, jumpForce_);
			isFlying = true;
		}

		//once off screen/high enough, adjust x pos and CurRow to match player
		if (this.GetComponent<Rigidbody2D>().transform.position.y >= Constants.MAX_FATTEST_HEIGHT)
		{
			this.GetComponent<Rigidbody2D>().transform.position = new Vector2(playerPos.x, this.GetComponent<Rigidbody2D>().transform.position.y);
			this.m_CurRow = this.m_PlayerCurRow;
			Slam(playerPos);		
		}
		
	}

	//SLAM, fly down until boss's y == the curRow's Y
	public void Slam(Vector2 playerPos)
	{
		float SLAMFORCE = jumpForce_ * -1;
	
		m_Bully.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, SLAMFORCE);		
	}

}
