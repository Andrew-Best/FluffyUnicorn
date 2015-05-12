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
	private float AttackUniqueCurTime = 0;

	private bool BullyUATK_;
	private bool JockUATK_;
	private bool FatUATK_;
	private bool BlingUATK_;
	private bool PepperUATK_;

	public const float DEFAULT_PEPPER_SPEED = 5;//Move to Constants file (default moves to the right)

	public void Start()
	{
		m_ShotSpeed = DEFAULT_PEPPER_SPEED;
	}	

	public void BullyUniqueAttack(GameObject bully)//Done
	{
		//create BEAM attack
		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);
		bully.GetComponent<BeamAttack>().Fire(bully, bully.GetComponent<EnemyBaseClass>().m_Player);

		this.m_AttackUniqueAnimLength = Constants.BULLY_UNIQUE_ATK_LENGTH;
		this.AttackUniqueCurTime = 0;
		//water gun animation
	}

	public void FatUniqueAttack(GameObject bully)//Requires Animation
	{
		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);

		this.m_AttackUniqueAnimLength = Constants.FAT_UNIQUE_ATK_LENGTH;
		this.AttackUniqueCurTime = 0;

		this.FatUATK_ = true;

		bully.GetComponent<Rigidbody2D>().velocity = this.m_Velocity * 2;	
	}

	public void JockUniqueAttack(GameObject bully)//Requires Animation
	{
		//Charge Anim
		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);

		this.m_AttackUniqueAnimLength = Constants.JOCK_UNIQUE_ATK_LENGTH;
		this.AttackUniqueCurTime = 0;
		this.JockUATK_ = true;

		//increase Velocity x
		bully.GetComponent<Rigidbody2D>().velocity += new Vector2(this.m_Velocity.x * 2, this.GetComponent<Rigidbody2D>().velocity.y);
		
	}

	public void BlingUniqueAttack(GameObject bully)//Requires Animation
	{
		//freeze position
		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);

		this.m_AttackUniqueAnimLength = Constants.BLING_UNIQUE_ATK_LENGTH;
		this.AttackUniqueCurTime = 0;
		this.BlingUATK_ = true;
		//cane stab animation
	}

	public void PepperUniqueAttack(GameObject bully)//Done
	{
		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);
		//Get a bullet from the ObjectPool
		GameObject bullet = ObjectPool.Instance.GetObjectForType(m_ProjectileName, true);
		bullet.transform.position = bully.transform.position;
		this.GetComponent<UniqueAttackScript>().m_ShotSpeed = DEFAULT_PEPPER_SPEED;//positive number, moves to the right
		if(bully.GetComponent<EnemyBaseClass>().m_EnemyGoingLeft > 0)//if bully moving left
		{
			this.GetComponent<UniqueAttackScript>().m_ShotSpeed *= -1;//make the shot go left
		}
		bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<UniqueAttackScript>().m_ShotSpeed, 0);
		bullets.Add(bullet);

		this.m_AttackUniqueAnimLength = Constants.PEPPER_UNIQUE_ATK_LENGTH;
		this.AttackUniqueCurTime = 0;
		this.PepperUATK_ = true;
	}

	public void UpdateUATKs(GameObject bully)
	{
		//update Pepper Spray
		for(int i = 0; i < bullets.Count; ++i)
		{
			//bullets[i].GetComponent<Rigidbody2D>().velocity = this.m_Velocity;
			//change to Screen width
			if(bullets[i].GetComponent<Rigidbody2D>().transform.position.x <= -25 || bullets[i].GetComponent<Rigidbody2D>().transform.position.x >= 25)
			{
				Destroy(bullets[i]);
				bullets.Remove(bullets[i].gameObject);
			}
			
//			if(Collider2D)
		}
		//update Water Gun
		if (this.BullyUATK_)
		{
			this.AttackUniqueCurTime += Time.deltaTime;
			if (this.AttackUniqueCurTime >= this.m_AttackUniqueAnimLength)
			{
				this.BullyUATK_ = false;
				bully.GetComponent<EnemyBaseClass>().m_EnemyInMotion = true;
			}
		}
		//update Jock Headbutt
		if(this.JockUATK_)
		{
			this.AttackUniqueCurTime += Time.deltaTime;
			if(this.AttackUniqueCurTime >= this.m_AttackUniqueAnimLength)
			{
				this.JockUATK_ = false;
				bully.GetComponent<EnemyBaseClass>().m_EnemyInMotion = true;
			}
		}

		//update Fat Barrel
		if(this.FatUATK_)
		{
			this.AttackUniqueCurTime += Time.deltaTime;
			if (this.AttackUniqueCurTime >= this.m_AttackUniqueAnimLength)
			{
				this.FatUATK_ = false;
				bully.GetComponent<EnemyBaseClass>().m_EnemyInMotion = true;
			}
		}

		//update PepperBully
		if (this.PepperUATK_)
		{
			this.AttackUniqueCurTime += Time.deltaTime;
			if (this.AttackUniqueCurTime >= this.m_AttackUniqueAnimLength)
			{
				this.PepperUATK_ = false;
				bully.GetComponent<EnemyBaseClass>().m_EnemyInMotion = true;
			}
		}
		
		//update Bling Cane Stab
		if (this.BlingUATK_)
		{
			this.AttackUniqueCurTime += Time.deltaTime;
			if (this.AttackUniqueCurTime >= this.m_AttackUniqueAnimLength)
			{
				this.BlingUATK_ = false;
				bully.GetComponent<EnemyBaseClass>().m_EnemyInMotion = true;
			}
		}
	}

}
