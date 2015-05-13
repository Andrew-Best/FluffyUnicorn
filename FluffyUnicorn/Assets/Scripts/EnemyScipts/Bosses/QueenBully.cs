using UnityEngine;
using System.Collections;

public class QueenBully : BossBaseClass 
{
	public GameObject m_Junk;
	public string m_JunkName;

	public Vector2 m_ThrowForce;
	private Vector2 playerPosition_;

	private float timeUntilNextThrow_;
	public const float DEFAULT_TIME_UNTIL_THROW = 10;

	// Use this for initialization
	void Start () 
	{
		InitEnemy(new Vector2(0, 0), 2, this.gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(timeUntilNextThrow_ > 0)
		{
			timeUntilNextThrow_--;
		}
		else
		{
//			m_JunkName = "DeadFish";//Junk Selector
//			m_Junk = ObjectPool.Instance.GetObjectForType(m_JunkName, true);

			ThrowStuff(this.gameObject, m_Junk);
			timeUntilNextThrow_ = DEFAULT_TIME_UNTIL_THROW;
		}
	
	}

	public void ThrowStuff(GameObject bully, GameObject junk)//Done
	{
		bully.GetComponent<EnemyBaseClass>().EnemyStopMotion(bully);
		//First, Get the Boss' position
		//Next, get object from pool
		int JunkSelect = Random.Range(0, 3);
		if(JunkSelect == 1)
		{
			m_JunkName = "PopCan";
		}
		else if(JunkSelect == 2)
		{
			m_JunkName = "DeadFish";
		}
		else
		{
			m_JunkName = "BurntToast";
		}
		GameObject m_Junk = ObjectPool.Instance.GetObjectForType(m_JunkName, true);
		//get player position
		GetPlayerInfo(bully);
		playerPosition_ = GetComponent<EnemyBaseClass>().m_PlayerPos;
		m_Junk.transform.position = bully.transform.position;
		//send object in an arc toward the player
		if (bully.GetComponent<EnemyBaseClass>().m_EnemyGoingLeft > 0)//if bully moving left
		{
			m_ThrowForce.x *= -1;//make the junk go left
		}
		m_Junk.GetComponent<Rigidbody2D>().velocity = new Vector2(m_ThrowForce.x, m_ThrowForce.y);

	}

	public override void InitEnemy(Vector2 spawnPos, int row, GameObject newBully)
	{
		base.InitEnemy(spawnPos, row, newBully);

		m_Position = m_ThisBoss.GetComponent<Rigidbody2D>().position;

		m_HP = Constants.QUEEN_BULLY_HP;

		m_Curstate = 0;
		m_BossName = "Queen Bully";
		m_CurFrame = 0;
		this.m_CurRow = row;
		//m_TotalFrames = this.GetComponent<Animator>().framesInAnim;
	}
}
