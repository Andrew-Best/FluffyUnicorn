using UnityEngine;
using System.Collections;

public class FattestBully : BossBaseClass
{
	/****************/
	float tempTimer;
	float tempTimerResetVal = 5.0f;
	/**********/
	public const float MAX_FATTEST_HEIGHT = 100.0f;
	public const float FATTEST_BULLY_JUMP_TIMER = 2;

	private float jumpForce_;
	private float rollSpeed_;
	public float m_JumpTimer;

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

		m_JumpTimer = FATTEST_BULLY_JUMP_TIMER;
//		m_TotalFrames = this.GetComponent<Animator>().framesInAnim;
	}

	
	// Update is called once per frame
	public override void EnemyUpdate(GameObject ThisBoss) 
	{
		Vector2 playerPos = new Vector2(m_Player.GetComponent<Rigidbody2D>().transform.position.x, m_Player.GetComponent<Rigidbody2D>().transform.position.y);

		tempTimer -= Time.deltaTime;//
		if(tempTimer <= 0)
		{
			SLAM(playerPos, this.GetComponent<Rigidbody2D>().transform.position);
		}
	}

	public void SLAM(Vector2 playerPos, Vector2 thisBossPos)
	{
		float SLAMFORCE = jumpForce_ * -1;
		//start the helicopter anim

		//after 2 seconds into the animation, FLY
		m_JumpTimer -= Time.deltaTime; // temp var
		if (m_JumpTimer <= 0)
		{
			m_JumpTimer = 0;
			this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, jumpForce_);
		}
		//once off screen/high enough, adjust x pos and CurRow to match player
		if(this.GetComponent<Rigidbody2D>().transform.position.y >= MAX_FATTEST_HEIGHT)
		{
			this.GetComponent<Rigidbody2D>().transform.position = new Vector2(playerPos.x, this.GetComponent<Rigidbody2D>().transform.position.y);
			this.m_CurRow = 0; //this.m_Bully.GetComponent<BullyScript>().m_PlayerCurRow;

			if(m_Bully.transform.position.y > this.m_Player.transform.position.y)
			{
				m_Bully.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, SLAMFORCE);
			}
			
		}
		//SLAM, fly down until boss's y == the curRow's Y
	}

}
