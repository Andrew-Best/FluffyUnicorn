using UnityEngine;
using System.Collections;

public class BeamAttack : MonoBehaviour
{
	public float m_CooldownTimer;
	public int m_Ammo;
	public Vector2 m_Offset;
//	public float m_Rotation;

	public void UpdateWeapon()
	{
		if (m_CooldownTimer > 0.0f)
		{
			m_CooldownTimer -= Time.deltaTime;
		}
	}

	public GameObject m_ProjectilePrefab;
	public float m_Cooldown;
	public int m_MaxAmmo;

	public void Fire(GameObject bully, GameObject player)
	{
		GameObject projectile = Objectpooler.Instance.GetObjectForType("BlasterProjectile1", false);
		projectile.transform.position = bully.transform.position;
		projectile.transform.rotation = bully.transform.rotation;

		projectile.transform.position = projectile.transform.position + this.m_Offset.x *
										bully.transform.right + this.m_Offset.y *
										bully.transform.up;

		float beamSpeed = projectile.GetComponent<LaserScript>().m_Accel;

		if(bully.GetComponent<EnemyBaseClass>().m_EnemyGoingLeft > 0)//Left is 1, Right is -1
		{
			beamSpeed *= -1;//default is 10 (moving right)
		}
		projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(beamSpeed, 0.0f);

		this.m_CooldownTimer = m_Cooldown;
		this.m_Ammo--;
	}	
}
