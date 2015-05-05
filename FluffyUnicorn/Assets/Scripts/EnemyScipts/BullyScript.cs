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
		this.m_UniqueAttackHolder = GameObject.FindGameObjectWithTag("UATKHolder");
		this.PepperSpray = this.m_UniqueAttackHolder.GetComponent<UniqueAttackScript>().m_PepperSpray;
    }

	#region Creation

	public override void InitEnemy(Vector2 spawnPos, int row)
	{
		base.InitEnemy(spawnPos, row);

        LoadFromXML();			//Load bully's stats from xml file
		m_EnemyInMotion = true;	//Make the enemy move when it is spawned
		m_EnemyGoingLeft = 1;	//Set the starting direction
		m_isIdle = true;		//The enemy begins Idle
		m_InitialXY = spawnPos;	//Get the initial position to "anchor" it to	

		m_CurRow = row;

		this.m_TargetPoints[0] = GameObject.FindGameObjectWithTag("TargetLastTrack");

		this.m_TargetPoints[1] = GameObject.FindGameObjectWithTag("TargetMidTrack");

		this.m_TargetPoints[2] = GameObject.FindGameObjectWithTag("TargetFrontTrack");

		this.changeTrackCountdown = this.m_ChangeTrackTimer;
		m_MaxDist = this.GetComponent<Rigidbody2D>().position.x - Constants.BULLY_MAX_TRAVEL_DIST; //Set the maximum travel distance

	//	Bully.GetComponent<Rigidbody2D>().transform.position = new Vector3(Bully.transform.position.x, m_TargetPoints[(int)spawnPos.x].transform.position.y, m_TargetPoints[(int)spawnPos.y].transform.position.z);

	}
	#endregion

	#region Attacks
	public override void EnemyAttackKick(GameObject bully)
	{
		//play Bully's Kick Animation
		this.m_BullyWalk.SetBool("IsKick", true);
		this.m_AnimationLength = 3;
		float AttackTimer = m_AttackResetTime + m_KickRestTime; //assign the particular bully's Resttime for after a Kick
		ResetEnemyAttackTimer(AttackTimer); //Reset the AttackTimer according to the last attack and the bully's default resttime
	}

	public override void EnemyAttackPunch(GameObject bully)
	{
		//play Bully's Kick Animation
		this.m_BullyWalk.SetBool("IsPunch", true);
		this.m_AnimationLength = 2;
		float AttackTimer = m_AttackResetTime + m_PunchRestTime; //assign the particular bully's Resttime for after a Punch
		this.ResetEnemyAttackTimer(AttackTimer); //Reset the AttackTimer according to the last attack and the bully's default resttime
	}

	public override void EnemyAttackUnique(GameObject bully)
	{
		//play Bully's Kick Animation
		this.m_BullyWalk.SetBool("IsUnique", true);
		if (bully.name == "PepperBully")
		{
			m_UniqueAttackHolder.GetComponent<UniqueAttackScript>().PepperUniqueAttack(bully);
		}
		else if (bully.name == "FatBully")
		{
			m_UniqueAttackHolder.GetComponent<UniqueAttackScript>().FatUniqueAttack(bully);
		}
		else if (bully.name == "JockBully")
		{
			m_UniqueAttackHolder.GetComponent<UniqueAttackScript>().JockUniqueAttack(bully);
		}
		else if (bully.name == "BlingBully")
		{
			m_UniqueAttackHolder.GetComponent<UniqueAttackScript>().BlingUniqueAttack(bully);
		}
		else if (bully.name == "Bully")
		{
			m_UniqueAttackHolder.GetComponent<UniqueAttackScript>().BullyUniqueAttack(bully);
		}

		this.m_AnimationLength = 10;
		float AttackTimer = m_AttackResetTime + m_UniqueRestTime; //assign the particular bully's Resttime for after a Unique Attack
		this.ResetEnemyAttackTimer(AttackTimer); //Reset the AttackTimer according to the last attack and the bully's default resttime
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
				m_AttackDist = int.Parse(node.Attributes["AttackDist"].Value);
				m_ChangeTrackTimer = int.Parse(node.Attributes["ChangeTrackDelay"].Value);
            }
        }
    }
    #endregion
}
