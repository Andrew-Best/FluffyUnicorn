using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelSelect : MonoBehaviour
{
    public GameObject m_ButtonPrefab;
    public GameObject m_LevelButtonsParent;

    private GameObject lastClicked_;
    private GameObject currentClick_;
    private PlayerData pData_;
    private List<GameObject> levelButtons_ = new List<GameObject>();
    private Canvas levelSelectCanvas_;

    private int timesClicked_;

    private float buttonWidth_ = 320.0f;
    private float buttonHeight_ = 323.0f;

    void Awake()
    {
        pData_ = GameObject.Find("Player").GetComponent<PlayerData>();
        levelSelectCanvas_ = FindObjectOfType<Canvas>();

        CreateButtons();
    }

    //This function creates the buttons for the Level Select scene
    void CreateButtons()
    {
        //Set initial values - locally as they aren't needed elsewhere
        float startXPos = -438.8f;
        float startYPos = 134.0f;
        int row = 0;
        int col = 0;
        int maxCol = 4;
        int maxRow = 2;

        //Create a button per level
        for(int i = 0; i < Constants.LEVELS_PER_STAGE; ++i)
        {
            //Here we instantiate the button prefab we have already made, give it a position, set it's parent, size it properly, assign the function it calls and name it appropriately
            GameObject go = (GameObject)Instantiate(m_ButtonPrefab, new Vector3(startXPos, startYPos, 0.0f), Quaternion.identity);
            go.gameObject.transform.SetParent(m_LevelButtonsParent.transform, false);
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonWidth_, buttonHeight_);
            go.GetComponent<Button>().onClick.AddListener(delegate { ChooseLevel(go); });
            go.gameObject.name = "Level" + (i + 1);

            //If the column has not reached it's maximum, increment the amount of columns and increase the X position for the next button
            if(col < maxCol)
            {
                col++;
                startXPos += buttonWidth_;
                //If the column has reached it's maximum, reset the columns and X position, then incrememnt rows and decrease the Y pos for the next row
                if(col >= maxCol)
                {
                    if(row < maxRow)
                    {
                        col = 0;
                        startXPos = -438.8f;
                        row++;
                        startYPos -= buttonHeight_;
                    }
                }
            }
            //Add the new button to our list of buttons
            levelButtons_.Add(go);
        }
        CheckLevelProgress();
    }

    //This function checks how many levels the player has unlocked, and unlocks the appropriate level accordingly
    public void CheckLevelProgress()
    {
        for(int i = 0; i < levelButtons_.Count; ++i)
        {
            for (int j = 1; j < pData_.m_LevelsUnlocked + 1; ++j)
            {
                if (levelButtons_[i].name == "Level" + j)
                {
                    levelButtons_[i].GetComponent<Button>().interactable = true;
                    levelButtons_[i].gameObject.transform.FindChild("Lock").GetComponent<Image>().enabled = false;
                }
            }
        }
    }

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