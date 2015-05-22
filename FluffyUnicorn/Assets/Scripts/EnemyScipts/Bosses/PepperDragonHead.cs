using UnityEngine;
using System.Collections;

public class PepperDragonHead : PepperDragon 
{
	public float m_ThisHeadCurPosY;
	public float m_ThisHeadDestPosY;
	public float m_ThisHeadStartYPos;

	public GameObject m_ArmToMove;

	private bool HeadIsMoving_;

	public GameObject MoveToArmPos(GameObject hitArm, int armLayerIndex, int headLayerIndex)
	{
		this.gameObject.layer = armLayerIndex;
		m_ArmToMove = hitArm;
		HeadIsMoving_ = true;
		return m_ArmToMove;
	}

	// Use this for initialization
	void Start () 
	{
		HeadIsMoving_ = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_ThisHeadCurPosY = this.GetComponent<Rigidbody2D>().transform.position.y;
		if(HeadIsMoving_)
		{
			if (m_ThisHeadStartYPos > m_ArmToMove.GetComponent<PepperDragonArm>().m_ThisArmStartPosY)//the head is higher and must be lowered
			{
				if (m_ThisHeadCurPosY > m_ArmToMove.GetComponent<PepperDragonArm>().m_ThisArmStartPosY)
				{
					this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1.0f);
				}
				else
				{
					this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
					HeadIsMoving_ = false;
				}
			}
			else//the arm is lower and must be raised
			{
				if (m_ThisHeadCurPosY < m_ArmToMove.GetComponent<PepperDragonArm>().m_ThisArmStartPosY)
				{
					this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 1.0f);
				}
				else
				{
					this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
					HeadIsMoving_ = false;
				}
			}
			ChangeZPos();
		}
		
	}
}
