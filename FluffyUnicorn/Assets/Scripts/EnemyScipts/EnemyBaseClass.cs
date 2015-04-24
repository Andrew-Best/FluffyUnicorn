using UnityEngine;
using System.Collections;

public class EnemyBaseClass : MonoBehaviour
{
	#region Enemy Variables
	public GameObject Bully;
	public Animator m_BullyWalk;

	public int m_VelocityX;
	public int m_CurRow;

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
	public int m_EnemyGoingLeft = 1;

	public Rigidbody2D m_RigidBody;
	public Vector2 m_InitialXY;
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
	}

	public virtual void EnemyStopMotion(GameObject bully)
	{
		this.m_RigidBody.velocity = new Vector2(0, 0); //freeze position
	}

	public virtual void ChasePlayer(Vector2 playerPos, Vector2 enemyPos, GameObject bully)
	{
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
		if(enemyPos.x < playerPos.x  && this.m_VelocityX < 0)
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
		EnemyStopMotion(bully);
		m_EnemyInMotion = false; //prevent continued motion of the bully
		int attackSelector = Random.Range(0, 100);
		if (attackSelector <= m_AttackPunchOdds) //If attack selector is less than the odds of punching
		{
			this.EnemyAttackPunch(); //PAWNCH
			
		}
		else if (attackSelector <= m_AttackKickOdds)//not less than Punch odds, so check if less than kick odds
		{
			this.EnemyAttackKick(); //Kick
		}
		else if (attackSelector >= m_AttackKickOdds)//must be greater than kick odds by now so Unique Attack is called
		{
			this.EnemyAttackUnique(); //
		}
	}

	public virtual void ResetEnemyAttackTimer(float enemyAttackTimer)
	{
		this.m_AttackTimer = enemyAttackTimer; //reassign the attack timer to the enemy's default
		this.m_EnemyInMotion = true; //tell the enemy it can move again
	}

	public virtual void EnemyAttackPunch() //This function is overwritten in the BullyScript
	{		
		this.m_BullyWalk.SetBool("IsPunch", true);
		//play punch animation
		//set delay for the attack countdown timer to resume only when the animation is done
	}

	public virtual void EnemyAttackKick()//This function is overwritten in the BullyScript
	{
		this.m_BullyWalk.SetBool("IsKick", true);
		//play kick animation
		//set delay for the attack countdown timer to resume only when the animation is done	
	}

	public virtual void EnemyAttackUnique()//This function is overwritten in the BullyScript
	{
		this.m_BullyWalk.SetBool("IsUnique", true);
		//play the unique animation
		//set delay for the attack countdown timer to resume only when the animation is done
	}
	#endregion

	#region Enemy Combat
	//Straightforward
	public virtual void EnemyTakeDamage(int damageDealt)
	{
		this.m_BullyWalk.SetBool("IsHit", true);
		this.m_HP -= damageDealt;
		if(m_HP <= 0)
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
	public virtual void InitEnemy(Vector2 spawnPos)
	{
		m_VelocityX = 0;
		m_AttackTimer = 0;
		m_HP = 0;
		m_CurRow = 0;

		m_AttackPunchOdds = 0;
		m_AttackKickOdds = 0;
		m_AttackUniqueOdds = 0;

		m_MaxDist = 0;

		m_InitialXY = spawnPos;
	}
	#endregion

	public virtual void EnemyUpdate(GameObject bully)
	{
		if(this.m_EnemyGoingLeft == -1)
		{
			this.m_BullyWalk.SetInteger("IsWalkingLeft", -1);
		}
		else
		{
			this.m_BullyWalk.SetInteger("IsWalkingLeft", 1);
		}

		Vector2 enemyPos = new Vector2(this.m_RigidBody.position.x, this.m_RigidBody.position.y);
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		Vector2 playerPos = new Vector2(player.GetComponent<Rigidbody2D>().position.x, player.GetComponent<Rigidbody2D>().position.y);

		float differenceThenNow = this.m_InitialXY.x - enemyPos.x;
		float pointB = m_MaxDist;
		float pointA = this.m_InitialXY.x + 1;

		this.EnemyMove(bully);

		if(this.m_isIdle)
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
			if(this.m_EnemyInMotion) //the enemy is awake, chase the player
			{
				ChasePlayer(playerPos, enemyPos, bully);
			}
			if (this.m_AnimationLength > 0) //if animating, subtract Delta.Time
			{
				this.m_AnimationLength -= Time.deltaTime;
			}			
			if(this.m_AttackTimer > 0 && this.m_AnimationLength <= 0) //if the enemy isn't cooled down, and is not animating
			{
				this.m_AttackTimer -= Time.deltaTime;
			}
			if (this.m_AttackTimer <= 0 && this.m_BullyWalk.GetBool("IsPunch") == false && this.m_BullyWalk.GetBool("IsKick") == false && this.m_BullyWalk.GetBool("IsUnique") == false) //If the enemy is cooled down, and is not animating
			{
				this.m_AttackTimer = 0;
				EnemyAttack(bully);
			}

			if(this.m_AnimationLength <= 0)
			{
				this.m_AnimationLength = 0;
				this.m_BullyWalk.SetBool("IsPunch", false);				
				this.m_BullyWalk.SetBool("IsKick", false);
				this.m_BullyWalk.SetBool("IsUnique", false);
			}			
		}		
		//Detect Row
	}

	// Update is called once per frame
	void Update ()
	{
		EnemyUpdate(Bully);
	}

}
