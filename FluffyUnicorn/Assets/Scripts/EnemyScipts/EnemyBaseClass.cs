using UnityEngine;
using System.Collections;

public class EnemyBaseClass : MonoBehaviour
{
	public GameObject Bully;

	public int m_VelocityX;
	public int m_CurRow;

	public int m_EnemyType;

	public float m_AttackTimer;

	public float m_HP;
	public float m_Damage;

	public int m_AttackPunchOdds;
	public int m_AttackKickOdds;
	public int m_AttackUniqueOdds;

	public bool m_EnemyInMotion;

	public Rigidbody2D m_RigidBody;

	public virtual void InitEnemy()
	{
		m_VelocityX = 0;
		m_AttackTimer = 0;
		m_HP = 0;
		m_CurRow = 0;
		m_Damage = 0;

		m_AttackPunchOdds = 0;
		m_AttackKickOdds = 0;
		m_AttackUniqueOdds = 0;
	}

	public virtual void EnemyMoveLeft()
	{
//		Bully.GetComponent<Rigidbody2D>().transform.position.x += m_VelocityX;
		this.m_RigidBody.velocity = new Vector2(this.m_VelocityX, 0);
	}

	public virtual void EnemyStopMotion()
	{
		this.m_RigidBody.velocity = new Vector2(0, 0);
	}

	public virtual void EnemyAttack()
	{
		m_EnemyInMotion = false; //Stop the Enemy's movement when attacking
		int attackSelector = Random.Range(0, 100);
		if(attackSelector <= m_AttackPunchOdds) //If attack selector is less than the odds of punching
		{
			EnemyAttackPunch();
		}
		else if(attackSelector <= m_AttackKickOdds)//not less than Punch odds, so check if less than kick odds
		{
			EnemyAttackKick();
		}
		else if(attackSelector >= m_AttackKickOdds)//must be greater than kick odds by now so Unique Attack is called
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
	
	// Update is called once per frame
	void Update ()
	{
		if (m_EnemyInMotion)
		{
			EnemyMoveLeft();
		}
		else
		{
			EnemyStopMotion();
          }
		
		m_AttackTimer -= Time.deltaTime;
		if(m_AttackTimer <= 0)
		{
			EnemyAttack();
		}
		//Detect Row

	}

	public virtual void SpawnEnemy(int row, int type)
	{
		m_CurRow = row;
		if (type == 1)
		{
			InitEnemy();
		}
		this.transform.position = new Vector2(0,0);
	}

}
