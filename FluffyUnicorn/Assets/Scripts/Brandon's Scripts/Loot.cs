using UnityEngine;
using System.Collections;
using System;

public class Loot : MonoBehaviour
{
    #region Public variables
    //change to whatever loot you want to give the player
    public enum m_Loot
    {
        HEALTH = 0,
        MONEY
    };
    public m_Loot m_LootType;

    public int m_Money;     //how much money to give to the player

    public PlayerController m_Player;
    #endregion

    #region Private variables
    private bool canGiveLoot_ = true;
    private bool playFinishedAnimation_ = false;

    private Animator animator_;
    #endregion

    void Start()
    {
        animator_ = this.GetComponent<Animator>();
    }

    void Update()
    {
        SetAnimationValues();   //update animation
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        UpgradeManager upgrades = other.gameObject.GetComponentInChildren<UpgradeManager>();
        if (other.tag == "Player" && canGiveLoot_)
        {
            canGiveLoot_ = !canGiveLoot_;
            if(m_LootType == m_Loot.HEALTH)
            {
                FillHealth();
            }
            else if(m_LootType == m_Loot.MONEY)
            {
                AddCurrency();
            }
            playFinishedAnimation_ = true;
        }
    }

    void SetAnimationValues()
    {
        animator_.SetBool("Finished", playFinishedAnimation_);
    }

    #region Upgrades
    public void FillHealth()
    {
        m_Player.m_PlayerHealth = Constants.PLAYER_DEFAULT_MAX_HEALTH;     
    }

    public void AddCurrency()
    {
        m_Player.m_Currency += m_Money;  
    }
    #endregion
}
