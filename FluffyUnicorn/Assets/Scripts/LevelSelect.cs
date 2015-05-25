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

    void CreateButtons()
    {
        //float startXPos = -476.25f;
        //float startYPos = 159.966f;
        float startXPos = -438.8f;
        float startYPos = 134.0f;
        //float startXPos = 0.0f;
        //float startYPos = -1.0f;
        int row = 0;
        int col = 0;
        int maxCol = 4;
        int maxRow = 2;

        for(int i = 0; i < Constants.LEVELS_PER_STAGE; ++i)
        {
            GameObject go = (GameObject)Instantiate(m_ButtonPrefab, new Vector3(startXPos, startYPos, 0.0f), Quaternion.identity);
            go.gameObject.transform.SetParent(m_LevelButtonsParent.transform, false);
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonWidth_, buttonHeight_);
            go.GetComponent<Button>().onClick.AddListener(delegate { ChooseLevel(go); });
            go.gameObject.name = "Level" + (i + 1);

            if(col < maxCol)
            {
                col++;
                startXPos += buttonWidth_;
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

            levelButtons_.Add(go);
        }

        CheckLevelProgress();
    }

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