using UnityEngine;
using System.Collections;

public class PepperDragonHead : PepperDragon 
{
	public float m_ThisHeadCurPosY;
	public float m_ThisHeadDestPosY;
	public float m_ThisHeadStartYPos;

	public void MoveToArmPos(GameObject hitArm)
	{
		if (m_ThisHeadStartYPos > hitArm.GetComponent<PepperDragonArm>().m_ThisArmStartPosY)//the head is higher and must be lowered
		{
			if (m_ThisHeadCurPosY > hitArm.GetComponent<PepperDragonArm>().m_ThisArmStartPosY)
			{
				this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1.0f);
			}
		}
		else//the arm is lower and must be raised
		{
			if (m_ThisHeadCurPosY < hitArm.GetComponent<PepperDragonArm>().m_ThisArmStartPosY)
			{
				this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 1.0f);
			}
		}
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_ThisHeadCurPosY = this.GetComponent<Rigidbody2D>().transform.position.y;
	}
}
