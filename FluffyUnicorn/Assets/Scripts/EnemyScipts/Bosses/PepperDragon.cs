using UnityEngine;
using System.Collections;

public class PepperDragon : BossBaseClass 
{
	public GameObject m_Arm1;
	public GameObject m_Arm2;
	public GameObject m_Head;

	public GameObject m_PepperDragon;

	public const float DRAGON_PART_MOVEMENT = 0.5f;

	public void SwitchRow(GameObject head, GameObject hitArm)
	{
		head.GetComponent<PepperDragonHead>().m_ThisHeadStartYPos = m_Head.GetComponent<Rigidbody2D>().transform.position.y;
		hitArm.GetComponent<PepperDragonArm>().m_ThisArmStartPosY = hitArm.GetComponent<Rigidbody2D>().transform.position.y;

		head.GetComponent<PepperDragonHead>().MoveToArmPos(hitArm);
		hitArm.GetComponent<PepperDragonArm>().MoveToHeadPos();
	}

	private void SpewPepper()
	{

	}

	// Use this for initialization
	void Start () 
	{
	
	}
}
