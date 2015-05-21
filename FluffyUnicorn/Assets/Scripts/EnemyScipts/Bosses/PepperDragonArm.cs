using UnityEngine;
using System.Collections;

public class PepperDragonArm : PepperDragon
{
	public float m_ThisArmCurPosY;
	public float m_ThisArmDestPosY;//AKA the start Y of the head
	public float m_ThisArmStartPosY;

	bool ArmIsMoving_;

	public GameObject m_DragonHead;

	public void MoveToHeadPos(string armLayer, string headLayer)
	{		
		ArmIsMoving_ = true;
		this.gameObject.layer = LayerMask.NameToLayer(headLayer);
	}

	void OnTriggerEnter2D(Collider2D playerAttack)
	{
		if (playerAttack.tag == "PlayerProjectile")
		{
			string ass = this.gameObject.layer.ToString();
			int assValue = int.Parse(ass);

			string curLayerName = LayerMask.LayerToName(assValue);
		
			if (MatchPlayerRowToLayer(curLayerName))
			{
				
				m_PepperDragon.GetComponent<BossBaseClass>().m_HP -= playerAttack.GetComponent<Projectile>().m_Damage;
				m_PepperDragon.GetComponent<PepperDragon>().SwitchRow(m_Head, this.gameObject);
			}
		}
	}

	// Use this for initialization
	void Start()
	{
		ArmIsMoving_ = false;
	}

	// Update is called once per frame
	void Update()
	{
		m_ThisArmCurPosY = this.GetComponent<Rigidbody2D>().transform.position.y;
		if(ArmIsMoving_)
		{
			if (m_ThisArmStartPosY > m_DragonHead.GetComponent<PepperDragonHead>().m_ThisHeadStartYPos)//the arm is higher and must be lowered
			{
				if (m_ThisArmCurPosY > m_DragonHead.GetComponent<PepperDragonHead>().m_ThisHeadStartYPos)
				{
					this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1.0f);
				}
			}
			else//the arm is lower and must be raised
			{
				if (m_ThisArmCurPosY < m_DragonHead.GetComponent<PepperDragonHead>().m_ThisHeadStartYPos)
				{
					this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 1.0f);
				}
			}
		}
	}

}
