using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBaseClass : MonoBehaviour
{
	#region Enemy Variables
	public List<GameObject> m_Bullies = new List<GameObject>();
	public GameObject m_UniqueAttackHolder;

	public StateMachine m_StateMachine;

	public GameObject m_Player;// = GameObject.FindGameObjectWithTag("Player");
	private Vector2 m_PlayerPos;// = new Vector2(player.GetComponent<Rigidbody2D>().position.x, player.GetComponent<Rigidbody2D>().position.y);

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

	public int m_EnemyGoingLeft = 1; //1 == left, -1 == right

	public Rigidbody2D m_RigidBody;
	public Vector2 m_InitialXY;

	//two variables for changing tracks
	public float changeTrackCountdown = 2.0f; //primary timer, when this reaches 0 the enemy can change tracks, and the secondary timer start counting down and m_TimerIsCounting is set to false
	public float secondaryTrackTimer = 2.0f; //secondary timer, when this reaches 0 then the primary timer starts counting down and it's bool is set to true

	#endregion

	#region Enemy Movement
	public virtual void EnemyMove(GameObject bully)
	{
		if(this.m_EnemyInMotion)
		{
			this.m_RigidBody.velocity = new Vector2(this.m_VelocityX, 0);//set the enemy's velocity
		}
		
	}

	public virtual void EnemyIdle(GameObject bully, Vector2 enemyPos)
	{
		
		float differenceThenNow = this.m_InitialXY.x - enemyPos.x;
		float pointB = m_MaxDist;
		float pointA = this.m_InitialXY.x + 1;
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
			DetectPlayer(m_Player.transform.position, enemyPos);
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
		this.m_EnemyInMotion = false; //set bool that prevents movement in the update
	}

	public virtual void ChangeTrack(GameObject bully)
	{
		if(!this.m_isIdle)
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
		
	}

	public virtual void ChasePlayer(Vector2 playerPos, Vector2 enemyPos, GameObject bully)
	{
		float curEnemyXPOS = enemyPos.x;
		float lineOfSight = this.m_AttackDist;

		#region track change counter
		if (this.changeTrackCountdown <= 0)//If Timer is at 0
		{
			this.changeTrackCountdown = 0; //set the timer to 0
			this.m_TimerIsCounting = false;//prevent the timer from continueing to count down
			if (this.m_PlayerCurRow != this.m_CurRow) //If not on the same track
			{
				this.ChangeTrack(bully);//will not change track if Idle
				this.secondaryTrackTimer = Constants.TRACK_COUNTDOWN_DEFAULT; // The secondary timer is assigned its value
			}
		}
		#endregion

		if(playerPos.x > curEnemyXPOS)//if the player is on the right
		{
			//if the player is less to the right than the currentposition of the enemy's line of sight
			if(playerPos.x < enemyPos.x + lineOfSight)
			{
				
				EnemyStopMotion(bully);
			}
			else
			{
				EnemyMove(bully);
			}
		}
		else //the player is on the left
		{
			//if the player is less to the left than the enemy's pos - it's line of sight
			if(playerPos.x > enemyPos.x - lineOfSight)
			{
				
				//EnemyStopMotion(bully);
			}
			else
			{
				EnemyMove(bully);
			}
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

		if(this.m_CurRow == this.m_PlayerCurRow)
		{
			/*the difference between the player and bully is less than the detection dist on the right, OR if it is greater than the detection on the left*/
			if(differenceInDistance.x <= forwardDetectionX || differenceInDistance.x >= -forwardDetectionX)
			{
				this.m_isIdle = false;//then the enemy is no longer Idle	
			}
		}
		else
		{
			if (differenceInDistance.x <= forwardDetectionX)//if the player is within the detection "range" of a bully
			{
				this.m_isIdle = false;//then the enemy is no longer Idle	
			}
		}

	}
	#endregion

	#region Enemy Attacks
	public virtual void EnemyAttack(GameObject bully)
	{
		int attackSelector = Random.Range(0, 100);
		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);
		if (this.m_CurRow == this.m_PlayerCurRow)
		{			
//			m_EnemyInMotion = false; //prevent continued motion of the bully
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
//		this.m_EnemyInMotion = true; //tell the enemy it can move again
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
		changeTrackCountdown = m_ChangeTrackTimer;

		m_Player = GameObject.FindGameObjectWithTag("Player");
		m_PlayerPos = new Vector2(m_Player.GetComponent<Rigidbody2D>().position.x, m_Player.GetComponent<Rigidbody2D>().position.y);
		
		m_InitialXY = spawnPos;
	}
	#endregion

	public virtual void GetPlayerInfo(GameObject thisEnemy)
	{
		if (this.m_Player.GetComponent<PlayerController>().m_onFrontTrack)
		{
			this.m_PlayerCurRow = 2;
		}
		else if (this.m_Player.GetComponent<PlayerController>().m_onMiddleTrack)
		{
			this.m_PlayerCurRow = 1;
		}
		else if (this.m_Player.GetComponent<PlayerController>().m_onLastTrack)
		{
			this.m_PlayerCurRow = 0;
		}

		m_PlayerPos = m_Player.GetComponent<Rigidbody2D>().transform.position;
	}

	public virtual void EnemyUpdate(GameObject bully)
	{
		Vector2 enemyPos = new Vector2(this.m_RigidBody.position.x, this.m_RigidBody.position.y);
		this.m_UniqueAttackHolder.GetComponent<UniqueAttackScript>().UpdateUATKs(bully); //Update Enemy Projectiles on screen

		m_PlayerPos = new Vector2(m_Player.GetComponent<Rigidbody2D>().position.x, m_Player.GetComponent<Rigidbody2D>().position.y);
	
		//Detect Player Track
		GetPlayerInfo(bully);

		//Conditions for changing tracks
		if (!this.m_TimerIsCounting) //if the primary timer is not able to count down (disabled)
		{
			this.changeTrackCountdown = Constants.TRACK_COUNTDOWN_DEFAULT; //set the primary timer to its default value
			this.secondaryTrackTimer -= Time.deltaTime; // decrement the secondary timer
		}
		if(secondaryTrackTimer <= 0) //once the secondary timer reaches 0
		{
			this.m_TimerIsCounting = true; //enable the primary timer
			this.secondaryTrackTimer = Constants.TRACK_COUNTDOWN_DEFAULT; //set the secondary timer to it's default value
		}
		if(this.m_TimerIsCounting)
		{
			this.changeTrackCountdown -= Time.deltaTime;
		}

		this.GetComponent<Rigidbody2D>().transform.position = new Vector2(this.GetComponent<Rigidbody2D>().transform.position.x, this.m_TargetPoints[m_CurRow].transform.position.y);

		

		if(this.m_EnemyInMotion)
		{
			this.EnemyMove(bully);
		}

		if (this.m_isIdle)
		{
			this.EnemyIdle(bully, enemyPos);
		}
		else // enemy is not idle, therefore player is nearby
		{
			this.ChasePlayer(m_PlayerPos, enemyPos, bully);
			
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
				this.m_EnemyInMotion = true;
			}
		}
	}


	// Update is called once per frame
	void Update()
	{
		for(int i = 0; i < m_Bullies.Count; ++i)
		{
			EnemyUpdate(m_Bullies[i]);
		}
	}

}
