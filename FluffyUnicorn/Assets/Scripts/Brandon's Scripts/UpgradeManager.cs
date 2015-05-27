﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    #region public variables
    public AudioClip m_Error;
    public AudioClip m_Unlock;
    
    public int m_AmountOfUpgrades = 3;
    #endregion

    #region Private Variables
    private PlayerData pData_;
    private PlayerController pController_;

    private int[] meleeComboCost_ = new int[] { 50, 100, 150 };
    private int[] projectileComboCost_ = new int[] { 50, 100, 150 };
    private int[] multiComboCost_ = new int[] { 50, 100, 150 };

    private int meleeCounter_ = 0;
    private int projectileCounter_ = 0;
    private int combinedComboCounter_ = 0;
    private int healthUpgradeCounter_ = 1;
    private int damageUpgradeCounter_ = 1;
    private int attackRateUpgradeCounter_ = 1;
    private int speedUpgradeCounter_ = 1;
    private int currencyUpgradeCounter_ = 1;
    //these bools are used once to set the upgrades counters to their right locations. After they are set to false and are not used again. 
    private bool checkProjectileCounter_ = true;
    private bool checkMeleeCounter_ = true;
    private bool checkCombinedCounter_ = true;

    private AudioSource audio_;
    #endregion

    #region UI Variables
    public GameObject[] m_UIElements;
    public GameObject m_UpgradeMenu;
    public GameObject m_Description;
    public Image[] m_MeleeImages;
    public Image[] m_ProjectileImages;
    public Image[] m_CombinedComboImages;
    public Sprite m_FilledJewel;
    public Text m_Health;
    public Text m_Currency;
    public Text m_FireRate;
    public Text m_Damage;
    public Text m_Speed;
    public Slider m_FireRateSlider;
    public Button m_HealthButton;
    public Button m_DamageButton;
    public Button m_SpeedButton;
    public Button m_ScalarButton;
    public Button m_FireRateButton;
    public Button m_ProjectileButton;
    public Button m_MeleeButton;
    public Button m_ComboButton;
    public float m_IncreaseSliderAmount;
    private float sliderValue_ = 0.0f;
    #endregion

    #region Attributes
    #region UpgradeCounters
    public int HealthLevel { get { return healthUpgradeCounter_; } set { healthUpgradeCounter_ = value; } }
    public int DamageLevel { get { return damageUpgradeCounter_; } set { damageUpgradeCounter_ = value; } }
    public int AttackRateLevel { get { return attackRateUpgradeCounter_; } set { attackRateUpgradeCounter_ = value; } }
    public int SpeedLevel { get { return speedUpgradeCounter_; } set { speedUpgradeCounter_ = value; } }
    public int CurrencyLevel { get { return currencyUpgradeCounter_; } set { currencyUpgradeCounter_ = value; } }
    #endregion
    #region UpgradeCosts
    public int HealthCost { get { return Constants.BASE_UPGRADE_COST * healthUpgradeCounter_; } }
    public int DamageCost { get { return Constants.BASE_UPGRADE_COST * damageUpgradeCounter_; } }
    public int AttackRateCost { get { return Constants.BASE_UPGRADE_COST * attackRateUpgradeCounter_; } }
    public int SpeedCost { get { return Constants.BASE_UPGRADE_COST * speedUpgradeCounter_; } }
    public int CurrencyCost { get { return Constants.BASE_UPGRADE_COST * currencyUpgradeCounter_; } }
    #endregion
    #endregion

    void Start()
    {
        pData_ = GameObject.Find("Player").GetComponent<PlayerData>();
        pController_ = GameObject.Find("Player").GetComponent<PlayerController>();
        audio_ = GetComponent<AudioSource>();
    }

    void Update()
    {
        UpdateUpgradeUI();
        m_FireRateSlider.value = sliderValue_;
    }

    public void UpgradeHealth(int health)
    {
        if (pData_.m_Currency >= HealthCost && pData_.m_PlayerHealth < Constants.MAX_PLAYER_HEALTH)
        {
            pData_.m_Currency -= HealthCost;
            pData_.m_PlayerHealth += health;
            PlayAudio(m_Unlock);
        }
        else if (pData_.m_Currency <= HealthCost)
        {
            PlayAudio(m_Error);
        }
        else
        {
            m_HealthButton.interactable = false;
        }
        if(pData_.m_PlayerHealth > Constants.MAX_PLAYER_HEALTH)
        {
            pData_.m_PlayerHealth = Constants.MAX_PLAYER_HEALTH;
        }
    }

    public void UpgradeAttackRate(float fireRate)
    {
        if (pData_.m_Currency >= AttackRateCost && pData_.m_FireRate > Constants.MAX_PLAYER_Attack_Rate)
        {
            pData_.m_Currency -= AttackRateCost;
            pData_.m_FireRate -= fireRate;
            sliderValue_ += m_IncreaseSliderAmount;
            PlayAudio(m_Unlock);
        }
        else if (pData_.m_Currency <= AttackRateCost)
        {
            PlayAudio(m_Error);
        }
        else
        {
            m_FireRateButton.interactable = false;
        }
        if(pData_.m_FireRate < 0.1f)
        {
            pData_.m_FireRate = 0.1f;
        }
    }

    public void UpgadeDamage(int playerDamage)
    {
        if (pData_.m_Currency >= DamageCost && pData_.m_PlayerDamage < Constants.MAX_PLAYER_DAMAGE)
        {
            pData_.m_Currency -= DamageCost;
            pData_.m_PlayerDamage += playerDamage;
            PlayAudio(m_Unlock);
        }
        else if (pData_.m_Currency <= DamageCost)
        {
            PlayAudio(m_Error);
        }
        else
        {
            m_DamageButton.interactable = false;
        }
    }

    public void UpgradeSpeed(float speed)
    {
        if (pData_.m_Currency >= SpeedCost && pData_.m_MaxSpeed < Constants.MAX_PLAYER_SPEED)
        {
            pData_.m_Currency -= SpeedCost;
            pData_.m_MaxSpeed += speed;
            PlayAudio(m_Unlock);
        }
        else if (pData_.m_Currency <= SpeedCost)
        {
            PlayAudio(m_Error);
        }
        else
        {
            m_SpeedButton.interactable = false;
        }
    }

    public void UpgradeCurrency(int currency)
    {
        if (pData_.m_Currency >= CurrencyCost && pData_.m_CurrencyScalar < Constants.MAX_PLAYER_CURRENCY)
        {
            pData_.m_Currency -= CurrencyCost;
            pData_.m_CurrencyScalar += currency;
            PlayAudio(m_Unlock);
        }
        else if (pData_.m_Currency <= CurrencyCost)
        {
            PlayAudio(m_Error);
        }
        else
        {
            m_ScalarButton.interactable = false;
        }
    }

    public void UpgradeMeleeCombo()
    {
        //set the counter to the right variable as the player may already have upgrades
        for (int i = 0; i < pController_.m_UnlockedMeleeCombos.Length; ++i)
        {
            if (pController_.m_UnlockedMeleeCombos[i] == true && checkMeleeCounter_)
            {
                meleeCounter_++;
            }
        }
        checkMeleeCounter_ = false;
        if (meleeCounter_ < pController_.m_UnlockedMeleeCombos.Length)
        {
            //if you have enough money and the combo isn't already unlocked then upgrade
            if (pData_.m_Currency >= meleeComboCost_[meleeCounter_] && pController_.m_UnlockedMeleeCombos[meleeCounter_] != true)
            {
                m_MeleeImages[meleeCounter_].sprite = m_FilledJewel;
                pController_.m_UnlockedMeleeCombos[meleeCounter_] = true;
                pData_.m_Currency -= meleeComboCost_[meleeCounter_];
                meleeCounter_++;
                PlayAudio(m_Unlock);
            }
            else 
            {
                PlayAudio(m_Error);
            }
        }
        else
        {
            m_MeleeButton.interactable = false;
        }
    }

    public void UpgradeProjectileCombo()
    {
        //set the counter to the right variable as the player may already have upgrades
        for (int i = 0; i < pController_.m_UnlockedProjectileCombos.Length; ++i)
        {
            if (pController_.m_UnlockedProjectileCombos[i] == true && checkProjectileCounter_)
            {
                projectileCounter_++;
            }
        }
        checkProjectileCounter_ = false;
        if (projectileCounter_ < pController_.m_UnlockedProjectileCombos.Length)
        {
            //if you have enough money and the combo isn't already unlocked then upgrade
            if (pData_.m_Currency >= projectileComboCost_[projectileCounter_] && pController_.m_UnlockedProjectileCombos[projectileCounter_] != true)
            {
                m_ProjectileImages[projectileCounter_].sprite = m_FilledJewel;
                pController_.m_UnlockedProjectileCombos[projectileCounter_] = true;
                pData_.m_Currency -= projectileComboCost_[projectileCounter_];
                projectileCounter_++;
                PlayAudio(m_Unlock);
            }
            else
            {
                PlayAudio(m_Error);
            }
        }
        else
        {
            m_ProjectileButton.interactable = false;
        }
    }

    public void UpgradeMultiCombo()
    {
        //set the counter to the right variable as the player may already have upgrades
        for (int i = 0; i < pController_.m_UnlockedCombinedCombos.Length; ++i)
        {
            if (pController_.m_UnlockedCombinedCombos[i] == true && checkCombinedCounter_)
            {
                combinedComboCounter_++;
            }
        }
        checkCombinedCounter_ = false;
        if (combinedComboCounter_ < pController_.m_UnlockedCombinedCombos.Length)
        {
            //if you have enough money and the combo isn't already unlocked then upgrade
            if (pData_.m_Currency >= multiComboCost_[combinedComboCounter_] && pController_.m_UnlockedCombinedCombos[combinedComboCounter_] != true)
            {
                m_CombinedComboImages[combinedComboCounter_].sprite = m_FilledJewel;
                pController_.m_UnlockedCombinedCombos[combinedComboCounter_] = true;
                pData_.m_Currency -= multiComboCost_[combinedComboCounter_];
                combinedComboCounter_++;
                PlayAudio(m_Unlock);
            }
            else
            {
                PlayAudio(m_Error);
            }
        }
        else
        {
            m_ComboButton.interactable = false;
        }
    }

    public void OpenMenu()
    {
        //disable previous items and enable the upgrade menu 
        for(int i = 0; i < m_UIElements.Length; ++i)
        {
            m_UIElements[i].SetActive(false);
        }
        m_UpgradeMenu.SetActive(true);
    }

    public void CloseMenu()
    {
        //disable upgrade menu and reenable everything else 
        m_UpgradeMenu.SetActive(false);
        for(int i = 0; i < m_UIElements.Length; ++i)
        {
            m_UIElements[i].SetActive(true);
        }
    }

    public void OpenDescription()
    {
        m_UpgradeMenu.SetActive(false);
        m_Description.SetActive(true);
    }

    public void CloseDescription()
    {
        m_UpgradeMenu.SetActive(true);
        m_Description.SetActive(false);
    }

    void UpdateUpgradeUI()
    {
        //if there is an upgraded combo switch the image of that upgrade to a filled circle. 
        for (int i = 0; i < m_AmountOfUpgrades; i++)
        {
           if(pController_.m_UnlockedCombinedCombos[i] == true)
           {
               m_CombinedComboImages[i].sprite = m_FilledJewel;
           }
           if (pController_.m_UnlockedProjectileCombos[i] == true)
           {
               m_ProjectileImages[i].sprite = m_FilledJewel;
           }
           if (pController_.m_UnlockedMeleeCombos[i] == true)
           {
               m_MeleeImages[i].sprite = m_FilledJewel;
           }
        }
        m_Health.text = "Player Health: " + pData_.m_PlayerHealth.ToString();
        m_Speed.text = "Player Speed: " + pData_.m_MaxSpeed.ToString("F1");   //'F1' makes it one decimal
        m_Currency.text = "Currency Scalar: " + pData_.m_CurrencyScalar.ToString();
        m_FireRate.text = "Fire Rate: " + pData_.m_FireRate.ToString("F1");   //'F1' makes it one decimal
        m_Damage.text = "Player Damage: " + pData_.m_PlayerDamage.ToString();
    }

    public void PlayAudio(AudioClip audioClip)
    {
        if (!audio_.isPlaying)
        {
           AudioSource.PlayClipAtPoint(audioClip, this.transform.position);
        }  
    }
}