using UnityEngine;
using System.Collections;

public class IdleBully : State 
{
	public override void OnStateEntered()
	{

	}

	public override void StateUpdate()
	{
/*		//moving right and has passed pointA
		if (enemyPos.x >= pointA && this.m_EnemyGoingLeft == -1)
		{
			enemyPos.x = pointA;
			this.TurnAround(bully);
		}
		//moving left and has passed point B
		if (enemyPos.x <= pointB && this.m_EnemyGoingLeft == 1)
		{
			this.TurnAround(bully);
		}
		DetectPlayer(player.transform.position, enemyPos);		*/	
	}

	public override void OnStateExit()
	{
		throw new System.NotImplementedException();
	}

	public override void StateGUI()
	{
		throw new System.NotImplementedException();
	}
}
