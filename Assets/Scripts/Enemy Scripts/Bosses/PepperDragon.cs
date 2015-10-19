using UnityEngine;
using System.Collections;

public class PepperDragon : BossBaseClass 
{
	public GameObject m_Arm1;
	public GameObject m_Arm2;
	public GameObject m_Head;
	public int m_DestroyedArms;

	public GameObject m_PepperDragon;
	
	protected string layerNameAsIndex;
	protected int layerIndex;
	protected string curLayerName;

	public string m_PlayerLayer;

	public void SwitchRow(GameObject head, GameObject hitArm)
	{
		head.GetComponent<PepperDragonHead>().m_ThisHeadStartYPos = m_Head.GetComponent<Rigidbody2D>().transform.position.y;
		hitArm.GetComponent<PepperDragonArm>().m_ThisArmStartPosY = hitArm.GetComponent<Rigidbody2D>().transform.position.y;

		string headStartLayer = head.gameObject.layer.ToString();
		string armStartLayer = hitArm.gameObject.layer.ToString();

		int headStartLayerIndex = int.Parse(headStartLayer);
		int armStartLayerIndex = int.Parse(armStartLayer);

		head.GetComponent<PepperDragonHead>().MoveToArmPos(hitArm, armStartLayerIndex, headStartLayerIndex);
		hitArm.GetComponent<PepperDragonArm>().MoveToHeadPos(armStartLayerIndex, headStartLayerIndex);
	}

	protected void GetCurLayer()
	{
		layerNameAsIndex = this.gameObject.layer.ToString();
		layerIndex = int.Parse(layerNameAsIndex);

		curLayerName = LayerMask.LayerToName(layerIndex);
	}
	protected void ChangeZPos()
	{
		float curVelX = this.gameObject.GetComponent<Rigidbody2D>().velocity.x;
		float curVelY = this.gameObject.GetComponent<Rigidbody2D>().velocity.y;

		float curZPos = this.gameObject.GetComponent<Rigidbody2D>().transform.position.z;
		Vector3 curVel = this.gameObject.GetComponent<Rigidbody2D>().velocity;

		if (curLayerName == "PDBackRow")
		{
			//If the layer is the BackRow, and the Z pos isn't the assigned backrow z pos
			if (curZPos != Constants.PEPPER_DRAGON_Z_POS_BACKROW)
			{
				//if the z pos is higher than the position it should be then
				if (curZPos > Constants.PEPPER_DRAGON_Z_POS_BACKROW)
				{
					//assign the velocity to move the object lower until is reaches the designated position
					curVel = new Vector3(curVelX, curVelY, -Constants.PEPPER_DRAGON_Z_VELOCITY);
				}
				else
				{
					curVel = new Vector3(curVelX, curVelY, Constants.PEPPER_DRAGON_Z_VELOCITY);
				}				
			}
		}
		if (curLayerName == "PDMidRow")
		{
			if (curZPos != Constants.PEPPER_DRAGON_Z_POS_MIDROW)
			{
				if (curZPos > Constants.PEPPER_DRAGON_Z_POS_MIDROW)
				{
					curVel = new Vector3(curVelX, curVelY, -Constants.PEPPER_DRAGON_Z_VELOCITY);
				}
				else
				{
					curVel = new Vector3(curVelX, curVelY, Constants.PEPPER_DRAGON_Z_VELOCITY);
				}
			}
		}
		if (curLayerName == "PDFrontRow")
		{
			if (curZPos != Constants.PEPPER_DRAGON_Z_POS_FRONTROW)
			{
				if (curZPos > Constants.PEPPER_DRAGON_Z_POS_FRONTROW)
				{
					curVel = new Vector3(curVelX, curVelY, -Constants.PEPPER_DRAGON_Z_VELOCITY);
				}
				else
				{
					curVel = new Vector3(curVelX, curVelY, Constants.PEPPER_DRAGON_Z_VELOCITY);
				}
			}
		}
		this.gameObject.GetComponent<Rigidbody2D>().velocity = curVel;
	}
    /*
	protected bool MatchPlayerRowToLayer(string LayerOfEnemy)
	{
		GetPlayerInfo(this.gameObject);
		int curPlayerRow = m_PlayerCurRow;

		if (curPlayerRow == 2)
		{
			m_PlayerLayer = "PDFrontRow";
		}
		else if (curPlayerRow == 1)
		{
			m_PlayerLayer = "PDMidRow";
		}
		else if (curPlayerRow == 0)
		{
			m_PlayerLayer = "PDBackRow";
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
    */
	private void SpewPepper()
	{

	}

	// Use this for initialization
	void Start () 
	{
		m_Player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update()
	{
		if(m_DestroyedArms == 2)
		{
			//PepperDragon is defeated
			Destroy(this.gameObject);
		}
	}
}
