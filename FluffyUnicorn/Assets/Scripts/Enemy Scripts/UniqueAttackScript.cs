using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UniqueAttackScript : MonoBehaviour
{
	public GameObject m_PepperSpray;
	public GameObject m_UATKController;
	public List<GameObject> bullets = new List<GameObject>();

	public Vector2 m_Velocity;
	public float m_ShotSpeed;
	public string m_ProjectileName = "PepperSpray";

	public float m_AttackUniqueAnimLength;
	private float attackUniqueCurTime = 0;

	private bool bullyUATK_;
	private bool jockUATK_;
	private bool fatUATK_;
	private bool blingUATK_;
	private bool pepperUATK_;

	

	public void Start()
	{
		m_ShotSpeed = Constants.DEFAULT_PEPPER_SPEED;
	}	

	public void BullyUniqueAttack(GameObject bully)//Done
	{
		//create BEAM attack
		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);
		bully.GetComponent<BeamAttack>().Fire(bully, bully.GetComponent<EnemyBaseClass>().m_Player);

		this.m_AttackUniqueAnimLength = Constants.BULLY_UNIQUE_ATK_LENGTH;
        this.attackUniqueCurTime = 0;
		//water gun animation
	}

	public void FatUniqueAttack(GameObject bully)//Requires Animation
	{
		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);

		this.m_AttackUniqueAnimLength = Constants.FAT_UNIQUE_ATK_LENGTH;
        this.attackUniqueCurTime = 0;

		this.fatUATK_ = true;

		bully.GetComponent<Rigidbody2D>().velocity = this.m_Velocity * 2;	
	}

	public void JockUniqueAttack(GameObject bully)//Requires Animation
	{
		//Charge Anim
		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);

		this.m_AttackUniqueAnimLength = Constants.JOCK_UNIQUE_ATK_LENGTH;
        this.attackUniqueCurTime = 0;
		this.jockUATK_ = true;

		//increase Velocity x
		bully.GetComponent<Rigidbody2D>().velocity += new Vector2(this.m_Velocity.x * 2, this.GetComponent<Rigidbody2D>().velocity.y);
		
	}

	public void BlingUniqueAttack(GameObject bully)//Requires Animation
	{
		//freeze position
		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);

		this.m_AttackUniqueAnimLength = Constants.BLING_UNIQUE_ATK_LENGTH;
        this.attackUniqueCurTime = 0;
		this.blingUATK_ = true;
		//cane stab animation
	}

	public void PepperUniqueAttack(GameObject bully)//Done
	{
		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);
		//Get a bullet from the ObjectPool
		GameObject bullet = ObjectPool.Instance.GetObjectForType(m_ProjectileName, true);
		bullet.transform.position = bully.transform.position;
		this.GetComponent<UniqueAttackScript>().m_ShotSpeed = Constants.DEFAULT_PEPPER_SPEED;//positive number, moves to the right
		if(bully.GetComponent<EnemyBaseClass>().m_EnemyGoingLeft > 0)//if bully moving left
		{
			this.GetComponent<UniqueAttackScript>().m_ShotSpeed *= -1;//make the shot go left
		}
		bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<UniqueAttackScript>().m_ShotSpeed, 0);
		bullets.Add(bullet);

		this.m_AttackUniqueAnimLength = Constants.PEPPER_UNIQUE_ATK_LENGTH;
        this.attackUniqueCurTime = 0;
		this.pepperUATK_ = true;
	}

	public void UpdateUATKs(GameObject bully)
	{
		//update Pepper Spray
		for(int i = 0; i < bullets.Count; ++i)
		{
			//bullets[i].GetComponent<Rigidbody2D>().velocity = this.m_Velocity;
			//change to Screen width
			if(bullets[i] == null)
			{
				bullets.Remove(bullets[i].gameObject);
			}
			else if(bullets[i].GetComponent<Rigidbody2D>().transform.position.x <= -25 || bullets[i].GetComponent<Rigidbody2D>().transform.position.x >= 25)
			{
				Destroy(bullets[i]);
				bullets.Remove(bullets[i].gameObject);
			}
			
//			if(Collider2D)
		}
		//update Water Gun
		if (this.bullyUATK_)
		{
            this.attackUniqueCurTime += Time.deltaTime;
            if (this.attackUniqueCurTime >= this.m_AttackUniqueAnimLength)
			{
				this.bullyUATK_ = false;
				bully.GetComponent<EnemyBaseClass>().m_EnemyInMotion = true;
			}
		}
		//update Jock Headbutt
		if(this.jockUATK_)
		{
            this.attackUniqueCurTime += Time.deltaTime;
            if (this.attackUniqueCurTime >= this.m_AttackUniqueAnimLength)
			{
				this.jockUATK_ = false;
				bully.GetComponent<EnemyBaseClass>().m_EnemyInMotion = true;
			}
		}

		//update Fat Barrel
		if(this.fatUATK_)
		{
            this.attackUniqueCurTime += Time.deltaTime;
            if (this.attackUniqueCurTime >= this.m_AttackUniqueAnimLength)
			{
				this.fatUATK_ = false;
				bully.GetComponent<EnemyBaseClass>().m_EnemyInMotion = true;
			}
		}

		//update PepperBully
		if (this.pepperUATK_)
		{
            this.attackUniqueCurTime += Time.deltaTime;
            if (this.attackUniqueCurTime >= this.m_AttackUniqueAnimLength)
			{
				this.pepperUATK_ = false;
				bully.GetComponent<EnemyBaseClass>().m_EnemyInMotion = true;
			}
		}
		
		//update Bling Cane Stab
		if (this.blingUATK_)
		{
            this.attackUniqueCurTime += Time.deltaTime;
            if (this.attackUniqueCurTime >= this.m_AttackUniqueAnimLength)
			{
				this.blingUATK_ = false;
				bully.GetComponent<EnemyBaseClass>().m_EnemyInMotion = true;
			}
		}
	}

}
