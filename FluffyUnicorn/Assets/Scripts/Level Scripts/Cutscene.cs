using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour 
{
    public string m_SceneName;
    public List<GameObject> m_Scenes;
    private int sceneCounter_ = 0;

    //changes background as well as message
    public void NextScene()
    {
        //set the previous pannel off and then set the next one on  
        m_Scenes[sceneCounter_].SetActive(false);
        sceneCounter_++;
        if(sceneCounter_ < m_Scenes.Count)
        {
            m_Scenes[sceneCounter_].SetActive(true);
        }
        else
        {
            Application.LoadLevel(m_SceneName);
        }
    }
}
