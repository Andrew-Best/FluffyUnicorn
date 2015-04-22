using UnityEngine;
using System.Collections;

public class EnemyBaseClass : MonoBehaviour
{
	#region Enemy Variables
	public GameObject Bully;

	public int m_VelocityX;
	public int m_CurRow;

	public int m_EnemyType;

	public float m_AttackTimer;

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

	public bool m_EnemyInMotion;
	public bool m_isIdle;
	public int m_EnemyGoingLeft = 1;

	public Rigidbody2D m_RigidBody;
	public Vector2 m_InitialXY;

	public bool m_JustSpawned = true;
	#endregion

	#region Enemy Movement
	public virtual void EnemyMoveLeft(GameObject bully)
	{
		//		Bully.GetComponent<Rigidbody2D>().transform.position.x += m_VelocityX;
		this.m_RigidBody.velocity = new Vector2(this.m_VelocityX, 0);
	}

	public virtual void TurnArond(GameObject bully)
	{
		//		this.m_RigidBody.velocity *= new Vector2(-1,0);
		m_VelocityX *= -1;
		this.m_EnemyGoingLeft *= -1;
	}

	public virtual void EnemyStopMotion(GameObject bully)
	{
		this.m_RigidBody.velocity = new Vector2(0, 0);
	}

	//Detect the Player
	public virtual void DetectPlayer(Vector2 playerPos, Vector2 enemyPos)
	{
		Vector2 differenceDistance = playerPos - enemyPos;
		//if the difference from the current enemy position and the player's current position
		//is less than the Detection Distance of the enemy 
		if (differenceDistance.x <= this.m_DetectionDist)
		{
			//then the enemy is no longer Idle	
			//this.m_isIdle = false;
		}
	}
	#endregion

	#region Enemy Attacks
	public virtual void EnemyAttack()
	{
		m_EnemyInMotion = false; //Stop the Enemy's movement when attacking
		int attackSelector = Random.Range(0, 100);
		if (attackSelector <= m_AttackPunchOdds) //If attack selector is less than the odds of punching
		{
			EnemyAttackPunch();
		}
		else if (attackSelector <= m_AttackKickOdds)//not less than Punch odds, so check if less than kick odds
		{
			EnemyAttackKick();
		}
		else if (attackSelector >= m_AttackKickOdds)//must be greater than kick odds by now so Unique Attack is called
		{
			EnemyAttackUnique();
		}
		m_EnemyInMotion = true; //Start the Enemy's movement again
	}

	public virtual void ResetEnemyAttackTimer(float enemyAttackTimer)
	{
		this.m_AttackTimer = enemyAttackTimer;
	}

	public virtual void EnemyAttackPunch()
	{
		//play punch animation
	}

	public virtual void EnemyAttackKick()
	{
		//play kick animation
	}

	public virtual void EnemyAttackUnique()
	{
		//play the unique animation
	}
	#endregion

	#region Enemy Combat
	public virtual void EnemyTakeDamage(int damageDealt)
	{
		this.m_HP -= damageDealt;
		if(m_HP <= 0)
		{
			KillEnemy(this.gameObject);
		}
	}

	public virtual void KillEnemy(GameObject enemy)
	{
		//play enemy death animation
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
		//m_Damage = 0;

		m_AttackPunchOdds = 0;
		m_AttackKickOdds = 0;
		m_AttackUniqueOdds = 0;

		m_MaxDist = 0;

		m_InitialXY = spawnPos;


			//new Vector2(this.GetComponent<Rigidbody2D>().transform.position.x, this.GetComponent<Rigidbody2D>().transform.position.y);
	}
	public virtual void SpawnEnemy(int row, int type)
	{
//		m_CurRow = row;
//			InitEnemy();
//		this.transform.position = new Vector2(0, 0);
	}
	#endregion

	public virtual void EnemyUpdate(GameObject bully)
	{
		Vector2 enemyPos = new Vector2(this.m_RigidBody.position.x, this.m_RigidBody.position.y);
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		Vector2 playerPos = new Vector2(player.GetComponent<Rigidbody2D>().position.x, player.GetComponent<Rigidbody2D>().position.y);

		GameObject tempBully = bully;

		float differenceThenNow = this.m_InitialXY.x - enemyPos.x;
		float pointB = m_MaxDist;
		float pointA = m_MaxDist * -1;

		//if the bully is travelling left and has exceeded the MxTravelDistance, turn around
		//if the bully is travelling right and has exceeded the -MxTravelDistance, turn around
		if(m_JustSpawned)
		{
			m_JustSpawned = false;
			this.EnemyMoveLeft(bully);
		}
		if(this.m_isIdle)
		{
				//moving left and has exceed the MxTravelDistance
			if(differenceThenNow > pointA && differenceThenNow < pointB)
			{
				this.TurnArond(bully);
			}
				//moving right and has exceeded the -MxTravelDistance
			else if (differenceThenNow > pointB && differenceThenNow < pointA)
			{
				this.TurnArond(bully);
			}
		}
		else // enemy is not idle, therefore player is nearby
		{
			this.EnemyMoveLeft(bully);
		}

		m_AttackTimer -= Time.deltaTime;
		if (m_AttackTimer <= 0)
		{
			EnemyAttack();
		}
		//Detect Row
	}
	// Update is called once per frame
	void Update ()
	{
		EnemyUpdate(Bully);
	}

}
