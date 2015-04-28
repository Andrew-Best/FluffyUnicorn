using UnityEngine;
using System.Collections;

public class UniqueAttackScript : MonoBehaviour
{
	public GameObject m_PepperSpray;

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
		//create Projectile Attack

		//Get a bullet from the ObjectPool
		GameObject bullet = ObjectPool.Instance.GetObjectForType(m_ProjectileName, true);
		bullet.transform.position = bully.transform.position;
		//Determine which direction to fire in
	}

	public void UpdateUATKs(GameObject bully, GameObject bullet)
	{
		if (bully.GetComponent<BullyScript>().m_EnemyGoingLeft == 1)
		{
			bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(this.m_ShotSpeed, 0);
//			bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(this.m_ShotSpeed, 0, 0));
		}
		else
		{
			bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-this.m_ShotSpeed, 0);
//			bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(-this.m_ShotSpeed, 0, 0));
		}
	}

}
