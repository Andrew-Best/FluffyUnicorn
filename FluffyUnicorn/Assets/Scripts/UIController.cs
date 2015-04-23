using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    #region public variables
    public Slider m_FartSlider; //The slider showing progress towards your next possible fart
    public Text m_BullyNumberText; //Text element which shows numerically how many bullies are remaining
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
	}
	
	void Update () 
    {
        m_FartSlider.value = currGasLevel_;
        m_BullyNumberText.text = "0" + "/" + bulliesNeeded_;
	}
}