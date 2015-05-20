using UnityEngine;
using System.Collections;

public class PepperDragonArm : MonoBehaviour 
{
	public GameObject m_PepperDragon;
	public GameObject m_Head;

	void OnTriggerEnter2D(Collider2D playerAttack)
	{
		if (playerAttack.tag == "PlayerProjectile")
		{
			if (this.gameObject.tag == "DragonArm")
			{
				m_PepperDragon.GetComponent<BossBaseClass>().m_HP -= playerAttack.GetComponent<Projectile>().m_Damage;
				m_PepperDragon.GetComponent<PepperDragon>().SwitchRow(m_Head, this.gameObject);
			}
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
