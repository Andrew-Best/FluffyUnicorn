using UnityEngine;
using System.Collections;

public class PepperDragon : BossBaseClass 
{
	public GameObject m_Arm1;
	public GameObject m_Arm2;
	public GameObject m_Head;

	public GameObject m_PepperDragon;

	public string m_PlayerLayer;

	public const float DRAGON_PART_MOVEMENT = 0.5f;

	public void SwitchRow(GameObject head, GameObject hitArm)
	{
		head.GetComponent<PepperDragonHead>().m_ThisHeadStartYPos = m_Head.GetComponent<Rigidbody2D>().transform.position.y;
		hitArm.GetComponent<PepperDragonArm>().m_ThisArmStartPosY = hitArm.GetComponent<Rigidbody2D>().transform.position.y;

		string headStartLayer = head.gameObject.layer.ToString();
		string armStartLayer = hitArm.gameObject.layer.ToString();

		head.GetComponent<PepperDragonHead>().MoveToArmPos(hitArm, armStartLayer, headStartLayer);
		hitArm.GetComponent<PepperDragonArm>().MoveToHeadPos(armStartLayer, headStartLayer);
	}

	protected bool MatchPlayerRowToLayer(string LayerOfEnemy)
	{
		if (this.GetComponent<EnemyBaseClass>().m_PlayerCurRow == 0)
		{
			m_PlayerLayer = "PDBackRow";
		}
		else if (this.GetComponent<EnemyBaseClass>().m_PlayerCurRow == 1)
		{
			m_PlayerLayer = "PDMidRow";
		}
		else if (this.GetComponent<EnemyBaseClass>().m_PlayerCurRow == 2)
		{
			m_PlayerLayer = "PDFrontRow";
		}
		if(LayerOfEnemy == m_PlayerLayer)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private void SpewPepper()
	{

	}

	// Use this for initialization
	void Start () 
	{
	
	}
}
