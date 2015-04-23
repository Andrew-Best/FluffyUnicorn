using UnityEngine;
using System.Collections;

public class UpgradeManager : MonoBehaviour 
{
    public PlayerController m_Player;

    public void UpgradeHealth(int health, int currency)
    {
      //  m_Player.m_PlayerHealth += health;
    }

    public void UpgradeAttackRate(float fireRate, int currency)
    {
        m_Player.m_FireRate += fireRate;
    }

    public void UpgadeDamage(int playerDamage, int currency)
    {
      //  m_Player.m_PlayerDamage += playerDamage;
    }

    public void UpgradeSpeed(float speed, int currency)
    {

    }
}
