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

	public void Start()
	{
		InitEnemy(new Vector2(0, 0), 2);
	}

	public override void InitEnemy(Vector2 spawnPos, int row)
	{
		base.InitEnemy(spawnPos, row);
		m_Player = GameObject.FindGameObjectWithTag("Player");
		m_ThisBoss = this.gameObject;
		m_Position = m_ThisBoss.GetComponent<Rigidbody2D>().position;

		m_HP = Constants.FATTEST_BULLY_HP;
		jumpForce_ = Constants.FATTEST_BULLY_JUMP_FORCE;
		rollSpeed_ = Constants.FATTEST_BULLY_ROLL_SPEED;

		m_Curstate = 0;

		m_BossName = "Fattest Bully";

		m_CurFrame = 0;

		tempTimer = tempTimerResetVal;
//		m_TotalFrames = this.GetComponent<Animator>().framesInAnim;
	}

	
	// Update is called once per frame
	public override void EnemyUpdate(GameObject ThisBoss) 
	{
		Vector2 playerPos = new Vector2(m_Player.GetComponent<Rigidbody2D>().transform.position.x, m_Player.GetComponent<Rigidbody2D>().transform.position.y);

		tempTimer -= Time.deltaTime;
		if(tempTimer <= 0)
		{
			SLAM(playerPos, this.m_Position);
		}
	}

	public void SLAM(Vector2 playerPos, Vector2 thisBossPos)
	{
		this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, jumpForce_);

	}

}
