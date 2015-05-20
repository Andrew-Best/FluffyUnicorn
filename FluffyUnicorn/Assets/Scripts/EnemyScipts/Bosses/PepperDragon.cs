using UnityEngine;
using System.Collections;

public class PepperDragon : BossBaseClass 
{
	public GameObject m_Arm1;
	public GameObject m_Arm2;
	public GameObject m_Head;

	public GameObject m_PepperDragon;

	private void GetRow(GameObject gameObject)
	{

	}

	private void SwitchRow(GameObject head, GameObject hitArm)
	{
		float headStartYPos = head.GetComponent<Rigidbody2D>().transform.position.y;
		float armStartYPos = head.GetComponent<Rigidbody2D>().transform.position.y;

		float headYPosDest = armStartYPos;
		float armYPosDest = headStartYPos;

		float headCurYPos = headStartYPos;
		float armCurYPos = armStartYPos;

		if(headStartYPos > armStartYPos)//head needs to lower, arms needs to rise
		{
			if(headStartYPos > headYPosDest && armStartYPos < armYPosDest )
			{
				head.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1.0f);
				hitArm.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 1.0f);
			}
		}
		else//head needs to rise, arm needs to lower
		{
			if (headStartYPos < headYPosDest && armStartYPos > armYPosDest)
			{
				head.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 1.0f);
				hitArm.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1.0f);
			}
		}
	}

	private void SpewPepper()
	{

	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter2D(Collider2D playerAttack)
	{
		if (playerAttack.tag == "PlayerProjectile")
		{
			if(this.gameObject.tag == "DragonArm")
			{
				m_PepperDragon.GetComponent<BossBaseClass>().m_HP -= playerAttack.GetComponent<Projectile>().m_Damage;
				SwitchRow(m_Head, this.gameObject);
			}
		}
	}
}
