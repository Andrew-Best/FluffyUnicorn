using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    #region public variables
    public Slider m_FartSlider; //The slider showing progress towards your next possible fart
    public Slider m_HealthSlider; //The slider showing remaining player health
    public Text m_BullyNumberText; //Text element which shows numerically how many bullies are remaining
    public PlayerController m_PlayerControlller; //Player Controller 
    #endregion

    #region private variables
    private float currGasLevel_; //Value of the fart slider, how much gas has been built up
    private int bulliesNeeded_; //Value showing how many bullies are required to beat the level
    #endregion

    #region attributes
    public float GasLevel { get { return currGasLevel_; } set { currGasLevel_ = value; } }
    public int BulliesNeeded { set { bulliesNeeded_ = value; } }
    #endregion

    void Start () 
    {
        m_FartSlider.minValue = Constants.FART_SLIDER_MIN;
        m_FartSlider.maxValue = Constants.FART_SLIDER_MAX;
        m_HealthSlider.minValue = Constants.PLAYER_MIN_HEALTH;
        m_HealthSlider.maxValue = Constants.PLAYER_DEFAULT_MAX_HEALTH;
	}
	
	void Update () 
    {
        m_FartSlider.value = currGasLevel_;
        m_HealthSlider.value = m_PlayerControlller.m_PlayerHealth;
        m_BullyNumberText.text = "0" + "/" + bulliesNeeded_;
	}

    #region Player Movement UI Controls

    public void OnPointerUp()
    {
        m_PlayerControlller.OnPointerUp();
    }

    public void OnPointerDown()
    {
        m_PlayerControlller.OnPointerDown();
    }

    public void MoveLeft()
    {
        m_PlayerControlller.MoveLeft();
    }

    public void MoveRight()
    {
        m_PlayerControlller.MoveRight();
    }

    public void MoveUp()
    {
        m_PlayerControlller.MoveUp();
    }

    public void MoveDown()
    {
        m_PlayerControlller.MoveDown();
    }
    #endregion
}