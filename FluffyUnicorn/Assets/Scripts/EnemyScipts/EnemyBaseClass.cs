using UnityEngine;
using System.Collections;

public class EnemyBaseClass : MonoBehaviour
{
	#region Enemy Variables
	public GameObject Bully;
	public GameObject m_UniqueAttackHolder;

	public GameObject PepperSpray;

	public Animator m_BullyWalk;
	public Collider2D[] m_Tracks;
	public GameObject[] m_TargetPoints;

	public int m_VelocityX;

	public int m_PlayerCurRow;
	public int m_CurRow;
	public bool m_AbleToChangeTrack = false;
	public float m_ChangeTrackTimer;

	public int m_EnemyType;

	public float m_AttackTimer;
	public float m_AnimationLength;

	public float m_HP;
	public float m_PunchDamage;
	public float m_KickDamage;
	public float m_UniqueDamage;

	public float m_AttackResetTime;
	public float m_PunchRestTime;
	public float m_KickRestTime;
	public float m_UniqueRestTime;

	public int m_AttackPunchOdds;
	public int m_AttackKickOdds;
	public int m_AttackUniqueOdds;
	public float m_MaxDist;
	public float m_DetectionDist;
	public float m_AttackDist;

	public bool m_EnemyInMotion;
	public bool m_isIdle;
	public bool m_TimerIsCounting; //bool for Timer for changing tracks (primary timer)
    public bool m_IsDead = false;

	public int m_EnemyGoingLeft = 1;

	public Rigidbody2D m_RigidBody;
	public Vector2 m_InitialXY;

	//two variables for changing tracks
	public float changeTrackCountdown = 2.0f; //primary timer, when this reaches 0 the enemy can change tracks, and the secondary timer start counting down and m_TimerIsCounting is set to false
	public float secondaryTrackTimer = 2.0f; //secondary timer, when this reaches 0 then the primary timer starts counting down and it's bool is set to true

	public const float TRACK_COUNTDOWN_DEFAULT = 2;
	#endregion

	#region Enemy Movement
	public virtual void EnemyMove(GameObject bully)
	{
		this.m_RigidBody.velocity = new Vector2(this.m_VelocityX, 0);//set the enemy's velocity
	}

