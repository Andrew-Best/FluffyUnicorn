using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KingBully : BossBaseClass
{
	public GameObject enemySpawner;

	public float m_AttackUniqueAnimLength;
	private float AttackUniqueCurTime = 0;

	private float tempTimer;
	private float tempTimerResetVal = 240.0f;
    private List<GameObject> targetPoints_ = new List<GameObject>();

	public void Start()
	{

        InitEnemy(new Vector3(1, 0, 0), enemyPos, this.gameObject);

        for (int i = 0; i < 3; ++i)
        {
            targetPoints_.Add(GameObject.FindGameObjectWithTag("Targetpoint" + i));
        }   
	}

	public void WaterGun()//Done
	{
		//create BEAM attack
		this.GetComponent<EnemyBaseClass>().EnemyStopMotion(m_ThisBoss);
		this.GetComponent<BeamAttack>().Fire(m_ThisBoss, m_ThisBoss.GetComponent<EnemyBaseClass>().m_Player);

		this.m_AttackUniqueAnimLength = Constants.KB_WATER_GUN_LENGTH;
		this.AttackUniqueCurTime = 0;
		//water gun animation
	}

    public override void InitEnemy(Vector3 spawnPos, Vector3  enemyPos, GameObject newBully)
	{
        base.InitEnemy(spawnPos, enemyPos, newBully);
		this.m_ThisBoss = this.gameObject;
		this.m_Position = m_ThisBoss.GetComponent<Rigidbody2D>().position;
		this.m_HP = Constants.KING_BULLY_HP;
		this.m_Curstate = 0;
		this.m_BossName = "King Bully";
		this.m_CurFrame = 0;
		//this.m_CurRow = row;

		//this.m_TargetPoints[0] = GameObject.FindGameObjectWithTag("TargetLastTrack");

		//this.m_TargetPoints[1] = GameObject.FindGameObjectWithTag("TargetMidTrack");

		//this.m_TargetPoints[2] = GameObject.FindGameObjectWithTag("TargetFrontTrack");

		this.GetComponent<BeamAttack>().m_Ammo = Constants.KB_WATER_AMMO;
		//m_TotalFrames = this.GetComponent<Animator>().framesInAnim;
	}


	// Update is called once per frame
	public void KingUpdate()
	{
		m_Player = GameObject.FindGameObjectWithTag("Player");

        Vector3 playerPos = new Vector3(m_Player.GetComponent<Rigidbody>().position.x, m_Player.GetComponent<Rigidbody>().position.y, m_Player.GetComponent<Rigidbody>().position.z);

		//Detect Player Track
		GetPlayerInfo(this.gameObject);

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

        Vector3 enemyPos = new Vector3(this.m_RigidBody.position.x, this.m_RigidBody.position.y, this.m_RigidBody.position.z);

		if (this.m_EnemyInMotion)
		{
			this.EnemyMove(m_ThisBoss);
		}

		//float differenceThenNow = this.m_InitialXY.x - enemyPos.x;
		float pointB = m_MaxDist;
		float pointA = this.m_InitialXY.x + 1;

		if (this.m_isIdle)
		{
			//moving right and has passed pointA
			if (enemyPos.x >= pointA && this.m_EnemyGoingLeft == -1)
			{
				enemyPos.x = pointA;
				this.TurnAround(m_ThisBoss);
			}
			//moving left and has passed point B
			if (enemyPos.x <= pointB && this.m_EnemyGoingLeft == 1)
			{
				this.TurnAround(m_ThisBoss);
			}
			DetectPlayer(m_Player.transform.position, m_ThisBoss);
		}
		else // enemy is not idle, therefore player is nearby
		{
			//this.ChasePlayer(playerPos, enemyPos, m_ThisBoss);

			//Animation
			if (this.m_AnimationLength > 0) //if animating, subtract Delta.Time
			{
				this.m_AnimationLength -= Time.deltaTime;
			}
			if (this.m_AttackTimer > 0 && this.m_AnimationLength <= 0) //if the enemy isn't cooled down, and is not animating
			{
				this.m_AttackTimer -= Time.deltaTime;
			}
			if (this.m_AttackTimer <= 0 && this.m_BullyAnimator.GetBool("IsPunch") == false && this.m_BullyAnimator.GetBool("IsKick") == false && this.m_BullyAnimator.GetBool("IsUnique") == false) //If the enemy is cooled down, and is not animating
			{
				this.m_AttackTimer = 0;
				EnemyAttack(m_ThisBoss);
			}

			if (this.m_AnimationLength <= 0)
			{
				this.m_AnimationLength = 0;
				this.m_BullyAnimator.SetBool("IsPunch", false);
				this.m_BullyAnimator.SetBool("IsKick", false);
				this.m_BullyAnimator.SetBool("IsUnique", false);
			}
		}
        if (this.GetComponent<BeamAttack>().m_Ammo > 0)//if there is ammo
        {
            WaterGun();//fire the water gun
            this.GetComponent<BeamAttack>().m_Ammo--;//decrease ammo
            tempTimer = tempTimerResetVal;
        }
        else //if out of ammo
        {
            --tempTimer;//countdown the timer until reload
        }
        if (tempTimer <= 0)
        {
            Debug.Log("Reload");
            this.GetComponent<BeamAttack>().m_Ammo = Constants.KB_WATER_AMMO;
            //tempTimer = tempTimerResetVal;
        }

		if (this.m_HP <= 0)
		{
            //enemySpawner.GetComponent<SpawnEnemies>().SpawnBoss(new Vector3(0, 0, 0), "QueenBully");
		}
	}
	//
}
