using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour 
{
	public float mAccel = 1.0f;
	public int mDamage = 1;
	public float mLifetime = 10.0f; //in seconds
	public float mDeadTime = 10.0f;
	private float lifeTimer_;

	private bool isDead_;
	private float emissionRate_;

	// Use this for initialization
	void Start()
	{
		lifeTimer_ = 0.0f;
		isDead_ = false;
		emissionRate_ = GetComponent<ParticleSystem>().emissionRate;
		//	GetComponent<ParticleSystem>().active(true);

	}

	void Update()
	{
		lifeTimer_ += Time.deltaTime;

		if (!isDead_ && lifeTimer_ > mLifetime)
		{
			//ObjectPool.Instance.PoolObject(gameObject);
			isDead_ = true;
			GetComponent<Collider2D>().enabled = false;
			lifeTimer_ -= mLifetime;
		}
		else if (isDead_ && lifeTimer_ > mDeadTime)
		{
			isDead_ = false;
			GetComponent<Collider2D>().enabled = true;
			GetComponent<ParticleSystem>().emissionRate = emissionRate_;
			lifeTimer_ = 0.0f;
			Objectpooler.Instance.PoolObject(gameObject);
		}

	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (!isDead_)
		{
			GetComponent<Rigidbody2D>().AddForce(-(transform.up * mAccel));
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		PlayerController hitShip = collision.gameObject.GetComponentInChildren<PlayerController>();
		if (hitShip != null)
		{
/*			hitShip.ApplyDamage(mDamage);
			GetComponent<Collider2D>().enabled = false;
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			GetComponent<ParticleSystem>().emissionRate = 0.0f;
			lifeTimer_ = 0.0f;
			isDead_ = true;*/
		}
	}

}
