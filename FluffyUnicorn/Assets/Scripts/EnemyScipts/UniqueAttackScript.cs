using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UniqueAttackScript : MonoBehaviour
{
	public GameObject m_PepperSpray;
	public List<GameObject> bullets = new List<GameObject>();

	public Vector2 m_Velocity;
	public float m_ShotSpeed;
	public string m_ProjectileName = "PepperSpray";

	public float m_AttackUniqueAnimLength;
	private float AttackUniqueCurTime = 0;

	private Vector2 oriVel;

	private bool BullyUATK_;
	private bool JockUATK_;
	private bool FatUATK_;
	private bool BlingUATK_;
	private bool PepperUATK_;

	public BullyScript m_Data;
	public string mExplosionName = "Explosion1";

	public void FireWeapons(GameObject bully, GameObject player)
	{
		bully.GetComponent<BeamAttack>().Fire(bully, player);
	}

	public void Start()
	{
	}
	

	public void BullyUniqueAttack(GameObject bully)//water gun?
	{
		//create BEAM attack
		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);
		bully.GetComponent<BeamAttack>().Fire(bully, bully.GetComponent<BullyScript>().m_Player);

		this.m_AttackUniqueAnimLength = Constants.BULLY_UNIQUE_ATK_LENGTH;
		this.AttackUniqueCurTime = 0;
		//water gun animation

	}

	public void FatUniqueAttack(GameObject bully)
	{

		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);

		this.m_AttackUniqueAnimLength = Constants.FAT_UNIQUE_ATK_LENGTH;
		this.AttackUniqueCurTime = 0;

		this.FatUATK_ = true;

		bully.GetComponent<Rigidbody2D>().velocity = this.m_Velocity * 2;
		//water gun animation
	}

	public void JockUniqueAttack(GameObject bully)
	{
		//Charge Anim

		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);

		this.m_AttackUniqueAnimLength = Constants.JOCK_UNIQUE_ATK_LENGTH;
		this.AttackUniqueCurTime = 0;
		this.JockUATK_ = true;

		//increase Velocity x
		this.GetComponent<Rigidbody2D>().velocity += new Vector2(this.m_Velocity.x * 2, this.GetComponent<Rigidbody2D>().velocity.y);
		
	}

	public void BlingUniqueAttack(GameObject bully)
	{
		//freeze position
		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);

		this.m_AttackUniqueAnimLength = Constants.BLING_UNIQUE_ATK_LENGTH;
		this.AttackUniqueCurTime = 0;
		this.BlingUATK_ = true;
		//cane stab animation
	}

	public void PepperUniqueAttack(GameObject bully)
	{
		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);
		//Get a bullet from the ObjectPool
		GameObject bullet = ObjectPool.Instance.GetObjectForType(m_ProjectileName, true);
		bullet.transform.position = bully.transform.position;
		if(bully.GetComponent<BullyScript>().m_VelocityX < 0)//if bully moving left
		{
			this.m_ShotSpeed *= -1;
		}
		bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(this.m_ShotSpeed, 0);

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
			m_Velocity = new Vector2(this.GetComponent<UniqueAttackScript>().m_ShotSpeed, 0);

			bullets[i].GetComponent<Rigidbody2D>().velocity = m_Velocity;
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
