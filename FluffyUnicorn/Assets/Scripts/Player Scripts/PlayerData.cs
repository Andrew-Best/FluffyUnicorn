using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour
{
    #region Public Variables
    public float m_MaxSpeed = 5.0f;
    public float m_Acceleration = 1.0f;
    public float m_ShotSpeed = 10.0f;
    public float m_FireRate = 1.0f;
    public float m_Deceleration = 1.0f;

    public int m_PlayerHealth = Constants.PLAYER_DEFAULT_MAX_HEALTH;
    public int m_PlayerDamage = 1;
    public int m_Currency = 0;
    public int m_CurrencyScalar = 1;

    public UpgradeManager m_Upgrades;
    #endregion
}