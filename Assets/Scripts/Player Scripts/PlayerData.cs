using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour
{
    #region Public Variables
    public float m_MaxSpeed = 5.0f;
    public float m_Acceleration = 1.0f;
    public float m_ShotSpeed = 10.0f;
    public float m_FireRate = 1.0f;
    public float m_PunchRate = 1.0f;
    public float m_Deceleration = 1.0f;
    public float m_PunchDamage = 1.0f;

    public int m_PlayerHealth = Constants.PLAYER_DEFAULT_MAX_HEALTH;
    public int m_PlayerDamage = 1;
    public int m_Currency = 0;
    public int m_CurrencyScalar = 1;
    public int m_LevelsUnlocked = 1; //This is an integer representing how many levels have been unlocked

    public UpgradeManager m_Upgrades;

    //used for upgrades so player can buy different combos
    public bool[] m_UnlockedMeleeCombos = new bool[3];
    public bool[] m_UnlockedProjectileCombos = new bool[3];
    public bool[] m_UnlockedCombinedCombos = new bool[3];
    #endregion

    //This function only serves as a means to incrememnt m_LevelsUnlocked when the player has successfully completed a level
    public void UnlockNextLevel()
    {
        m_LevelsUnlocked++;
    }
}