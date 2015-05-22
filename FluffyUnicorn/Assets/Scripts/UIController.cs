using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    #region Public Variables
    public Slider m_FartSlider; //The slider showing progress towards your next possible fart
    public Slider m_HealthSlider; //The slider showing remaining player health
    public Text m_BullyNumberText; //Text element which shows numerically how many bullies are remaining
    #endregion

    #region Private Variables
    private PlayerData pData_; //Player Data script
    private PlayerController pController_; //Player Controller script

    private float currGasLevel_; //Value of the fart slider, how much gas has been built up
    private int bulliesNeeded_; //Value showing how many bullies are required to beat the level
    #endregion

    #region Attributes
    public float GasLevel { get { return currGasLevel_; } set { currGasLevel_ = value; } }
    public int BulliesNeeded { set { bulliesNeeded_ = value; } }
    #endregion

    void Start () 
    {
        m_FartSlider.minValue = Constants.FART_SLIDER_MIN;
        m_FartSlider.maxValue = Constants.FART_SLIDER_MAX;
        pData_ = GameObject.Find("Player").GetComponent<PlayerData>();
        pController_ = GameObject.Find("Player").GetComponent<PlayerController>();
       // m_HealthSlider.minValue = Constants.PLAYER_MIN_HEALTH;
      //  m_HealthSlider.maxValue = Constants.PLAYER_DEFAULT_MAX_HEALTH;
	}
	
	void Update () 
    {
        m_FartSlider.value = currGasLevel_;
//        m_HealthSlider.value = m_PlayerData.m_PlayerHealth;
        m_BullyNumberText.text = "0" + "/" + bulliesNeeded_;
	}

    #region Player Movement UI Controls

    public void OnPointerUp()
    {
        pController_.OnPointerUp();
    }

    public void OnPointerDown()
    {
        pController_.OnPointerDown();
    }

    public void MoveLeft()
    {
        pController_.MoveLeft();
    }

    public void MoveRight()
    {
        pController_.MoveRight();
    }

    public void MoveUp()
    {
        pController_.MoveUp();
    }

    public void MoveDown()
    {
        pController_.MoveDown();
    }
    #endregion
}