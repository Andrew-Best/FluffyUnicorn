using UnityEngine;
using System.Collections;

public class BossBaseClass : EnemyBaseClass
{
//	public GameObject m_Player;
	public GameObject m_ThisBoss;
	public Vector2 m_Position;

	public int m_Curstate;

	public string m_BossName;

	public int m_CurFrame;
	public int m_TotalFrames;

	public override void InitEnemy(Vector2 spawnPos, int row)
	{
		base.InitEnemy(spawnPos, row);
		m_Player = GameObject.FindGameObjectWithTag("Player");
		this.m_ThisBoss.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);//Set Boss Velocity to 0
	}

	// Update is called once per frame
	public override void EnemyUpdate(GameObject ThisBoss) 
	{
		
	}
}
