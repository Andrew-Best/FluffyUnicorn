using UnityEngine;
using System.Collections;

public class UpgradeManager : MonoBehaviour
{
    #region public variables
    public PlayerData m_Player;
    public int m_HealthCost = 1;
    public int m_AttackRateCost = 1;
    public int m_DamageCost = 1;
    public int m_SpeedCost = 1;
    public int m_CurrencyCost = 1;
    #endregion

    #region Private Variables
    private int healthUpgradeCounter_ = 1;
    private int damageUpgradeCounter_ = 1;
    private int attackRateUpgradeCounter_ = 1;
    private int speedUpgradeCounter_ = 1;
    private int currencyUpgradeCounter_ = 1;
    #endregion

    #region Attributes
    public int HealthLevel { get { return healthUpgradeCounter_; } set { healthUpgradeCounter_ = value; } }
    public int DamageLevel { get { return damageUpgradeCounter_; } set { damageUpgradeCounter_ = value; } }
    public int AttackRateLevel { get { return attackRateUpgradeCounter_; } set { attackRateUpgradeCounter_ = value; } }
    public int SpeedLevel { get { return speedUpgradeCounter_; } set { speedUpgradeCounter_ = value; } }
    public int CurrencyLevel { get { return currencyUpgradeCounter_; } set { currencyUpgradeCounter_ = value; } }
    #endregion

    public void UpgradeHealth(int health)
    {
        if (m_Player.m_Currency >= m_HealthCost && m_Player.m_PlayerHealth < Constants.MAX_PLAYER_HEALTH)
        {
            m_Player.m_Currency -= m_HealthCost;
            m_Player.m_PlayerHealth += health;
        }  
        if(m_Player.m_PlayerHealth > Constants.MAX_PLAYER_HEALTH)
        {
            m_Player.m_PlayerHealth = Constants.MAX_PLAYER_HEALTH;
        }
    }

    public void UpgradeAttackRate(float fireRate)
    {
        if (m_Player.m_Currency >= m_AttackRateCost && m_Player.m_FireRate > Constants.MAX_PLAYER_Attack_Rate)
        {
            m_Player.m_Currency -= m_AttackRateCost;
            m_Player.m_FireRate -= fireRate;
        }     

        if(m_Player.m_FireRate < 0.1f)
        {
            m_Player.m_FireRate = 0.1f;
        }
    }

    public void UpgadeDamage(int playerDamage)
    {
        if (m_Player.m_Currency >= m_DamageCost && m_Player.m_PlayerDamage < Constants.MAX_PLAYER_DAMAGE)
        {
            m_Player.m_Currency -= m_DamageCost;
            m_Player.m_PlayerDamage += playerDamage;
        }   
    }

    public void UpgradeSpeed(float speed)
    {
        if (m_Player.m_Currency >= m_SpeedCost && m_Player.m_MaxSpeed < Constants.MAX_PLAYER_SPEED)
        {
            m_Player.m_Currency -= m_SpeedCost;
            m_Player.m_MaxSpeed += speed;
        }  
    }

    public void UpgradeCurrency(int currency)
    {
        if (m_Player.m_Currency >= m_CurrencyCost && m_Player.m_CurrencyScalar < Constants.MAX_PLAYER_CURRENCY)
        {
            m_Player.m_Currency -= m_CurrencyCost;
            m_Player.m_CurrencyScalar += currency;
        }
    }
}
