using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighlightScript : MonoBehaviour 
{
    private Image buttonImage_;

    private int alphaMin_ = 0;          //Lowest number the alpha can go to
    private int alphaMax_ = 134;        //Highest number the alpha can go to

    private float alphaTimer_;          //Timer for the alpha interpolation
    private float timerMax_ = 10.0f;     //Max time for the alphaTimer_
    private float timerMin_ = 0.0f;     //Min time for the alphaTimer_
    private float lerpSpeed_ = 0.3f;
    private float tParam_ = 0.0f;

    private bool switchAlpha_ = false;  //Bool to determine whether or not the alpha should be increasing or decreasing. False = increase, True = decrease
	
    void Start()
    {
        buttonImage_ = this.GetComponent<Image>();
    }

	void Update ()
    {
        alphaTimer_ += Time.deltaTime;


        if(alphaTimer_ >= timerMax_)
        {
            Debug.Log(switchAlpha_);
            switchAlpha_ = !switchAlpha_;
            alphaTimer_ = timerMin_;
        }

        if (!switchAlpha_)
        {
            Debug.Log("Increase Alpha");
            buttonImage_.CrossFadeAlpha(alphaMax_, 200.0f, false);
            Debug.Log(buttonImage_.color.a);
        }
        else if(switchAlpha_)
        {
            Debug.Log("Decrease Alpha");
            buttonImage_.CrossFadeAlpha(0, 200.0f, false);
            Debug.Log(buttonImage_.color.a);
        }
	}
}
