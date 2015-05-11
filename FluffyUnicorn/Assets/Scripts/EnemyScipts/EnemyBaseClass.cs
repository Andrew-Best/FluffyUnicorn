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
	private Vector2 m_PlayerPos;// = new Vector2(player.GetComponent<Rigidbody2D>().position.x, player.GetComponent<Rigidbody2D>().position.y);

	public GameObject PepperSpray;

	public Animator m_BullyWalk;
	public Collider2D[] m_Tracks;
	public GameObject[] m_TargetPoints;

	public int m_VelocityX;

	public int m_PlayerCurRow;
	public int m_CurRow;
	public bool m_AbleToChangeTrack = false;
	public float m_ChangeTrackTimer;

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

	public bool m_EnemyInMotion;
	public bool m_isIdle;
	public bool m_TimerIsCounting; //bool for Timer for changing tracks (primary timer)
    public bool m_IsDead = false;
	public bool m_IsABoss = false;

	public int m_EnemyGoingLeft = 1; //1 == left, -1 == right

	public Rigidbody2D m_RigidBody;
	public Vector2 m_InitialXY;

	//two variables for changing tracks
	public float changeTrackCountdown = 2.0f; //primary timer, when this reaches 0 the enemy can change tracks, and the secondary timer start counting down and m_TimerIsCounting is set to false
	public float secondaryTrackTimer = 2.0f; //secondary timer, when this reaches 0 then the primary timer starts counting down and it's bool is set to true

	#endregion

	#region Enemy Movement
	public virtual void EnemyMove(GameObject bully)
	{
		if (bully.GetComponent<EnemyBaseClass>().m_EnemyInMotion)
		{
			bully.GetComponent<Rigidbody2D>().velocity = new Vector2(bully.GetComponent<EnemyBaseClass>().m_VelocityX, 0);//set the enemy's velocity
		}
		
	}

	public virtual void EnemyIdle(GameObject bully, Vector2 enemyPos)
	{

		float differenceThenNow = bully.GetComponent<EnemyBaseClass>().m_InitialXY.x - enemyPos.x;
		float pointB = m_MaxDist;
		float pointA = bully.GetComponent<EnemyBaseClass>().m_InitialXY.x + 1;
		//moving right and has passed pointA
		if (enemyPos.x >= pointA && bully.GetComponent<EnemyBaseClass>().m_EnemyGoingLeft == -1)
			{
				enemyPos.x = pointA;
				bully.GetComponent<EnemyBaseClass>().TurnAround(bully);
			}
			//moving left and has passed point B
		if (enemyPos.x <= pointB && bully.GetComponent<EnemyBaseClass>().m_EnemyGoingLeft == 1)
			{
				bully.GetComponent<EnemyBaseClass>().TurnAround(bully);
			}
			DetectPlayer(m_Player.transform.position, enemyPos);
	}

	public virtual void TurnAround(GameObject bully)
	{
		bully.GetComponent<EnemyBaseClass>().m_VelocityX *= -1; //turn the enemy around
		bully.GetComponent<EnemyBaseClass>().m_EnemyGoingLeft *= -1; //tell the enemy it has turned around

		//Brandon's code to fip the animation,,, flips the x 
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public virtual void EnemyStopMotion(GameObject bully)
	{
		bully.GetComponent<EnemyBaseClass>().m_RigidBody.velocity = new Vector2(0, 0); //freeze position
		bully.GetComponent<EnemyBaseClass>().m_EnemyInMotion = false; //set bool that prevents movement in the update
	}

	public virtual void ChangeTrack(GameObject bully)
	{
		if (!bully.GetComponent<EnemyBaseClass>().m_isIdle)
		{
			if (bully.GetComponent<EnemyBaseClass>().m_CurRow < bully.GetComponent<EnemyBaseClass>().m_PlayerCurRow)
			{
				bully.GetComponent<EnemyBaseClass>().m_CurRow++;
			}
			else if (bully.GetComponent<EnemyBaseClass>().m_CurRow > bully.GetComponent<EnemyBaseClass>().m_PlayerCurRow)
			{
				bully.GetComponent<EnemyBaseClass>().m_CurRow--;
			}
			else //function should not have been called in the first place
			{
				Debug.Log("Why was this even called?");
			}
			bully.GetComponent<EnemyBaseClass>().GetComponent<Rigidbody2D>().transform.position = new Vector2(bully.GetComponent<EnemyBaseClass>().GetComponent<Rigidbody2D>().transform.position.x, bully.GetComponent<EnemyBaseClass>().m_TargetPoints[m_CurRow].transform.position.y);

		}
		
	}

	public virtual void ChasePlayer(Vector2 playerPos, Vector2 enemyPos, GameObject bully)
	{
		float curEnemyXPOS = enemyPos.x;
		float lineOfSight;
		if(bully.GetComponent<EnemyBaseClass>().m_IsABoss)
		{
			lineOfSight = bully.GetComponent<BossBaseClass>().m_AttackDist;
		}
		else
		{
			lineOfSight = bully.GetComponent<BullyScript>().m_AttackDist;
		}
		

		#region track change counter
		if (bully.GetComponent<EnemyBaseClass>().changeTrackCountdown <= 0)//If Timer is at 0
		{
			bully.GetComponent<EnemyBaseClass>().changeTrackCountdown = 0; //set the timer to 0
			bully.GetComponent<EnemyBaseClass>().m_TimerIsCounting = false;//prevent the timer from continueing to count down
			if (bully.GetComponent<EnemyBaseClass>().m_PlayerCurRow != bully.GetComponent<EnemyBaseClass>().m_CurRow) //If not on the same track
			{
				bully.GetComponent<EnemyBaseClass>().ChangeTrack(bully);//will not change track if Idle
				bully.GetComponent<EnemyBaseClass>().secondaryTrackTimer = Constants.TRACK_COUNTDOWN_DEFAULT; // The secondary timer is assigned its value
			}
		}
		#endregion

		if(playerPos.x > curEnemyXPOS)//if the player is on the right
		{
			//if the player is less to the right than the currentposition of the enemy's line of sight
			if(playerPos.x < enemyPos.x + lineOfSight)
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
			if(playerPos.x > enemyPos.x - lineOfSight)
			{				
				//EnemyStopMotion(bully);
			}
			else
			{
				EnemyMove(bully);
			}
		}

		//If the enemy is to the left of the player and if the enemy is moving to the right
		if (enemyPos.x < playerPos.x && bully.GetComponent<EnemyBaseClass>().m_VelocityX < 0)
		{
			bully.GetComponent<EnemyBaseClass>().TurnAround(bully); //correct movement direction
		}
		//If the enemy is to the right, and moving left
		if (enemyPos.x > playerPos.x && bully.GetComponent<EnemyBaseClass>().m_VelocityX > 0)
		{
			bully.GetComponent<EnemyBaseClass>().TurnAround(bully); //correct movement direction
		}
	}

	public virtual void DetectPlayer(Vector2 playerPos, Vector2 enemyPos)
	{
		Vector2 differenceInDistance = enemyPos - playerPos; //get the difference between the two entities
		float forwardDetectionX = enemyPos.x - this.GetComponent<EnemyBaseClass>().m_DetectionDist; //x position player has to reach or pass for the enemy to wake up

		if (this.GetComponent<EnemyBaseClass>().m_CurRow == this.GetComponent<EnemyBaseClass>().m_PlayerCurRow)
		{
			/*the difference between the player and bully is less than the detection dist on the right, OR if it is greater than the detection on the left*/
			if(differenceInDistance.x <= forwardDetectionX || differenceInDistance.x >= -forwardDetectionX)
			{
				this.GetComponent<EnemyBaseClass>().m_isIdle = false;//then the enemy is no longer Idle	
			}
		}
		else
		{
			if (differenceInDistance.x <= forwardDetectionX)//if the player is within the detection "range" of a bully
			{
				this.GetComponent<EnemyBaseClass>().m_isIdle = false;//then the enemy is no longer Idle	
			}
		}

	}
	#endregion

	#region Enemy Attacks
	public virtual void EnemyAttack(GameObject bully)
	{
		int attackSelector = Random.Range(0, 100);
		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);
		if (bully.GetComponent<EnemyBaseClass>().m_CurRow == bully.GetComponent<EnemyBaseClass>().m_PlayerCurRow)
		{			
//			m_EnemyInMotion = false; //prevent continued motion of the bully
			if (attackSelector <= m_AttackPunchOdds) //If attack selector is less than the odds of punching
			{
				bully.GetComponent<EnemyBaseClass>().EnemyAttackPunch(bully); //PAWNCH
			}
			else if (attackSelector <= m_AttackKickOdds)//not less than Punch odds, so check if less than kick odds
			{
				bully.GetComponent<EnemyBaseClass>().EnemyAttackKick(bully); //Kick
			}
			else if (attackSelector >= m_AttackKickOdds)//must be greater than kick odds by now so Unique Attack is called
			{
				bully.GetComponent<EnemyBaseClass>().EnemyAttackUnique(bully); //
			}
		}
		if (attackSelector >= m_AttackKickOdds)//must be greater than kick odds by now so Unique Attack is called
		{
			bully.GetComponent<EnemyBaseClass>().EnemyAttackUnique(bully); //
		}
	}

	public virtual void ResetEnemyAttackTimer(float enemyAttackTimer)
	{
		this.GetComponent<EnemyBaseClass>().m_AttackTimer = enemyAttackTimer; //reassign the attack timer to the enemy's default
//		this.m_EnemyInMotion = true; //tell the enemy it can move again
	}

	public virtual void EnemyAttackPunch(GameObject bully) //This function is overwritten in the BullyScript
	{
		bully.GetComponent<EnemyBaseClass>().m_BullyWalk.SetBool("IsPunch", true);
		//play punch animation
		//set delay for the attack countdown timer to resume only when the animation is done
	}

	public virtual void EnemyAttackKick(GameObject bully)//This function is overwritten in the BullyScript
	{
		bully.GetComponent<EnemyBaseClass>().m_BullyWalk.SetBool("IsKick", true);
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
		this.GetComponent<BullyScript>().m_BullyWalk.SetBool("IsHit", true);
		this.GetComponent<BullyScript>().m_HP -= damageDealt;
		if (m_HP <= 0)
		{
			KillEnemy(this.GetComponent<BullyScript>().gameObject);
		}
	}

	public virtual void KillEnemy(GameObject enemy)
	{
		enemy.GetComponent<BullyScript>().m_BullyWalk.SetBool("IsDead", true);//play enemy death animation
		GameObject.Destroy(transform.root.gameObject);
	}
	#endregion

	#region Creation
	public virtual void InitEnemy(Vector2 spawnPos, int row, GameObject newBully)
	{
		newBully.GetComponent<EnemyBaseClass>().changeTrackCountdown = m_ChangeTrackTimer;

		newBully.GetComponent<EnemyBaseClass>().m_Player = GameObject.FindGameObjectWithTag("Player");
		newBully.GetComponent<EnemyBaseClass>().m_PlayerPos = new Vector2(m_Player.GetComponent<Rigidbody2D>().position.x, m_Player.GetComponent<Rigidbody2D>().position.y);
		
		newBully.GetComponent<EnemyBaseClass>().m_InitialXY = spawnPos;

//		m_Bullies = m_EnemyController.GetComponent<EnemyControllerScript>().m_Bullies;
	}
	#endregion
	void Start()
	{
		m_EnemyController = GameObject.FindGameObjectWithTag("EnemyController");
	}

	public virtual void GetPlayerInfo(GameObject thisEnemy)
	{
		if (thisEnemy.GetComponent<BullyScript>().m_Player.GetComponent<PlayerController>().m_onFrontTrack)
		{
			thisEnemy.GetComponent<BullyScript>().m_PlayerCurRow = 2;
		}
		else if (thisEnemy.GetComponent<BullyScript>().m_Player.GetComponent<PlayerController>().m_onMiddleTrack)
		{
			thisEnemy.GetComponent<BullyScript>().m_PlayerCurRow = 1;
		}
		else if (thisEnemy.GetComponent<BullyScript>().m_Player.GetComponent<PlayerController>().m_onLastTrack)
		{
			thisEnemy.GetComponent<BullyScript>().m_PlayerCurRow = 0;
		}

		m_PlayerPos = m_Player.GetComponent<Rigidbody2D>().transform.position;
	}

	public virtual void EnemyUpdate(List<GameObject> bully)
	{
		bully = m_EnemyController.GetComponent<EnemyControllerScript>().m_Bullies;
		
		for(int i = 0; i < bully.Count; ++i)
		{
			Debug.Log(bully[i].name);
			if(bully[i].name != "FattestBully" && bully[i].name != "KingBully")
			{
				Vector2 enemyPos = new Vector2(bully[i].GetComponent<Rigidbody2D>().position.x, bully[i].GetComponent<Rigidbody2D>().position.y);
				bully[i].GetComponent<BullyScript>().m_UniqueAttackHolder.GetComponent<UniqueAttackScript>().UpdateUATKs(bully[i]); //Update Enemy Projectiles on screen

				m_Player = GameObject.FindGameObjectWithTag("Player");

				m_PlayerPos = new Vector2(m_Player.GetComponent<Rigidbody2D>().position.x, m_Player.GetComponent<Rigidbody2D>().position.y);

				#region Keep Bullies away from the same rows if possible
				/*			if (m_Bullies.Count >= 2)
			{
				for (int j = 0; j < m_Bullies.Count; ++j)
				{
					if (m_Bullies[j].GetComponent<EnemyBaseClass>().m_CurRow > m_Bullies[j + 1].GetComponent<EnemyBaseClass>().m_CurRow)
					{
						m_Bullies[j].GetComponent<EnemyBaseClass>().EnemyAttackUnique(bully[i]); //> m_Bullies[i+1].GetComponent<EnemyBaseClass>().m_CurRow
						m_Bullies[j + 1].GetComponent<EnemyBaseClass>().EnemyAttackUnique(bully[i]);
					}
					if (m_Bullies[j].GetComponent<EnemyBaseClass>().m_CurRow == m_Bullies[j + 1].GetComponent<EnemyBaseClass>().m_CurRow)
					{
						if (m_Bullies[j + 1].GetComponent<EnemyBaseClass>().m_CurRow == 0)
						{
							m_Bullies[j + 1].GetComponent<EnemyBaseClass>().m_CurRow++;
						}
						if (m_Bullies[j + 1].GetComponent<EnemyBaseClass>().m_CurRow == 2)
						{
							m_Bullies[j + 1].GetComponent<EnemyBaseClass>().m_CurRow--;
						}
					}
				}
			}*/
				#endregion

				//Detect Player Track
				GetPlayerInfo(bully[i]);

				//Conditions for changing tracks
				if (!bully[i].GetComponent<EnemyBaseClass>().m_TimerIsCounting) //if the primary timer is not able to count down (disabled)
				{
					bully[i].GetComponent<EnemyBaseClass>().changeTrackCountdown = Constants.TRACK_COUNTDOWN_DEFAULT; //set the primary timer to its default value
					bully[i].GetComponent<EnemyBaseClass>().secondaryTrackTimer -= Time.deltaTime; // decrement the secondary timer
				}
				if (secondaryTrackTimer <= 0) //once the secondary timer reaches 0
				{
					bully[i].GetComponent<EnemyBaseClass>().m_TimerIsCounting = true; //enable the primary timer
					bully[i].GetComponent<EnemyBaseClass>().secondaryTrackTimer = Constants.TRACK_COUNTDOWN_DEFAULT; //set the secondary timer to it's default value
				}
				if (bully[i].GetComponent<EnemyBaseClass>().m_TimerIsCounting)
				{
					bully[i].GetComponent<EnemyBaseClass>().changeTrackCountdown -= Time.deltaTime;
				}

//				bully[i].GetComponent<BullyScript>().GetComponent<Rigidbody2D>().transform.position = new Vector2(bully[i].GetComponent<BullyScript>().GetComponent<Rigidbody2D>().transform.position.x, bully[i].GetComponent<BullyScript>().m_TargetPoints[m_CurRow].transform.position.y);

				if (bully[i].GetComponent<EnemyBaseClass>().m_EnemyInMotion)
				{
					bully[i].GetComponent<EnemyBaseClass>().EnemyMove(bully[i]);
				}

				if (bully[i].GetComponent<EnemyBaseClass>().m_isIdle)
				{
					bully[i].GetComponent<EnemyBaseClass>().EnemyIdle(bully[i], enemyPos);
				}
				else // enemy is not idle, therefore player is nearby
				{
					bully[i].GetComponent<EnemyBaseClass>().ChasePlayer(m_PlayerPos, enemyPos, bully[i]);

					//Animation
					if (bully[i].GetComponent<EnemyBaseClass>().m_AnimationLength > 0) //if animating, subtract Delta.Time
					{
						bully[i].GetComponent<EnemyBaseClass>().m_AnimationLength -= Time.deltaTime;
					}
					if (bully[i].GetComponent<EnemyBaseClass>().m_AttackTimer > 0 && bully[i].GetComponent<EnemyBaseClass>().m_AnimationLength <= 0) //if the enemy isn't cooled down, and is not animating
					{
						bully[i].GetComponent<EnemyBaseClass>().m_AttackTimer -= Time.deltaTime;
					}
					if (bully[i].GetComponent<EnemyBaseClass>().m_AttackTimer <= 0 && bully[i].GetComponent<EnemyBaseClass>().m_BullyWalk.GetBool("IsPunch") == false && bully[i].GetComponent<EnemyBaseClass>().m_BullyWalk.GetBool("IsKick") == false && bully[i].GetComponent<EnemyBaseClass>().m_BullyWalk.GetBool("IsUnique") == false) //If the enemy is cooled down, and is not animating
					{
						bully[i].GetComponent<EnemyBaseClass>().m_AttackTimer = 0;
						EnemyAttack(bully[i]);
					}

					if (bully[i].GetComponent<EnemyBaseClass>().m_AnimationLength <= 0)
					{
						bully[i].GetComponent<EnemyBaseClass>().m_AnimationLength = 0;
						bully[i].GetComponent<EnemyBaseClass>().m_BullyWalk.SetBool("IsPunch", false);
						bully[i].GetComponent<EnemyBaseClass>().m_BullyWalk.SetBool("IsKick", false);
						bully[i].GetComponent<EnemyBaseClass>().m_BullyWalk.SetBool("IsUnique", false);
						bully[i].GetComponent<EnemyBaseClass>().m_EnemyInMotion = true;
					}
				}
			}
			else
			{
			}
			
		}
		
	}


	// Update is called once per frame
	void Update()
	{
			EnemyUpdate(m_Bullies);
	}

}
