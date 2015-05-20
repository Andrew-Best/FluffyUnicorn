using UnityEngine;
using System.Collections;

public class PepperDragon : BossBaseClass 
{
	public GameObject m_Arm1;
	public GameObject m_Arm2;
	public GameObject m_Head;

	public GameObject m_PepperDragon;

	public const float DRAGON_PART_MOVEMENT = 0.5f;

	private void GetRow(GameObject gameObject)
	{

	}

	public void SwitchRow(GameObject head, GameObject hitArm)
	{
		float headStartYPos = head.GetComponent<Rigidbody2D>().transform.position.y;
		float armStartYPos = hitArm.GetComponent<Rigidbody2D>().transform.position.y;

		float headYPosDest = armStartYPos;
		float armYPosDest = headStartYPos;

		float headCurYPos = headStartYPos;
		float armCurYPos = armStartYPos;

		if(headStartYPos > armStartYPos)//head needs to lower, arms needs to rise
		{
			if(headStartYPos > headYPosDest && armStartYPos < armYPosDest )
			{
				head.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -DRAGON_PART_MOVEMENT);
				hitArm.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, DRAGON_PART_MOVEMENT);
			}
		}
		else//head needs to rise, arm needs to lower
		{
			if (headStartYPos < headYPosDest && armStartYPos > armYPosDest)
			{
				head.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, DRAGON_PART_MOVEMENT);
				hitArm.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -DRAGON_PART_MOVEMENT);
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

	
}
