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
	//public Collider2D[] m_Tracks;
	public GameObject[] m_TargetPoints;

	public int m_VelocityX;

	//public int m_PlayerCurRow;
	//public int m_CurRow;
	//public bool m_AbleToChangeTrack = false;
	//public float m_ChangeTrackTimer;

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

	public bool Row0Occupied = false;
	public bool Row1Occupied = false;
	public bool Row2Occupied = false;

	public int m_EnemyGoingLeft = 1; //1 == left, -1 == right

	public Rigidbody m_RigidBody;
	public Vector2 m_InitialXY;

	//two variables for changing tracks
	//public float changeTrackCountdown = 2.0f; //primary timer, when this reaches 0 the enemy can change tracks, and the secondary timer start counting down and m_TimerIsCounting is set to false
	//public float secondaryTrackTimer = 2.0f; //secondary timer, when this reaches 0 then the primary timer starts counting down and it's bool is set to true

    private Rigidbody bullyRigidbody_;
    private EnemyBaseClass bullyBaseClass_;

	#endregion

	#region Enemy Movement
	public virtual void EnemyMove(GameObject bully)
	{
		if (bully.GetComponent<EnemyBaseClass>().m_EnemyInMotion)
		{
			bully.GetComponent<Rigidbody>().velocity = new Vector2(bully.GetComponent<EnemyBaseClass>().m_VelocityX, 0);//set the enemy's velocity
		}

	}

	public virtual void EnemyIdle(GameObject bully, Vector2 enemyPos)
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

        bullyBaseClass_.m_RigidBody.velocity = new Vector2(0, 0); //freeze position
        bullyBaseClass_.m_EnemyInMotion = false; //set bool that prevents movement in the update
	}
    /*
	public void CheckOccupiedTracks()
	{
		for (int i = 0; i < m_Bullies.Count; ++i)
		{
			if (m_Bullies[i].GetComponent<EnemyBaseClass>().m_CurRow == 0)
			{
				Row0Occupied = true;
			}
			else { Row0Occupied = false; }
			if (m_Bullies[i].GetComponent<EnemyBaseClass>().m_CurRow == 1)
			{
				Row1Occupied = true;
			}
			else { Row1Occupied = false; }
			if (m_Bullies[i].GetComponent<EnemyBaseClass>().m_CurRow == 2)
			{
				Row2Occupied = true;
			}
			else { Row2Occupied = false; }
		}
	}*/
    /*
	public virtual void ChangeTrack(GameObject bully)//Bully will attempt to change tracks to match the player, but will fail if another bully is on that track
	{
        bullyBaseClass_ = bully.GetComponent<EnemyBaseClass>();

		CheckOccupiedTracks();

		//Moving UP tracks (DOWN on the screen)
        if (bullyBaseClass_.m_CurRow < bullyBaseClass_.m_PlayerCurRow)//If Bully's track is lower in the index than the player's
		{
           // if (bullyBaseClass_.m_PlayerCurRow++ == 1)//if enemy is on track 0 moving to track 1
			{
				if (!Row1Occupied)//if row one does not have a bully on it
				{
                    bullyBaseClass_.m_CurRow++;//bully changes rows and occupies the new track
					Row1Occupied = true;
					Row0Occupied = false;
				}
			}
            else if (bullyBaseClass_.m_PlayerCurRow++ == 2)//if enemy is on track 1 moving to 2
			{
				if (!Row2Occupied)//if track does not have a bully on it
				{
					bullyBaseClass_.m_CurRow++;//bully changes rows and occupies the new track
					Row2Occupied = true;
					Row1Occupied = false;
				}
			}
		}

        if (bullyBaseClass_.m_CurRow > bullyBaseClass_.m_PlayerCurRow)//If Bully's track is higher in the index than the player's
		{
            if (bullyBaseClass_.m_PlayerCurRow-- == 0)//if enemy is on track 1 moving to track 0
			{
				if (!Row0Occupied)//if row zero does not have a bully on it
				{
                    bullyBaseClass_.m_CurRow--;//bully changes rows and occupies the new track
					Row0Occupied = true;
					Row1Occupied = false;
				}
			}
            else if (bullyBaseClass_.m_PlayerCurRow-- == 1)//if enemy is on track 2 moving to 1
			{
				if (!Row1Occupied)//if track does not have a bully on it
				{
                    bullyBaseClass_.m_CurRow--;//bully changes rows and occupies the new track
					Row1Occupied = true;
					Row2Occupied = false;
				}
			}
		}

		else //function should not have been called in the first place
		{
			Debug.Log("Why was this even called?");
		}

        float xPos = bullyBaseClass_.GetComponent<Rigidbody2D>().transform.position.x;

		float yPos = bully.GetComponent<BullyScript>().m_TargetPoints[m_CurRow].transform.position.y;

        bullyBaseClass_.GetComponent<Rigidbody2D>().transform.position = new Vector2(xPos, yPos);		
	} */

	public virtual void ChasePlayer(Vector2 playerPos, Vector2 enemyPos, GameObject bully)
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

		#region track change counter
		//if (bully.GetComponent<EnemyBaseClass>().changeTrackCountdown <= 0)//If Timer is at 0
		{
			//bully.GetComponent<EnemyBaseClass>().changeTrackCountdown = 0; //set the timer to 0
			bully.GetComponent<EnemyBaseClass>().m_TimerIsCounting = false;//prevent the timer from continueing to count down
			//if (bully.GetComponent<EnemyBaseClass>().m_PlayerCurRow != bully.GetComponent<EnemyBaseClass>().m_CurRow) //If not on the same track
			{
				//bully.GetComponent<EnemyBaseClass>().ChangeTrack(bully);//will not change track if Idle
				//bully.GetComponent<EnemyBaseClass>().secondaryTrackTimer = Constants.TRACK_COUNTDOWN_DEFAULT; // The secondary timer is assigned its value
			}
		}
		#endregion

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
	}

	public virtual void DetectPlayer(Vector2 playerPos, GameObject bully)
	{
        bullyBaseClass_ = bully.GetComponent<EnemyBaseClass>();

		Vector2 differenceInDistance = new Vector2(bully.transform.position.x, bully.transform.position.y) - playerPos; //get the difference between the two entities
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
	public virtual void InitEnemy(Vector2 spawnPos, int row, GameObject newBully)
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
		m_EnemyController = GameObject.FindGameObjectWithTag("EnemyController");
	}

	public virtual void GetPlayerInfo(GameObject thisEnemy)
	{
		/*if (thisEnemy.GetComponent<EnemyBaseClass>().m_Player.GetComponent<PlayerController>().m_onFrontTrack)
		{
			thisEnemy.GetComponent<EnemyBaseClass>().m_PlayerCurRow = 2;
		}
		else if (thisEnemy.GetComponent<EnemyBaseClass>().m_Player.GetComponent<PlayerController>().m_onMiddleTrack)
		{
			thisEnemy.GetComponent<EnemyBaseClass>().m_PlayerCurRow = 1;
		}
		else if (thisEnemy.GetComponent<EnemyBaseClass>().m_Player.GetComponent<PlayerController>().m_onLastTrack)
		{
			thisEnemy.GetComponent<EnemyBaseClass>().m_PlayerCurRow = 0;
		}*/

		m_PlayerPos = m_Player.GetComponent<Rigidbody>().transform.position;
	}

	public virtual void EnemyUpdate(GameObject bully)
	{
        bullyRigidbody_ = bully.GetComponent<Rigidbody>();
        bullyBaseClass_ = bully.GetComponent<EnemyBaseClass>();

		//Debug.Log(bully.name);
		if (bully.name != "FattestBully" && bully.name != "KingBully" && bully.name != "RefereeBully" && bully.name != "QueenBully")
		{
            Vector2 enemyPos = new Vector2(bullyRigidbody_.position.x, bullyRigidbody_.position.y);
			bully.GetComponent<BullyScript>().m_UniqueAttackHolder.GetComponent<UniqueAttackScript>().UpdateUATKs(bully); //Update Enemy Projectiles on screen

			m_Player = GameObject.FindGameObjectWithTag("Player");

			m_PlayerPos = new Vector2(m_Player.GetComponent<Rigidbody>().position.x, m_Player.GetComponent<Rigidbody>().position.y);

			//Detect Player Track
			GetPlayerInfo(bully);
            /*
			//Conditions for changing tracks
            if (!bullyBaseClass_.m_TimerIsCounting) //if the primary timer is not able to count down (disabled)
			{
                bullyBaseClass_.changeTrackCountdown = Constants.TRACK_COUNTDOWN_DEFAULT; //set the primary timer to its default value
                //bullyBaseClass_.secondaryTrackTimer -= Time.deltaTime; // decrement the secondary timer
			}
			if (secondaryTrackTimer <= 0) //once the secondary timer reaches 0
			{
                bullyBaseClass_.m_TimerIsCounting = true; //enable the primary timer
                //bullyBaseClass_.secondaryTrackTimer = Constants.TRACK_COUNTDOWN_DEFAULT; //set the secondary timer to it's default value
			}
            if (bullyBaseClass_.m_TimerIsCounting)
			{
               // bullyBaseClass_.changeTrackCountdown -= Time.deltaTime;
			}

			//bully.GetComponent<BullyScript>().GetComponent<Rigidbody2D>().transform.position = new Vector2(bully.GetComponent<BullyScript>().GetComponent<Rigidbody2D>().transform.position.x, bully.GetComponent<BullyScript>().m_TargetPoints[m_CurRow].transform.position.y);

            if (bullyBaseClass_.m_EnemyInMotion)
			{
                bullyBaseClass_.EnemyMove(bully);
			}

            if (bullyBaseClass_.m_isIdle)
			{
                bullyBaseClass_.EnemyIdle(bully, enemyPos);
			}
			//else // enemy is not idle, therefore player is nearby */
			{
                bullyBaseClass_.ChasePlayer(m_PlayerPos, enemyPos, bully);

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
					m_EnemyController.GetComponent<SpawnEnemies>().SpawnBoss(1, "QueenBully");
				}
			}
		}
		
	}

	#region Collision
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "PlayerProjectile")
		{		
			this.GetComponent<Rigidbody>().velocity = new Vector2(0, 0);
			this.GetComponent<Rigidbody>().isKinematic = true;
			this.m_HP -= collision.gameObject.GetComponent<Projectile>().m_Damage;
		}
		this.GetComponent<Rigidbody>().AddForce(new Vector2(m_ReactForce, 0.0f));//Brandon's Wiggle
	}
	#endregion

}
