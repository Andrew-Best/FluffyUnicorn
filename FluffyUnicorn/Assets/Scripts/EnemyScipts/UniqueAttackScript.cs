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


	public void BullyUniqueAttack(GameObject bully)//water gun?
	{
		//create BEAM attack
		//water gun animation

	}

	public void FatUniqueAttack(GameObject bully)
	{
		//water gun animation
	}

	public void JockUniqueAttack(GameObject bully)
	{
		//water gun animation
	}

	public void BlingUniqueAttack(GameObject bully)
	{
		//water gun animation
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

	public void UpdateUATKs()
	{
		for(int i = 0; i < bullets.Count; ++i)
		{
			m_Velocity = new Vector2(this.GetComponent<UniqueAttackScript>().m_ShotSpeed, 0);

			bullets[i].GetComponent<Rigidbody2D>().velocity = m_Velocity;
		}
	}

}
