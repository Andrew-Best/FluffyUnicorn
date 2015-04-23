using UnityEngine;
using System.Collections;

public class UpgradeManager : MonoBehaviour 
{
    public PlayerController m_Player;
    public int m_HealthCost = 1;
    public int m_AttackRateCost = 1;
    public int m_DamageCost = 1;
    public int m_SpeedCost = 1;
    public int m_CurrencyCost = 1;
    public int m_MaxHealth = 5;
    public int m_MaxDamage = 3;
    public int m_MaxCurrency = 3;

    public float m_MaxAttackRate = 0.1f;
    public float m_MaxSpeed = 13.0f;

    public void UpgradeHealth(int health)
    {
        if (m_Player.m_Currency >= m_HealthCost && m_Player.m_PlayerHealth < m_MaxHealth)
        {
            m_Player.m_Currency -= m_HealthCost;
            m_Player.m_PlayerHealth += health;
        }  
    }

    public void UpgradeAttackRate(float fireRate)
    {
        if (m_Player.m_Currency >= m_AttackRateCost && m_Player.m_FireRate > m_MaxAttackRate)
        {
            m_Player.m_Currency -= m_AttackRateCost;
            m_Player.m_FireRate -= fireRate;
        }     
    }

    public void UpgadeDamage(int playerDamage)
    {
        if (m_Player.m_Currency >= m_DamageCost && m_Player.m_PlayerDamage < m_MaxDamage)
        {
            m_Player.m_Currency -= m_DamageCost;
            m_Player.m_PlayerDamage += playerDamage;
        }   
    }

    public void UpgradeSpeed(float speed)
    {
        if (m_Player.m_Currency >= m_SpeedCost && m_Player.m_MaxSpeed < m_MaxSpeed)
        {
            m_Player.m_Currency -= m_SpeedCost;
            m_Player.m_MaxSpeed += speed;
        }  
    }

    public void UpgradeCurrency(int currency)
    {
        if (m_Player.m_Currency >= m_CurrencyCost && m_Player.m_CurrencyScalar < m_MaxCurrency)
        {
            m_Player.m_Currency -= m_CurrencyCost;
            m_Player.m_CurrencyScalar += currency;
        }
    }
}
