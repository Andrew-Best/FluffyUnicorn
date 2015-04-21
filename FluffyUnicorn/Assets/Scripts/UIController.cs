using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    #region public variables
    public Slider m_FartSlider; //The slider showing progress towards your next possible fart
    #endregion

    #region private variables
    private float currGasLevel_; //Value of the fart slider, how much gas has been built up
    #endregion

    #region attributes
    public float GasLevel { set { currGasLevel_ = value; } }
    #endregion

    void Start () 
    {
        m_FartSlider.minValue = Constants.FART_SLIDER_MIN;
        m_FartSlider.maxValue = Constants.FART_SLIDER_MAX;
	}
	
	void Update () 
    {
        m_FartSlider.value = currGasLevel_;
	}
}