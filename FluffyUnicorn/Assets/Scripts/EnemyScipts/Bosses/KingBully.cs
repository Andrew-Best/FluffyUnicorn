using UnityEngine;
using System.Collections;

public class KingBully : BossBaseClass
{
	public float m_AttackUniqueAnimLength;
	private float AttackUniqueCurTime = 0;

	private float tempTimer;
	private float tempTimerResetVal = 240.0f;

	public void Start()
	{
		InitEnemy(new Vector2(0, 0), 2);
	}

	public void WaterGun()//Done
	{
		//create BEAM attack
		this.GetComponent<EnemyBaseClass>().EnemyStopMotion(m_ThisBoss);
		this.GetComponent<BeamAttack>().Fire(m_ThisBoss, m_ThisBoss.GetComponent<EnemyBaseClass>().m_Player);

		this.m_AttackUniqueAnimLength = Constants.KB_WATER_GUN_LENGTH;
		this.AttackUniqueCurTime = 0;
		//water gun animation
	}

	public override void InitEnemy(Vector2 spawnPos, int row)
	{
		base.InitEnemy(spawnPos, row);
		this.m_ThisBoss = this.gameObject;
		this.m_Position = m_ThisBoss.GetComponent<Rigidbody2D>().position;
		this.m_HP = Constants.KING_BULLY_HP;
		this.m_Curstate = 0;
		this.m_BossName = "King Bully";
		this.m_CurFrame = 0;

		this.m_TargetPoints[0] = GameObject.FindGameObjectWithTag("TargetLastTrack");

		this.m_TargetPoints[1] = GameObject.FindGameObjectWithTag("TargetMidTrack");

		this.m_TargetPoints[2] = GameObject.FindGameObjectWithTag("TargetFrontTrack");

		this.GetComponent<BeamAttack>().m_Ammo = Constants.KB_WATER_AMMO;
		//m_TotalFrames = this.GetComponent<Animator>().framesInAnim;
	}


	// Update is called once per frame
	public override void EnemyUpdate(GameObject ThisBoss)
	{
		Vector2 playerPos = new Vector2(m_Player.GetComponent<Rigidbody2D>().position.x, m_Player.GetComponent<Rigidbody2D>().position.y);

		//Detect Player Track
		GetPlayerInfo(m_ThisBoss);

		//Conditions for changing tracks
		if (!this.m_TimerIsCounting) //if the primary timer is not able to count down (disabled)
		{
			this.changeTrackCountdown = Constants.TRACK_COUNTDOWN_DEFAULT; //set the primary timer to its default value
			this.secondaryTrackTimer -= Time.deltaTime; // decrement the secondary timer
		}
		if (secondaryTrackTimer <= 0) //once the secondary timer reaches 0
		{
			this.m_TimerIsCounting = true; //enable the primary timer
			this.secondaryTrackTimer = Constants.TRACK_COUNTDOWN_DEFAULT; //set the secondary timer to it's default value
		}
		if (this.m_TimerIsCounting)
		{
			this.changeTrackCountdown -= Time.deltaTime;
		}

		this.GetComponent<Rigidbody2D>().transform.position = new Vector2(this.GetComponent<Rigidbody2D>().transform.position.x, this.GetComponent<EnemyBaseClass>().m_TargetPoints[m_CurRow].transform.position.y);

		Vector2 enemyPos = new Vector2(this.m_RigidBody.position.x, this.m_RigidBody.position.y);

		if (this.m_EnemyInMotion)
		{
			this.EnemyMove(m_ThisBoss);
		}

		float differenceThenNow = this.m_InitialXY.x - enemyPos.x;
		float pointB = m_MaxDist;
		float pointA = this.m_InitialXY.x + 1;

		if (this.m_isIdle)
		{
			//moving right and has passed pointA
			if (enemyPos.x >= pointA && this.m_EnemyGoingLeft == -1)
			{
				enemyPos.x = pointA;
				this.TurnAround(m_ThisBoss);
			}
			//moving left and has passed point B
			if (enemyPos.x <= pointB && this.m_EnemyGoingLeft == 1)
			{
				this.TurnAround(m_ThisBoss);
			}
			DetectPlayer(m_Player.transform.position, enemyPos);
		}
		else // enemy is not idle, therefore player is nearby
		{
			this.ChasePlayer(playerPos, enemyPos, m_ThisBoss);

			//Animation
			if (this.m_AnimationLength > 0) //if animating, subtract Delta.Time
			{
				this.m_AnimationLength -= Time.deltaTime;
			}
			if (this.m_AttackTimer > 0 && this.m_AnimationLength <= 0) //if the enemy isn't cooled down, and is not animating
			{
				this.m_AttackTimer -= Time.deltaTime;
			}
			if (this.m_AttackTimer <= 0 && this.m_BullyWalk.GetBool("IsPunch") == false && this.m_BullyWalk.GetBool("IsKick") == false && this.m_BullyWalk.GetBool("IsUnique") == false) //If the enemy is cooled down, and is not animating
			{
				this.m_AttackTimer = 0;
				EnemyAttack(m_ThisBoss);
			}

			if (this.m_AnimationLength <= 0)
			{
				this.m_AnimationLength = 0;
				this.m_BullyWalk.SetBool("IsPunch", false);
				this.m_BullyWalk.SetBool("IsKick", false);
				this.m_BullyWalk.SetBool("IsUnique", false);
			}
		}

		if(this.m_CurRow == m_PlayerCurRow)
		{		
			if(this.GetComponent<BeamAttack>().m_Ammo > 0)//if there is ammo
			{
				WaterGun();//fire the water gun
				this.GetComponent<BeamAttack>().m_Ammo--;//decrease ammo
				tempTimer = tempTimerResetVal;
			}
			else //if out of ammo
			{
				--tempTimer;//countdown the timer until reload
			}
			if(tempTimer <= 0)
			{
				Debug.Log("Reload");
				this.GetComponent<BeamAttack>().m_Ammo = Constants.KB_WATER_AMMO;
//				tempTimer = tempTimerResetVal;
			}
		}
	}
}
