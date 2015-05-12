using UnityEngine;
using System.Collections;

public class QueenBully : BossBaseClass 
{

	// Use this for initialization
	void Start () 
	{
		InitEnemy(new Vector2(0, 0), 2, this.gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
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
