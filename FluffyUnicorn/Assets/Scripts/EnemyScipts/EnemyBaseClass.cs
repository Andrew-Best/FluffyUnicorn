using UnityEngine;
using System.Collections;

public class EnemyBaseClass : MonoBehaviour
{
	public GameObject Bully;

	public int m_VelocityX;
	public float m_AttackTimer;

	public float m_HP;
	public float m_Damage;

	public int m_AttackPunchOdds;
	public int m_AttackKickOdds;
	public int m_AttackUniqueOdds;

	public Rigidbody2D m_RigidBody;

	public virtual void EnemyMoveLeft()
	{
//		Bully.GetComponent<Rigidbody2D>().transform.position.x += m_VelocityX;
		this.m_RigidBody.velocity = new Vector2(this.m_VelocityX, 0);
	}

	public virtual void EnemyAttack()
	{
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
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		EnemyMoveLeft();
	}


}

/*
public GameObject mPlayer;

public float mXPos;

// Use this for initialization
void Start()
{

}

// Update is called once per frame
void Update()
{

	if ((mPlayer.GetComponent<Rigidbody2D>().transform.position.y - this.transform.position.y) >= 4.5f)
	{
		this.transform.position = mPlayer.GetComponent<Rigidbody2D>().transform.position - new Vector3(mXPos, 4.5f, 0.0f);
	}
	if ((mPlayer.GetComponent<Rigidbody2D>().transform.position.y - this.transform.position.y) <= 4.5f)
	{
		this.transform.position = this.transform.position;
	}
	//this.rigidbody2D.transform.position.x = 0.0f;

}*/