using UnityEngine;
using System.Collections;

public class JunkScript : MonoBehaviour 
{
	public GameObject QueenBully_;
	public GameObject Player_;

	public float m_HP;
	public float m_ReactForce = 0.4f;
	public int m_Damage = 1;

    private float Death_ = 4.0f;
    private Rigidbody2D rigidbody_; //The object that has this script's rigidbody

	// Use this for initialization
	void Start () 
	{
		if(this.name == "DeadFish")
		{
			m_HP = Constants.DEAD_FISH_HP;
		}
		if (this.name == "Popcan")
		{
			m_HP = Constants.POP_CAN_HP;
		}
		if (this.name == "BurntToast")
		{
			m_HP = Constants.BURNT_TOAST_HP;
		}


        rigidbody_ = GetComponent<Rigidbody2D>();
		//QueenBully_ = GameObject.FindGameObjectWithTag("QueenBully");
		//Player_ = GameObject.FindGameObjectWithTag("Player");

        rigidbody_.transform.position = QueenBully_.transform.position;
        rigidbody_.velocity = ArcShot(Player_.transform, Constants.ARC_DEGREE);//throw angle
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			//
            rigidbody_.velocity = new Vector2(0, 0);
			Player_.GetComponent<PlayerData>().m_PlayerHealth -= m_Damage;
            rigidbody_.isKinematic = true;

		}
		if (collision.gameObject.tag == "PlayerProjectile")
		{
			m_HP -= GetComponent<Projectile>().m_Damage;
            rigidbody_.velocity = new Vector2(0, 0);
            rigidbody_.isKinematic = true;
		}
		if (collision.gameObject.tag == "PlayerProjectile2")
		{
			m_HP -= GetComponent<Projectile>().m_Damage;
            rigidbody_.velocity = new Vector2(0, 0);
            rigidbody_.isKinematic = true;
		}
		if (collision.gameObject.tag == "PlayerProjectile3")
		{
			m_HP -= GetComponent<Projectile>().m_Damage;
            rigidbody_.velocity = new Vector2(0, 0);
            rigidbody_.isKinematic = true;
		}
        rigidbody_.AddForce(new Vector2(m_ReactForce, 0.0f));//Brandon's Wiggle
	}

	Vector2 ArcShot(Transform target, float angle)
	{
		Vector2 targetPos = new Vector2(target.position.x, target.position.y);
		Vector2 junkPos = new Vector2(this.transform.position.x, this.transform.position.y);
		Vector2 direction = targetPos - junkPos;

		float height = direction.y;
		direction.y = 0;
		float distance = direction.magnitude; //horizontal distance
		float radAngle = angle * Mathf.Deg2Rad;
		direction.y = distance * Mathf.Tan(radAngle);
		distance += height / Mathf.Tan(radAngle);

		float vel = Mathf.Sqrt(distance * Physics2D.gravity.magnitude / Mathf.Sin(2 * radAngle));
		return vel * direction.normalized;

	}


	// Update is called once per frame
	void Update () 
	{
		if(this.m_HP <= 0)
		{
			Destroy(this.gameObject);
		}

        Death_ -= Time.deltaTime;
        if(Death_ <= 0.0f)
        {
            Destroy(gameObject);
            Death_ = 4.0f;
        }
	}
}
