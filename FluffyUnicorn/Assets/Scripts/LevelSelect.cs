using UnityEngine;
using System;
using System.Collections;

public class LevelSelect : MonoBehaviour
{
    private HighlightScript highlightScript_;
    private int timesClicked_;
    private GameObject lastClicked_;
    private GameObject currentClick_;
    
    public void ChooseLevel(GameObject go)
    {
        currentClick_ = go;
        if (lastClicked_ == null)
        {
            lastClicked_ = currentClick_;
        }

        if (currentClick_.name == lastClicked_.name)
        {
            timesClicked_++;
            lastClicked_.GetComponent<HighlightScript>().enabled = true;

            if (timesClicked_ >= 2)
            {
                try
                {
                    Application.LoadLevel(go.name);
                }
                catch(Exception e)
                {
                    Debug.Log("Level does not exist or is not unlocked, " + e);
                }
                
            }
        }
        else
        {
            lastClicked_.GetComponent<HighlightScript>().enabled = false;
            lastClicked_ = currentClick_;
            currentClick_.GetComponent<HighlightScript>().enabled = true;
            timesClicked_ = 0;
        }
    }
}