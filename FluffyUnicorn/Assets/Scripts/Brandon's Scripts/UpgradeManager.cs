using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    #region public variables
    public PlayerData m_Player;
    public PlayerController m_PlayerController;
    public AudioClip m_Error;
    public int m_HealthCost = 1;
    public int m_AttackRateCost = 1;
    public int m_DamageCost = 1;
    public int m_SpeedCost = 1;
    public int m_CurrencyCost = 1;
    public int[] m_MeleeComboCost;
    public int[] m_ProjectileComboCost;
    public int[] m_MultiComboCost;
    public int m_AmountOfUpgrades = 3;
    #endregion

    #region Private Variables
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
    public Image[] m_MeleeImages;
    public Image[] m_ProjectileImages;
    public Image[] m_CombinedComboImages;
    public Sprite m_FilledJewel;
    public Text m_Health;
    public Text m_Currency;
    public Text m_FireRate;
    public Text m_Damage;
    public Text m_Speed;
    #endregion

    #region Attributes
    public int HealthLevel { get { return healthUpgradeCounter_; } set { healthUpgradeCounter_ = value; } }
    public int DamageLevel { get { return damageUpgradeCounter_; } set { damageUpgradeCounter_ = value; } }
    public int AttackRateLevel { get { return attackRateUpgradeCounter_; } set { attackRateUpgradeCounter_ = value; } }
    public int SpeedLevel { get { return speedUpgradeCounter_; } set { speedUpgradeCounter_ = value; } }
    public int CurrencyLevel { get { return currencyUpgradeCounter_; } set { currencyUpgradeCounter_ = value; } }
    #endregion

    void Start()
    {
        audio_ = GetComponent<AudioSource>();
    }

    void Update()
    {
        UpdateUpgradeUI();
    }

    public void UpgradeHealth(int health)
    {
        if (m_Player.m_Currency >= m_HealthCost && m_Player.m_PlayerHealth < Constants.MAX_PLAYER_HEALTH)
        {
            m_Player.m_Currency -= m_HealthCost;
            m_Player.m_PlayerHealth += health;
        }
        else
        {
            PlayAudio(m_Error);
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
        else
        {
            PlayAudio(m_Error);
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
        else
        {
            PlayAudio(m_Error);
        }
    }

    public void UpgradeSpeed(float speed)
    {
        if (m_Player.m_Currency >= m_SpeedCost && m_Player.m_MaxSpeed < Constants.MAX_PLAYER_SPEED)
        {
            m_Player.m_Currency -= m_SpeedCost;
            m_Player.m_MaxSpeed += speed;
        }
        else
        {
            PlayAudio(m_Error);
        }
    }

    public void UpgradeCurrency(int currency)
    {
        if (m_Player.m_Currency >= m_CurrencyCost && m_Player.m_CurrencyScalar < Constants.MAX_PLAYER_CURRENCY)
        {
            m_Player.m_Currency -= m_CurrencyCost;
            m_Player.m_CurrencyScalar += currency;
        }
        else
        {
            PlayAudio(m_Error);
        }
    }

    public void UpgradeMeleeCombo()
    {
        //set the counter to the right variable as the player may already have upgrades
        for (int i = 0; i < m_PlayerController.m_UnlockedMeleeCombos.Length; ++i)
        {
            if (m_PlayerController.m_UnlockedMeleeCombos[i] == true && checkMeleeCounter_)
            {
                meleeCounter_++;
            }
        }
        checkMeleeCounter_ = false;
        //if you have enough money and the combo isn't already unlocked then upgrade
        if (m_Player.m_Currency >= m_MeleeComboCost[meleeCounter_] && m_PlayerController.m_UnlockedMeleeCombos[meleeCounter_] != true)
        {       
            m_MeleeImages[meleeCounter_].sprite = m_FilledJewel;     
            m_PlayerController.m_UnlockedMeleeCombos[meleeCounter_] = true;
            m_Player.m_Currency -= m_MeleeComboCost[meleeCounter_];
            meleeCounter_++;
        }
        else
        {
            PlayAudio(m_Error);
        }
    }

    public void UpgradeProjectileCombo()
    {
        //set the counter to the right variable as the player may already have upgrades
        for (int i = 0; i < m_PlayerController.m_UnlockedProjectileCombos.Length; ++i)
        {
            if (m_PlayerController.m_UnlockedProjectileCombos[i] == true && checkProjectileCounter_)
            {
                projectileCounter_++;
            }
        }
        checkProjectileCounter_ = false;
        //if you have enough money and the combo isn't already unlocked then upgrade
        if (m_Player.m_Currency >= m_ProjectileComboCost[projectileCounter_] && m_PlayerController.m_UnlockedProjectileCombos[projectileCounter_] != true)
        {
            m_ProjectileImages[projectileCounter_].sprite = m_FilledJewel;
            m_PlayerController.m_UnlockedProjectileCombos[projectileCounter_] = true;
            m_Player.m_Currency -= m_ProjectileComboCost[projectileCounter_];
            projectileCounter_++;
        }
        else
        {
            PlayAudio(m_Error);
        }
    }

    public void UpgradeMultiCombo()
    {
        //set the counter to the right variable as the player may already have upgrades
        for (int i = 0; i < m_PlayerController.m_UnlockedCombinedCombos.Length; ++i)
        {
            if (m_PlayerController.m_UnlockedCombinedCombos[i] == true && checkCombinedCounter_)
            {
                combinedComboCounter_++;
            }
        }
        checkCombinedCounter_ = false;
        //if you have enough money and the combo isn't already unlocked then upgrade
        if (m_Player.m_Currency >= m_MultiComboCost[combinedComboCounter_] && m_PlayerController.m_UnlockedCombinedCombos[combinedComboCounter_] != true)
        {           
            m_CombinedComboImages[combinedComboCounter_].sprite = m_FilledJewel;
            m_PlayerController.m_UnlockedCombinedCombos[combinedComboCounter_] = true;
            m_Player.m_Currency -= m_MultiComboCost[combinedComboCounter_];
            combinedComboCounter_++;
        }
        else
        {
            PlayAudio(m_Error);
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

    void UpdateUpgradeUI()
    {
        //if there is an upgraded combo switch the image of that upgrade to a filled circle. 
        for (int i = 0; i < m_AmountOfUpgrades; i++)
        {
           if(m_PlayerController.m_UnlockedCombinedCombos[i] == true)
           {
               m_CombinedComboImages[i].sprite = m_FilledJewel;
           }
           if (m_PlayerController.m_UnlockedProjectileCombos[i] == true)
           {
               m_ProjectileImages[i].sprite = m_FilledJewel;
           }
           if (m_PlayerController.m_UnlockedMeleeCombos[i] == true)
           {
               m_MeleeImages[i].sprite = m_FilledJewel;
           }
        }
        m_Health.text = "Player Health: " + m_Player.m_PlayerHealth.ToString();
        m_Speed.text = "Player Speed: " + m_Player.m_MaxSpeed.ToString("F1");   //'F1' makes it one decimal
        m_Currency.text = "Currency Scalar: " + m_Player.m_CurrencyScalar.ToString();
        m_FireRate.text = "Fire Rate: " + m_Player.m_FireRate.ToString("F1");   //'F1' makes it one decimal
        m_Damage.text = "Player Damage: " + m_Player.m_PlayerDamage.ToString();
    }

    public void PlayAudio(AudioClip audioClip)
    {
        if (!audio_.isPlaying)
        {
           AudioSource.PlayClipAtPoint(audioClip, this.transform.position);
        }  
    }
}