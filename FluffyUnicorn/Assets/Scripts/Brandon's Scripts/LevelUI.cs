using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public PlayerData m_Player;
    public Text m_Health;
    public Text m_Speed;
    public Text m_AttackRate;
    public Text m_Damage;
    public Text m_Currency;

    void Update()
    {
        m_Health.text = "Health: " + m_Player.m_PlayerHealth;
        m_Speed.text = "Speed: " + m_Player.m_MaxSpeed;
        m_AttackRate.text = "FireRate: " + m_Player.m_FireRate;
        m_Damage.text = "Damage: " + m_Player.m_PlayerDamage;
        m_Currency.text = "Currency: " + m_Player.m_Currency;
    }
}