	public virtual void TurnAround(GameObject bully)
	{
		this.m_VelocityX *= -1; //turn the enemy around
		this.m_EnemyGoingLeft *= -1; //tell the enemy it has turned around

		//Brandon's code to fip the animation,,, flips the x 
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public virtual void EnemyStopMotion(GameObject bully)
	{
		this.m_RigidBody.velocity = new Vector2(0, 0); //freeze position
	}

	public virtual void ChangeTrack(GameObject bully)
	{
		
		if (this.m_CurRow < this.m_PlayerCurRow)
		{
			this.m_CurRow++;
		}
		else if (this.m_CurRow > this.m_PlayerCurRow)
		{
			this.m_CurRow--;
		}
		else //function should not have been called in the first place
		{
			Debug.Log("Why was this even called?");
		}
	}

	public virtual void ChasePlayer(Vector2 playerPos, Vector2 enemyPos, GameObject bully, float playerPosY, float thisEnemyYPos)
	{
		if (this.changeTrackCountdown <= 0)//If Timer is at 0
		{
			this.changeTrackCountdown = 0; //set the timer to 0
			this.m_TimerIsCounting = false;//prevent the timer from continueing to count down
			if (this.m_PlayerCurRow != this.m_CurRow) //If not on the same track
			{
				this.ChangeTrack(bully);
				this.secondaryTrackTimer = TRACK_COUNTDOWN_DEFAULT; // The secondary timer is assigned its value
			}
		}
		//If the enemy is moving Left and 5 pixels to the left of the player, STOP
		if (this.m_EnemyGoingLeft == -1 && enemyPos.x + this.m_AttackDist > playerPos.x || this.m_EnemyGoingLeft == 1 && enemyPos.x - this.m_AttackDist < playerPos.x)
		{
			this.EnemyStopMotion(bully);
		}
		else
		{
			this.EnemyMove(bully);
		}
		//If the enemy is to the left of the player and if the enemy is moving to the right
		if (enemyPos.x < playerPos.x && this.m_VelocityX < 0)
		{
			this.TurnAround(bully); //correct movement direction
		}
		//If the enemy is to the right, and moving left
		if (enemyPos.x > playerPos.x && this.m_VelocityX > 0)
		{
			this.TurnAround(bully); //correct movement direction
		}
	}

	public virtual void DetectPlayer(Vector2 playerPos, Vector2 enemyPos)
	{
		Vector2 differenceInDistance = enemyPos - playerPos; //get the difference between the two entities
		float forwardDetectionX = enemyPos.x - this.m_DetectionDist; //x position player has to reach or pass for the enemy to wake up

		if (playerPos.x >= forwardDetectionX)//if the player is within the detection "range" of a bully
		{
			this.m_isIdle = false;//then the enemy is no longer Idle	
		}
	}
	#endregion

	#region Enemy Attacks
	public virtual void EnemyAttack(GameObject bully)
	{
		int attackSelector = Random.Range(0, 100);
		if (this.m_CurRow == this.m_PlayerCurRow)
		{
			EnemyStopMotion(bully);
			m_EnemyInMotion = false; //prevent continued motion of the bully
			if (attackSelector <= m_AttackPunchOdds) //If attack selector is less than the odds of punching
			{
				this.EnemyAttackPunch(bully); //PAWNCH

			}
			else if (attackSelector <= m_AttackKickOdds)//not less than Punch odds, so check if less than kick odds
			{
				this.EnemyAttackKick(bully); //Kick
			}
			else if (attackSelector >= m_AttackKickOdds)//must be greater than kick odds by now so Unique Attack is called
			{
				this.EnemyAttackUnique(bully); //
			}
		}
		if (attackSelector >= m_AttackKickOdds)//must be greater than kick odds by now so Unique Attack is called
		{
			this.EnemyAttackUnique(bully); //
		}
	}

	public virtual void ResetEnemyAttackTimer(float enemyAttackTimer)
	{
		this.m_AttackTimer = enemyAttackTimer; //reassign the attack timer to the enemy's default
		this.m_EnemyInMotion = true; //tell the enemy it can move again
	}

	public virtual void EnemyAttackPunch(GameObject bully) //This function is overwritten in the BullyScript
	{
		this.m_BullyWalk.SetBool("IsPunch", true);
		//play punch animation
		//set delay for the attack countdown timer to resume only when the animation is done
	}

	public virtual void EnemyAttackKick(GameObject bully)//This function is overwritten in the BullyScript
	{
		this.m_BullyWalk.SetBool("IsKick", true);
		//play kick animation
		//set delay for the attack countdown timer to resume only when the animation is done	
	}

	public virtual void EnemyAttackUnique(GameObject bully)//This function is overwritten in the BullyScript
	{
	}
	#endregion

	#region Enemy Combat
	//Straightforward
	public virtual void EnemyTakeDamage(int damageDealt)
	{
		this.m_BullyWalk.SetBool("IsHit", true);
		this.m_HP -= damageDealt;
		if (m_HP <= 0)
		{
			KillEnemy(this.gameObject);
		}
	}

	public virtual void KillEnemy(GameObject enemy)
	{
		this.m_BullyWalk.SetBool("IsDead", true);//play enemy death animation
		GameObject.Destroy(transform.root.gameObject);
	}
	#endregion

	#region Creation
	public virtual void InitEnemy(Vector2 spawnPos, int row)
	{
		m_VelocityX = 0;
		m_AttackTimer = 0;
		m_HP = 0;
		m_CurRow = 0;

		m_AttackPunchOdds = 0;
		m_AttackKickOdds = 0;
		m_AttackUniqueOdds = 0;

		m_MaxDist = 0;
		changeTrackCountdown = m_ChangeTrackTimer;

		//		Bully.GetComponent<Rigidbody2D>().transform.position = new Vector3(Bully.transform.position.x, m_TargetPoints[(int) spawnPos.x].transform.position.y, m_TargetPoints[(int) spawnPos.y].transform.position.z);

		m_InitialXY = spawnPos;
	}
	#endregion

	public virtual void EnemyUpdate(GameObject bully, GameObject bullets)
	{
		this.m_UniqueAttackHolder.GetComponent<UniqueAttackScript>().UpdateUATKs(bully, bullets);

		//Conditions for changing tracks
		if (!this.m_TimerIsCounting) //if the primary timer is not able to count down (disabled)
		{
			this.changeTrackCountdown = TRACK_COUNTDOWN_DEFAULT; //set the primary timer to its default value
			this.secondaryTrackTimer -= Time.deltaTime; // decrement the secondary timer
		}
		if(secondaryTrackTimer <= 0) //once the secondary timer reaches 0
		{
			this.m_TimerIsCounting = true; //enable the primary timer
			this.secondaryTrackTimer = TRACK_COUNTDOWN_DEFAULT; //set the secondary timer to it's default value
		}
		if(this.m_TimerIsCounting)
		{
			this.changeTrackCountdown -= Time.deltaTime;
		}


		//Change Bully's Y position accordin to the track it is on
/*		float frontTrackY = this.m_TargetPoints[2].transform.position.y;
		float midTrackY = this.m_TargetPoints[1].transform.position.y;
		float lastTrackY = this.m_TargetPoints[0].transform.position.y;*/


		this.GetComponent<Rigidbody2D>().transform.position = new Vector2(this.GetComponent<Rigidbody2D>().transform.position.x, this.m_TargetPoints[m_CurRow].transform.position.y);

		

		//Change direction of Anim
		if (this.m_EnemyGoingLeft == -1)
		{
			//this.m_BullyWalk.SetInteger("IsWalkingLeft 0", -1);
		}
		else if (this.m_EnemyGoingLeft == 1)
		{
			//this.m_BullyWalk.SetInteger("IsWalkingLeft 0", 1);
		}

		Vector2 enemyPos = new Vector2(this.m_RigidBody.position.x, this.m_RigidBody.position.y);
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		Vector2 playerPos = new Vector2(player.GetComponent<Rigidbody2D>().position.x, player.GetComponent<Rigidbody2D>().position.y);

		//Detect Player Track
		if (player.GetComponent<PlayerController>().m_onFrontTrack)
		{
			m_PlayerCurRow = 2;
		}
		else if (player.GetComponent<PlayerController>().m_onMiddleTrack)
		{
			m_PlayerCurRow = 1;
		}
		else if (player.GetComponent<PlayerController>().m_onLastTrack)
		{
			m_PlayerCurRow = 0;
		}

		float differenceThenNow = this.m_InitialXY.x - enemyPos.x;
		float pointB = m_MaxDist;
		float pointA = this.m_InitialXY.x + 1;

		this.EnemyMove(bully);

		if (this.m_isIdle)
		{
			//moving right and has passed pointA
			if (enemyPos.x >= pointA && this.m_EnemyGoingLeft == -1)
			{
				enemyPos.x = pointA;
				this.TurnAround(bully);
			}
			//moving left and has passed point B
			if (enemyPos.x <= pointB && this.m_EnemyGoingLeft == 1)
			{
				this.TurnAround(bully);
			}
			DetectPlayer(player.transform.position, enemyPos);
		}
		else // enemy is not idle, therefore player is nearby
		{
			if (this.m_EnemyInMotion) //the enemy is awake, chase the player
			{
				ChasePlayer(playerPos, enemyPos, bully, player.GetComponent<Rigidbody2D>().transform.position.y, enemyPos.y);
			}
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
				EnemyAttack(bully);
			}

			if (this.m_AnimationLength <= 0)
			{
				this.m_AnimationLength = 0;
				this.m_BullyWalk.SetBool("IsPunch", false);
				this.m_BullyWalk.SetBool("IsKick", false);
				this.m_BullyWalk.SetBool("IsUnique", false);
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		EnemyUpdate(Bully, PepperSpray);
	}

}
