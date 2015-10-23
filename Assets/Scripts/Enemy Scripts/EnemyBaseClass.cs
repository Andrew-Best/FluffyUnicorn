using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBaseClass : MonoBehaviour
{
	#region Enemy Variables
	public List<GameObject> m_Bullies = new List<GameObject>();

	public GameObject m_EnemyController;
	public GameObject m_UniqueAttackHolder;

	public StateMachine m_StateMachine;

	public GameObject m_Player;// = GameObject.FindGameObjectWithTag("Player");
	public Vector2 m_PlayerPos;// = new Vector2(player.GetComponent<Rigidbody2D>().position.x, player.GetComponent<Rigidbody2D>().position.y);

	public GameObject PepperSpray;

	public Animator m_BullyAnimator;

	public GameObject[] m_TargetPoints;

	public int m_VelocityX;
	public int m_EnemyType;

	public float m_AttackTimer;
	public float m_AnimationLength;

	public float m_HP;
	public float m_PunchDamage;
	public float m_KickDamage;
	public float m_UniqueDamage;

	public float m_AttackResetTime;
	public float m_PunchRestTime;
	public float m_KickRestTime;
	public float m_UniqueRestTime;

	public int m_AttackPunchOdds;
	public int m_AttackKickOdds;
	public int m_AttackUniqueOdds;
	public float m_MaxDist;
	public float m_DetectionDist;
	public float m_AttackDist;
	public float m_ReactForce = 0.4f;//set this in constants for different enemies

	public bool m_EnemyInMotion;
	public bool m_isIdle;
	public bool m_TimerIsCounting; //bool for Timer for changing tracks (primary timer)
	public bool m_IsDead = false;
	public bool m_IsABoss = false;



	public int m_EnemyGoingLeft = 1; //1 == left, -1 == right

	public Rigidbody m_RigidBody;
	public Vector3 m_InitialXY;
    public Vector3 zOffSet_;
    public Vector3 enemyPos;

	//two variables for changing tracks
	//public float changeTrackCountdown = 2.0f; //primary timer, when this reaches 0 the enemy can change tracks, and the secondary timer start counting down and m_TimerIsCounting is set to false
	//public float secondaryTrackTimer = 2.0f; //secondary timer, when this reaches 0 then the primary timer starts counting down and it's bool is set to true

    private Rigidbody bullyRigidbody_;
    private EnemyBaseClass bullyBaseClass_;
    private NavMeshAgent enemyWalker_;

	#endregion

	#region Enemy Movement
	public virtual void EnemyMove(GameObject bully)
	{
		if (bully.GetComponent<EnemyBaseClass>().m_EnemyInMotion)
		{
			bully.GetComponent<Rigidbody>().velocity = new Vector3(bully.GetComponent<EnemyBaseClass>().m_VelocityX, 0,0);//set the enemy's velocity
		}

	}

	public virtual void EnemyIdle(GameObject bully, Vector3 enemyPos)
	{
        bullyBaseClass_ = bully.GetComponent<EnemyBaseClass>();

        //float differenceThenNow = bullyBaseClass_.m_InitialXY.x - enemyPos.x;
		float pointB = m_MaxDist;
        float pointA = bullyBaseClass_.m_InitialXY.x + 1;
		//moving right and has passed pointA
        if (enemyPos.x >= pointA && bullyBaseClass_.m_EnemyGoingLeft == -1)
		{
			enemyPos.x = pointA;
            bullyBaseClass_.TurnAround(bully);
		}
		//moving left and has passed point B
        if (enemyPos.x <= pointB && bullyBaseClass_.m_EnemyGoingLeft == 1)
		{
            bullyBaseClass_.TurnAround(bully);
		}
		DetectPlayer(m_Player.transform.position, bully);
	}

	public virtual void TurnAround(GameObject bully)
	{
        bullyBaseClass_ = bully.GetComponent<EnemyBaseClass>();

        bullyBaseClass_.m_VelocityX *= -1; //turn the enemy around
        bullyBaseClass_.m_EnemyGoingLeft *= -1; //tell the enemy it has turned around

		//Brandon's code to fip the animation,,, flips the x 
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public virtual void EnemyStopMotion(GameObject bully)
	{
        bullyBaseClass_ = bully.GetComponent<EnemyBaseClass>();

        bullyBaseClass_.m_RigidBody.velocity = new Vector3(0, 0, 0); //freeze position
        bullyBaseClass_.m_EnemyInMotion = false; //set bool that prevents movement in the update
	}
   
    /*
	public virtual void ChasePlayer(Vector3 playerPos, Vector3 enemyPos, GameObject bully)
	{
        bullyBaseClass_ = bully.GetComponent<EnemyBaseClass>();

		for (int i = 0; i < Physics2D.AllLayers; ++i )
		{
			string LayerName = LayerMask.LayerToName(i);
			if(LayerName == "ActiveBully")
			{
				bully.layer = i;
			}
		}
		float curEnemyXPOS = enemyPos.x;
		float lineOfSight;
        if (bullyBaseClass_.m_IsABoss)
		{
			lineOfSight = bully.GetComponent<BossBaseClass>().m_AttackDist;
		}
		else
		{
			lineOfSight = bully.GetComponent<BullyScript>().m_AttackDist;
		}


		if (playerPos.x > curEnemyXPOS)//if the player is on the right
		{
			//if the player is less to the right than the currentposition of the enemy's line of sight
			if (playerPos.x < enemyPos.x + lineOfSight)
			{
				EnemyStopMotion(bully);
			}
			else
			{
				EnemyMove(bully);
			}
		}
		else //the player is on the left
		{
			//if the player is less to the left than the enemy's pos - it's line of sight
			if (playerPos.x > enemyPos.x - lineOfSight)
			{
				//EnemyStopMotion(bully);
			}
			else
			{
				EnemyMove(bully);
			}
		}

		//If the enemy is to the left of the player and if the enemy is moving to the right
        if (enemyPos.x < playerPos.x && bullyBaseClass_.m_VelocityX < 0)
		{
            bullyBaseClass_.TurnAround(bully); //correct movement direction
		}
		//If the enemy is to the right, and moving left
        if (enemyPos.x > playerPos.x && bullyBaseClass_.m_VelocityX > 0)
		{
            bullyBaseClass_.TurnAround(bully); //correct movement direction
		}
	}*/

	public virtual void DetectPlayer(Vector3 playerPos, GameObject bully)
	{
        bullyBaseClass_ = bully.GetComponent<EnemyBaseClass>();

        Vector2 differenceInDistance = new Vector3(bully.transform.position.x, bully.transform.position.y, bully.transform.position.z) - playerPos; //get the difference between the two entities
        float forwardDetectionX = bully.transform.position.x - bullyBaseClass_.m_DetectionDist; //x position player has to reach or pass for the enemy to wake up

       // if (bullyBaseClass_.m_CurRow == bullyBaseClass_.m_PlayerCurRow)
		//{
			//e - p = differenceInDistance
			//difference in distance == a line between the two p_____e
			//if this "line" is shorter than the bully's forwardDetection aka "Line Of Sight" (while the player is to the left) -|____e____|+
			//or if the "line" is shorter than (player is inside the line) the bully's ForwardDetectionX (while the player is to the right)
			if (differenceInDistance.x <= forwardDetectionX || differenceInDistance.x <= -forwardDetectionX)
			{
                bullyBaseClass_.m_isIdle = false;//then the enemy is no longer Idle	
			}
		//}
		//else
		//{
			if (differenceInDistance.x <= forwardDetectionX)//if the player is within the detection "range" of a bully
			{
                bullyBaseClass_.m_isIdle = false;//then the enemy is no longer Idle	
			}
		//}
	}
	#endregion

	#region Enemy Attacks
	public virtual void EnemyAttack(GameObject bully)
	{
        bullyBaseClass_ = bully.GetComponent<EnemyBaseClass>();

		int attackSelector = Random.Range(0, 100);
        bullyBaseClass_.EnemyStopMotion(bully);
      //  if (bullyBaseClass_.m_CurRow == bullyBaseClass_.m_PlayerCurRow)
		{
			//			m_EnemyInMotion = false; //prevent continued motion of the bully
			if (attackSelector <= m_AttackPunchOdds) //If attack selector is less than the odds of punching
			{
                bullyBaseClass_.EnemyAttackPunch(bully); //PAWNCH
			}
			else if (attackSelector <= m_AttackKickOdds)//not less than Punch odds, so check if less than kick odds
			{
                bullyBaseClass_.EnemyAttackKick(bully); //Kick
			}
			else if (attackSelector >= m_AttackKickOdds)//must be greater than kick odds by now so Unique Attack is called
			{
                bullyBaseClass_.EnemyAttackUnique(bully); //
			}
		}
		if (attackSelector >= m_AttackKickOdds)//must be greater than kick odds by now so Unique Attack is called
		{
            bullyBaseClass_.EnemyAttackUnique(bully); //
		}
	}

	public virtual void ResetEnemyAttackTimer(float enemyAttackTimer)
	{
		this.GetComponent<EnemyBaseClass>().m_AttackTimer = enemyAttackTimer; //reassign the attack timer to the enemy's default
		//		this.m_EnemyInMotion = true; //tell the enemy it can move again
	}

	public virtual void EnemyAttackPunch(GameObject bully) //This function is overwritten in the BullyScript
	{
		bully.GetComponent<EnemyBaseClass>().m_BullyAnimator.SetBool("IsPunch", true);
		//play punch animation
		//set delay for the attack countdown timer to resume only when the animation is done
	}

	public virtual void EnemyAttackKick(GameObject bully)//This function is overwritten in the BullyScript
	{
		bully.GetComponent<EnemyBaseClass>().m_BullyAnimator.SetBool("IsKick", true);
		//play kick animation
		//set delay for the attack countdown timer to resume only when the animation is done	
	}

	public virtual void EnemyAttackUnique(GameObject bully)//This function is overwritten in the BullyScript
	{
	}
	#endregion

	#region Enemy Combat
	//Straightforward
	public virtual void EnemyTakeDamage(int damageDealt)
	{
		this.GetComponent<BullyScript>().m_BullyAnimator.SetBool("IsHit", true);
		this.GetComponent<BullyScript>().m_HP -= damageDealt;
		if (m_HP <= 0)
		{
			KillEnemy(this.GetComponent<BullyScript>().gameObject);
		}
	}

	public virtual void KillEnemy(GameObject enemy)
	{
		enemy.GetComponent<BullyScript>().m_BullyAnimator.SetBool("IsDead", true);//play enemy death animation
		GameObject.Destroy(transform.root.gameObject);
	}
	#endregion

	#region Creation
	public virtual void InitEnemy(Vector3 spawnPos, Vector3 zOffSet_, GameObject newBully)
	{
        bullyBaseClass_ = newBully.GetComponent<EnemyBaseClass>();

       // bullyBaseClass_.changeTrackCountdown = m_ChangeTrackTimer;

        bullyBaseClass_.m_Player = GameObject.FindGameObjectWithTag("Player");
        bullyBaseClass_.m_PlayerPos = new Vector2(m_Player.GetComponent<Rigidbody>().position.x, m_Player.GetComponent<Rigidbody>().position.y);

        bullyBaseClass_.m_InitialXY = spawnPos;
        bullyBaseClass_.m_isIdle = true;
        bullyBaseClass_.m_BullyAnimator = newBully.GetComponent<Animator>();

		//		
	}
	#endregion
	void Start()
	{
        enemyWalker_ = this.gameObject.GetComponent<NavMeshAgent>();
		m_EnemyController = GameObject.FindGameObjectWithTag("EnemyController");
        m_Player = GameObject.FindGameObjectWithTag("Player");
	}

	public virtual void GetPlayerInfo(GameObject thisEnemy)
	{
		m_PlayerPos = m_Player.GetComponent<Rigidbody>().transform.position;
	}

	public virtual void EnemyUpdate(GameObject bully)
	{
        bullyRigidbody_ = bully.GetComponent<Rigidbody>();
        bullyBaseClass_ = bully.GetComponent<EnemyBaseClass>();

		//Debug.Log(bully.name);
		if (bully.name != "FattestBully" && bully.name != "KingBully" && bully.name != "RefereeBully" && bully.name != "QueenBully")
		{
            Vector3 enemyPos = new Vector3(bullyRigidbody_.position.x, bullyRigidbody_.position.y, bullyRigidbody_.position.z);
			bully.GetComponent<BullyScript>().m_UniqueAttackHolder.GetComponent<UniqueAttackScript>().UpdateUATKs(bully); //Update Enemy Projectiles on screen

            m_PlayerPos = new Vector3(m_Player.GetComponent<Rigidbody>().position.x, m_Player.GetComponent<Rigidbody>().position.y, m_Player.GetComponent<Rigidbody>().position.z);

			
			GetPlayerInfo(bully);
          
			{
               // bullyBaseClass_.ChasePlayer(m_PlayerPos, enemyPos, bully);

				//Animation
                if (bullyBaseClass_.m_AnimationLength > 0) //if animating, subtract Delta.Time
				{
                    bullyBaseClass_.m_AnimationLength -= Time.deltaTime;
				}
                if (bullyBaseClass_.m_AttackTimer > 0 && bullyBaseClass_.m_AnimationLength <= 0) //if the enemy isn't cooled down, and is not animating
				{
                    bullyBaseClass_.m_AttackTimer -= Time.deltaTime;
				}
                if (bullyBaseClass_.m_AttackTimer <= 0 && bullyBaseClass_.m_BullyAnimator.GetBool("IsPunch") == false && bullyBaseClass_.m_BullyAnimator.GetBool("IsKick") == false && bullyBaseClass_.m_BullyAnimator.GetBool("IsUnique") == false) //If the enemy is cooled down, and is not animating
				{
                    bullyBaseClass_.m_AttackTimer = 0;
					EnemyAttack(bully);
				}

                if (bullyBaseClass_.m_AnimationLength <= 0)
				{
                    bullyBaseClass_.m_AnimationLength = 0;
                    bullyBaseClass_.m_BullyAnimator.SetBool("IsPunch", false);
                    bullyBaseClass_.m_BullyAnimator.SetBool("IsKick", false);
                    bullyBaseClass_.m_BullyAnimator.SetBool("IsUnique", false);
                    bullyBaseClass_.m_EnemyInMotion = true;
				}
			}
		}
		else
		{
		}

	}


	// Update is called once per frame
	void Update()
	{
        enemyWalker_.SetDestination(m_Player.transform.position);
		m_Bullies = m_EnemyController.GetComponent<EnemyControllerScript>().m_Bullies;
		for (int i = 0; i < m_Bullies.Count; ++i)
		{
			EnemyUpdate(m_Bullies[i]);

			#region Keep Bullies away from the same rows if possible
			if (m_Bullies.Count >= 2)
			{
				if (!m_Bullies[i].GetComponent<EnemyBaseClass>().m_IsABoss)
				{
					for (int j = 1; j < m_Bullies.Count; ++j)
					{
//						Debug.Log(m_Bullies[i].name + " row " + m_Bullies[i].GetComponent<EnemyBaseClass>().m_CurRow + " vs " + m_Bullies[j].name + " row " + m_Bullies[j].GetComponent<EnemyBaseClass>().m_CurRow);
						if (!m_Bullies[j].GetComponent<EnemyBaseClass>().m_IsABoss)
						{
							//if (m_Bullies[i].GetComponent<EnemyBaseClass>().collider == m_Bullies[j].GetComponent<EnemyBaseClass>().m_CurRow)
							//{
							//	if (m_Bullies[j].GetComponent<EnemyBaseClass>().m_CurRow != 2)
								//{
								//	m_Bullies[j].GetComponent<EnemyBaseClass>().m_CurRow++;
								//}
								//else if (m_Bullies[j].GetComponent<EnemyBaseClass>().m_CurRow != 0)
								//{
								//	m_Bullies[j].GetComponent<EnemyBaseClass>().m_CurRow--;
								//}
							//}
						}
					}
				}
			}
			#endregion
			if (m_Bullies[i].GetComponent<EnemyBaseClass>().m_HP <= 0)
			{
				string BullyJustKilled = m_Bullies[i].name;			
				Destroy(m_Bullies[i]);
				m_Bullies.Remove(m_Bullies[i].gameObject);
				if (BullyJustKilled == "KingBully")
				{
                   // m_EnemyController.GetComponent<SpawnEnemies>().SpawnBoss(enemyPos, "QueenBully");
				}
			}
		}
		
	}

	#region Collision
	void OnTriggerEnter(Collider collision)
	{
		if (collision.tag == "PlayerProjectile")
		{		
			this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0,0);
			this.GetComponent<Rigidbody>().isKinematic = true;
			this.m_HP -= collision.gameObject.GetComponent<Projectile>().m_Damage;
		}
		this.GetComponent<Rigidbody>().AddForce(new Vector3(m_ReactForce, 0.0f));//Brandon's Wiggle
	}
	#endregion

}
