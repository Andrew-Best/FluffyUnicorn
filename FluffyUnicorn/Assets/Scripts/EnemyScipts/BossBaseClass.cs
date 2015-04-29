using UnityEngine;
using System.Collections;

public class BossBaseClass : EnemyBaseClass
{
	public GameObject m_Player;
	public GameObject m_ThisBoss;

	public override void InitEnemy(Vector2 spawnPos, int row)
	{
		base.InitEnemy(spawnPos, row);

	}

	// Update is called once per frame
	public void BossUpdate () 
	{
		this.m_ThisBoss.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
	}
}
