using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UniqueAttackScript : MonoBehaviour
{
	public GameObject m_PepperSpray;
	public List<GameObject> bullets = new List<GameObject>();
	public LineRenderer line;

	public Vector2 m_Velocity;
	public float m_ShotSpeed;
	public string m_ProjectileName = "PepperSpray";

	public float m_AttackUniqueAnimLength;
	private float AttackUniqueCurTime = 0;

	private Vector2 oriVel;

	private bool JockUATK;
	private bool FatUATK;

	public BullyScript m_Data;
	public string mExplosionName = "Explosion1";

	public void FireWeapons(GameObject bully, GameObject player)
	{
		bully.GetComponent<BeamAttack>().Fire(bully, player);
	}

	public void Start()
	{
		line = gameObject.GetComponent<LineRenderer>();
		line.enabled = false;
	}
	

	public void BullyUniqueAttack(GameObject bully)//water gun?
	{
		//create BEAM attack
//		Fire(WeaponStateData stateData, GameObject bully, string collisionLayer)

		bully.GetComponent<BeamAttack>().Fire(bully, bully.GetComponent<BullyScript>().m_Player);
//		BeamAttack.
		//water gun animation

	}

	public void FatUniqueAttack(GameObject bully)
	{
		this.oriVel = bully.GetComponent<Rigidbody2D>().velocity;
		this.m_AttackUniqueAnimLength = Constants.FAT_UNIQUE_ATK_LENGTH;
		this.AttackUniqueCurTime = 0;

		this.FatUATK = true;

		bully.GetComponent<Rigidbody2D>().velocity = this.m_Velocity * 2;
		//water gun animation
	}

	public void JockUniqueAttack(GameObject bully)
	{
		//Charge Anim
		this.oriVel = bully.GetComponent<Rigidbody2D>().velocity;
		this.m_AttackUniqueAnimLength = Constants.JOCK_UNIQUE_ATK_LENGTH;
		this.AttackUniqueCurTime = 0;
		this.JockUATK = true;

		//increase Velocity x
		this.GetComponent<Rigidbody2D>().velocity += new Vector2(this.m_Velocity.x * 2, this.GetComponent<Rigidbody2D>().velocity.y);
		
	}

	public void BlingUniqueAttack(GameObject bully)
	{
		//freeze position
		//cane stab animation
	}

	public void PepperUniqueAttack(GameObject bully)
	{
		//Get a bullet from the ObjectPool
		GameObject bullet = ObjectPool.Instance.GetObjectForType(m_ProjectileName, true);
		bullet.transform.position = bully.transform.position;
		if(bully.GetComponent<BullyScript>().m_VelocityX < 0)//if bully moving left
		{
			this.m_ShotSpeed *= -1;
		}
		bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(this.m_ShotSpeed, 0);

		bullets.Add(bullet);
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
		//update Jock Headbutt
		if(this.JockUATK)
		{
			this.AttackUniqueCurTime += Time.deltaTime;
			if(this.AttackUniqueCurTime >= this.m_AttackUniqueAnimLength)
			{
				this.JockUATK = false;
				bully.GetComponent<Rigidbody2D>().velocity = this.oriVel;
			}
		}

		//update Fat Barrel
		if(this.FatUATK)
		{
			this.AttackUniqueCurTime += Time.deltaTime;
			if (this.AttackUniqueCurTime >= this.m_AttackUniqueAnimLength)
			{
				this.FatUATK = false;
				bully.GetComponent<Rigidbody2D>().velocity = this.oriVel;
			}
		}
		
		//update Bling Cane Stab
	}

}
