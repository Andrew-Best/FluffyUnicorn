using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighlightScript : MonoBehaviour 
{
    private Image buttonImage_;
    private Color startColour_;
    private Color endColour_;

    private float alphaMin_ = 0.0f;          //Lowest number the alpha can go to
    private float alphaMax_ = 0.8f;        //Highest number the alpha can go to

    private float alphaTimer_;          //Timer for the alpha interpolation
    private float timerMax_ = 0.8f;     //Max time for the alphaTimer_
    private float timerMin_ = 0.0f;     //Min time for the alphaTimer_

    private bool switchAlpha_ = false;  //Bool to determine whether or not the alpha should be increasing or decreasing. False = increase, True = decrease
	
    void Start()
    {
        buttonImage_ = this.GetComponent<Image>();
        buttonImage_.color = new Vector4(buttonImage_.color.r, buttonImage_.color.g, buttonImage_.color.b, alphaMin_);
        startColour_ = buttonImage_.color;
        endColour_ = new Vector4(buttonImage_.color.r, buttonImage_.color.g, buttonImage_.color.b, alphaMax_);
        this.GetComponent<HighlightScript>().enabled = false;
    }

	void Update ()
    {
        alphaTimer_ += Time.deltaTime;

        if(alphaTimer_ >= timerMax_)
        {
            switchAlpha_ = !switchAlpha_;
            alphaTimer_ = timerMin_;
        }

        if (!switchAlpha_)
        {
            buttonImage_.color = Color.Lerp(startColour_, endColour_, alphaTimer_);
        }
        else if(switchAlpha_)
        {
            buttonImage_.color = Color.Lerp(endColour_, startColour_, alphaTimer_);
        }
	}

    void OnDisable()
    {
        buttonImage_.color = new Vector4(buttonImage_.color.r, buttonImage_.color.g, buttonImage_.color.b, alphaMin_);
    }
}
