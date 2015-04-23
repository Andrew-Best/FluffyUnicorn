using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class BullyScript : EnemyBaseClass
{
    void Start()
    {
        LoadFromXML();
    }
	#region Creation

	public override void InitEnemy(Vector2 spawnPos)
	{
        LoadFromXML();			//Load bully's stats from xml file
		m_EnemyInMotion = true;	//Make the enemy move when it is spawned
		m_EnemyGoingLeft = 1;	//Set the starting direction
		m_isIdle = true;		//The enemy begins Idle
		m_InitialXY = spawnPos;	//Get the initial position to "anchor" it to		
		m_MaxDist = this.GetComponent<Rigidbody2D>().position.x - Constants.BULLY_MAX_TRAVEL_DIST; //Set the maximum travel distance
	}
	#endregion

	#region Attacks
	public override void EnemyAttackKick()
	{
		//play Bully's Kick Animation
		float AttackTimer = m_AttackResetTime + m_KickRestTime; //assign the particular bully's Resttime for after a Kick
		ResetEnemyAttackTimer(AttackTimer); //Reset the AttackTimer according to the last attack and the bully's default resttime

	}
	public override void EnemyAttackPunch()
	{
		//play Bully's Kick Animation
		float AttackTimer = m_AttackResetTime + m_PunchRestTime; //assign the particular bully's Resttime for after a Punch
		ResetEnemyAttackTimer(AttackTimer); //Reset the AttackTimer according to the last attack and the bully's default resttime

	}
	public override void EnemyAttackUnique()
	{
		//play Bully's Kick Animation
		float AttackTimer = m_AttackResetTime + m_UniqueRestTime; //assign the particular bully's Resttime for after a Unique Attack
		ResetEnemyAttackTimer(AttackTimer); //Reset the AttackTimer according to the last attack and the bully's default resttime
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
			m_DetectionDist = 5;
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
