using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class BullyScript : EnemyBaseClass
{
//	m_EnemyType = 1;

    void Start()
    {
        LoadFromXML();
    }
	#region Creation
/*	public override void SpawnEnemy(int row, int type)
	{
		//base.SpawnEnemy(row, type);
		this.InitEnemy();
	}*/
	public override void InitEnemy(Vector2 spawnPos)
	{
//		base.InitEnemy();
        LoadFromXML();
		/*m_VelocityX = Constants.BULLY_VEL_X;
		m_AttackTimer = Constants.BULLY_ATTACK_TIMER_RESET_VALUE;
		m_HP = Constants.BULLY_HP;
		m_Damage = 0;*/
		//		m_CurRow = Random.Range(1, 3);
		m_EnemyInMotion = true;

		m_EnemyGoingLeft = 1;

		/*m_AttackPunchOdds = Constants.BULLY_PUNCH_ODDS;
		m_AttackKickOdds = Constants.BULLY_KICK_ODDS;
		m_AttackUniqueOdds = Constants.BULLY_UNIQUE_ATK_ODDS;*/

		m_isIdle = true;
		m_InitialXY = spawnPos;
		
		m_MaxDist = this.GetComponent<Rigidbody2D>().position.x - Constants.BULLY_MAX_TRAVEL_DIST;
	}
	#endregion

	#region Attacks
	public override void EnemyAttackKick()
	{
		//play Bully's Kick Animation
		//m_Damage = Constants.BULLY_KICK_DAMAGE;
		float AttackTimer = Constants.BULLY_ATTACK_TIMER_RESET_VALUE + Constants.BULLY_KICK_RESTTIME;
		ResetEnemyAttackTimer(AttackTimer);

	}
	public override void EnemyAttackPunch()
	{
		//play Bully's Kick Animation
		//m_Damage = Constants.BULLY_PUNCH_DAMAGE;
		float AttackTimer = Constants.BULLY_ATTACK_TIMER_RESET_VALUE + Constants.BULLY_PUNCH_RESTTIME;
          ResetEnemyAttackTimer(AttackTimer);

	}
	public override void EnemyAttackUnique()
	{
		//play Bully's Kick Animation
		//m_Damage = Constants.BULLY_UNIQUE_ATTACK_DAMAGE;
		float AttackTimer = Constants.BULLY_ATTACK_TIMER_RESET_VALUE + Constants.BULLY_UNIQUE_ATK_RESTTIME;
          ResetEnemyAttackTimer(AttackTimer);
	}
	#endregion

    #region Data Loading
    void LoadFromXML()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load("./Assets/XML/BullyStats.xml");

        XmlNode root = xmlDoc.FirstChild;

        foreach(XmlNode node in root.ChildNodes)
        {
            Debug.Log("Node name: " + node.Name + "   This name: " + this.name);
            //Find the node with a matching name as GameObject
            if(node.Name == this.name)
            {
                m_HP = float.Parse(node.Attributes["HP"].Value); //Load the HP value from the XML file
                m_PunchDamage = float.Parse(node.Attributes["PunchDamage"].Value); //Load the punch damage value from the XML file
                m_KickDamage = float.Parse(node.Attributes["KickDamage"].Value); //Load the kick damage value from the XML file
                m_UniqueDamage = float.Parse(node.Attributes["UniqueDamage"].Value); //Load the unique damage value from the XML file
                m_PunchRestTime = float.Parse(node.Attributes["PunchRest"].Value); //Load the punch rest time from the XML file
                m_KickRestTime = float.Parse(node.Attributes["KickRest"].Value); //Load the kick rest time from the XML file
                m_UniqueRestTime = float.Parse(node.Attributes["UniqueRest"].Value); //Load the unique attack rest time from the XML file
                m_AttackPunchOdds = int.Parse(node.Attributes["PunchOdds"].Value); //Load the punch odds from the XML file
                m_AttackKickOdds = int.Parse(node.Attributes["KickOdds"].Value); //Load the kick odds from the XML file
                m_AttackUniqueOdds = int.Parse(node.Attributes["UniqueOdds"].Value); //Load the unique attack odds from the XML file
                m_AttackResetTime = float.Parse(node.Attributes["AttackReset"].Value); //Load the attack reset time from the XML file
                m_VelocityX = int.Parse(node.Attributes["Velocity"].Value); //Load the velocity from the XML file
            }
        }
    }
    #endregion
}
