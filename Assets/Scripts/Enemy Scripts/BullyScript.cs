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
        this.m_NavAgent = GetComponent<NavMeshAgent>();
        this.m_NavAgent.updateRotation = false;
    }
   
	#region Creation
    public override void InitEnemy(Vector3 spawnPos, Vector3 enemyPos, GameObject newBully)
	{
		base.InitEnemy(spawnPos, enemyPos, newBully);

		this.m_EnemyController = GameObject.FindGameObjectWithTag("EnemyController");
        this.LoadFromXML();			//Load bully's stats from xml file
		this.m_EnemyInMotion = true;	//Make the enemy move when it is spawned
		this.m_EnemyGoingLeft = 1;	//Set the starting direction
		this.m_isIdle = true;		//The enemy begins Idle
		this.m_InitialXY = spawnPos;	//Get the initial position to "anchor" it to	

		this.m_UniqueAttackHolder = GameObject.FindGameObjectWithTag("UATKHolder");
		this.PepperSpray = this.m_UniqueAttackHolder.GetComponent<UniqueAttackScript>().m_PepperSpray;


		this.m_TargetPoints[0] = GameObject.FindGameObjectWithTag("Targetpoint2");

		this.m_TargetPoints[1] = GameObject.FindGameObjectWithTag("Targetpoint1");

		this.m_TargetPoints[2] = GameObject.FindGameObjectWithTag("Targetpoint0");

		m_MaxDist = this.GetComponent<Rigidbody>().position.x - Constants.BULLY_MAX_TRAVEL_DIST; //Set the maximum travel distance

        newBully.GetComponent<Rigidbody>().transform.position = new Vector3(newBully.transform.position.x, newBully.transform.position.y, newBully.transform.position.z);

	}
	#endregion

	#region Attacks
	public override void EnemyAttackKick(GameObject bully)
	{
		//play Bully's Kick Animation
		this.m_BullyAnimator.SetBool("IsKick", true);
		this.m_AnimationLength = 3;
		float AttackTimer = m_AttackResetTime + m_KickRestTime; //assign the particular bully's Resttime for after a Kick
		this.ResetEnemyAttackTimer(AttackTimer); //Reset the AttackTimer according to the last attack and the bully's default resttime
	}

	public override void EnemyAttackPunch(GameObject bully)
	{
		//play Bully's Kick Animation
		this.m_BullyAnimator.SetBool("IsPunch", true);
		this.m_AnimationLength = 2;
		float AttackTimer = m_AttackResetTime + m_PunchRestTime; //assign the particular bully's Resttime for after a Punch
		this.ResetEnemyAttackTimer(AttackTimer); //Reset the AttackTimer according to the last attack and the bully's default resttime
	}

	public override void EnemyAttackUnique(GameObject bully)
	{
		//play Bully's Kick Animation
		this.m_BullyAnimator.SetBool("IsUnique", true);
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
		float AttackTimer = this.m_AttackResetTime + this.m_UniqueRestTime; //assign the particular bully's Resttime for after a Unique Attack
		this.ResetEnemyAttackTimer(AttackTimer); //Reset the AttackTimer according to the last attack and the bully's default resttime
	}
	#endregion

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

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
			
            }
        }
    }
    #endregion
}
