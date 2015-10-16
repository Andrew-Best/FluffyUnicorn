using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class MainMenuScript : MonoBehaviour 
{
    public Button m_ContinueButton;

	void Start ()
    {
        if (!File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "fuSaveData.dat"))
        {
            m_ContinueButton.interactable = true;
        }
	}
	
	void Update ()
    {
	
	}
}
