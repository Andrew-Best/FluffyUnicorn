using UnityEngine;
using System.Collections;

public class BullyScript : EnemyBaseClass
{
	public override void SpawnEnemy(int row, int type)
	{
		base.SpawnEnemy(row, type);
	}
	public override void InitEnemy()
	{
		base.InitEnemy();

		m_VelocityX = Constants.BULLY_VEL_X;
		m_AttackTimer = Constants.BULLY_ATTACK_TIMER_RESET_VALUE;
		m_HP = Constants.BULLY_HP;
		m_Damage = 0;
		//		m_CurRow = Random.Range(1, 3);
		m_EnemyInMotion = true;

		m_AttackPunchOdds = Constants.BULLY_PUNCH_ODDS;
		m_AttackKickOdds = Constants.BULLY_KICK_ODDS;
		m_AttackUniqueOdds = Constants.BULLY_UNIQUE_ATK_ODDS;
	}
	public override void EnemyAttackKick()
	{
		//play Bully's Kick Animation
		m_Damage = Constants.BULLY_KICK_DAMAGE;
		float AttackTimer = Constants.BULLY_ATTACK_TIMER_RESET_VALUE + Constants.BULLY_KICK_RESTTIME;
		ResetEnemyAttackTimer(AttackTimer);

	}
	public override void EnemyAttackPunch()
	{
		//play Bully's Kick Animation
		m_Damage = Constants.BULLY_PUNCH_DAMAGE;
		float AttackTimer = Constants.BULLY_ATTACK_TIMER_RESET_VALUE + Constants.BULLY_PUNCH_RESTTIME;
          ResetEnemyAttackTimer(AttackTimer);

	}
	public override void EnemyAttackUnique()
	{
		//play Bully's Kick Animation
		m_Damage = Constants.BULLY_UNIQUE_ATTACK_DAMAGE;
		float AttackTimer = Constants.BULLY_ATTACK_TIMER_RESET_VALUE + Constants.BULLY_UNIQUE_ATK_RESTTIME;
          ResetEnemyAttackTimer(AttackTimer);
	}


}
