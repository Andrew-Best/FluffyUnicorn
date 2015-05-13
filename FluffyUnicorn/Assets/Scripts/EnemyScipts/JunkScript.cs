using UnityEngine;
using System.Collections;

public class JunkScript : MonoBehaviour 
{
	public float m_HP;
	public float m_ReactForce = 0.4f;
	public int m_Damage = 1;

	// Use this for initialization
	void Start () 
	{
		if(this.name == "DeadFish")
		{
			m_HP = Constants.DEAD_FISH_HP;
		}
		if (this.name == "DeadFish")
		{
			m_HP = Constants.POP_CAN_HP;
		}
		if (this.name == "DeadFish")
		{
			m_HP = Constants.BURNT_TOAST_HP;
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			//
			GetComponent<PlayerData>().m_PlayerHealth -= m_Damage;

		}
		if (collision.gameObject.tag == "PlayerProjectile")
		{
			m_HP -= GetComponent<Projectile>().m_Damage;
		}
		if (collision.gameObject.tag == "PlayerProjectile2")
		{
			m_HP -= GetComponent<Projectile>().m_Damage;
		}
		if (collision.gameObject.tag == "PlayerProjectile3")
		{
			m_HP -= GetComponent<Projectile>().m_Damage;
		}
		this.GetComponent<Rigidbody2D>().AddForce(new Vector2(m_ReactForce, 0.0f));//Brandon's Wiggle
	}


	// Update is called once per frame
	void Update () 
	{
		if (this.GetComponent<Rigidbody2D>().transform.position.y > 15)
		{
			this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, 0);
		}

		if(this.m_HP <= 0)
		{
			Destroy(this.gameObject);
		}
	}
}
