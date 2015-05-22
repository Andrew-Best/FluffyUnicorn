using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    #region public variables
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
    private PlayerData pData_;
    private PlayerController pController_;

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
    public int HealthLevel { get { return healthUpgradeCounter_; } set { healthUpgradeCounter_ = value; } }
    public int DamageLevel { get { return damageUpgradeCounter_; } set { damageUpgradeCounter_ = value; } }
    public int AttackRateLevel { get { return attackRateUpgradeCounter_; } set { attackRateUpgradeCounter_ = value; } }
    public int SpeedLevel { get { return speedUpgradeCounter_; } set { speedUpgradeCounter_ = value; } }
    public int CurrencyLevel { get { return currencyUpgradeCounter_; } set { currencyUpgradeCounter_ = value; } }
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
        if (pData_.m_Currency >= m_HealthCost && pData_.m_PlayerHealth < Constants.MAX_PLAYER_HEALTH)
        {
            pData_.m_Currency -= m_HealthCost;
            pData_.m_PlayerHealth += health;
        }
        else if (pData_.m_Currency <= m_HealthCost)
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
        if (pData_.m_Currency >= m_AttackRateCost && pData_.m_FireRate > Constants.MAX_PLAYER_Attack_Rate)
        {
            pData_.m_Currency -= m_AttackRateCost;
            pData_.m_FireRate -= fireRate;
            sliderValue_ += m_IncreaseSliderAmount;
        }
        else if (pData_.m_Currency <= m_AttackRateCost)
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
        if (pData_.m_Currency >= m_DamageCost && pData_.m_PlayerDamage < Constants.MAX_PLAYER_DAMAGE)
        {
            pData_.m_Currency -= m_DamageCost;
            pData_.m_PlayerDamage += playerDamage;
        }
        else if (pData_.m_Currency <= m_DamageCost)
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
        if (pData_.m_Currency >= m_SpeedCost && pData_.m_MaxSpeed < Constants.MAX_PLAYER_SPEED)
        {
            pData_.m_Currency -= m_SpeedCost;
            pData_.m_MaxSpeed += speed;
        }
        else if (pData_.m_Currency <= m_SpeedCost)
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
        if (pData_.m_Currency >= m_CurrencyCost && pData_.m_CurrencyScalar < Constants.MAX_PLAYER_CURRENCY)
        {
            pData_.m_Currency -= m_CurrencyCost;
            pData_.m_CurrencyScalar += currency;
        }
        else if (pData_.m_Currency <= m_CurrencyCost)
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
            if (pData_.m_Currency >= m_MeleeComboCost[meleeCounter_] && pController_.m_UnlockedMeleeCombos[meleeCounter_] != true)
            {
                m_MeleeImages[meleeCounter_].sprite = m_FilledJewel;
                pController_.m_UnlockedMeleeCombos[meleeCounter_] = true;
                pData_.m_Currency -= m_MeleeComboCost[meleeCounter_];
                meleeCounter_++;
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
            if (pData_.m_Currency >= m_ProjectileComboCost[projectileCounter_] && pController_.m_UnlockedProjectileCombos[projectileCounter_] != true)
            {
                m_ProjectileImages[projectileCounter_].sprite = m_FilledJewel;
                pController_.m_UnlockedProjectileCombos[projectileCounter_] = true;
                pData_.m_Currency -= m_ProjectileComboCost[projectileCounter_];
                projectileCounter_++;
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
            if (pData_.m_Currency >= m_MultiComboCost[combinedComboCounter_] && pController_.m_UnlockedCombinedCombos[combinedComboCounter_] != true)
            {
                m_CombinedComboImages[combinedComboCounter_].sprite = m_FilledJewel;
                pController_.m_UnlockedCombinedCombos[combinedComboCounter_] = true;
                pData_.m_Currency -= m_MultiComboCost[combinedComboCounter_];
                combinedComboCounter_++;
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