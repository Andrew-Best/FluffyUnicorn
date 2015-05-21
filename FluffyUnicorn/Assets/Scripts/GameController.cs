using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class GameController : MonoBehaviour 
{
    public UIController m_UIControl;
    public UIController m_AndroidUI;
    public PlayerData m_PData;
    

    private int currLevel_; //Current level the player is on

	void Start () 
    {
        Screen.orientation = ScreenOrientation.Landscape;
#if UNITY_ANDROID
        m_AndroidUI.gameObject.SetActive(true);
        m_UIControl.gameObject.SetActive(false);
#endif

#if UNITY_EDITOR
        m_AndroidUI.gameObject.SetActive(false);
        m_UIControl.gameObject.SetActive(true);
#endif
        currLevel_ = 1; //Temporary default
        StartLevel();
	}
	
	void Update ()
    {
	    
	}

    void StartLevel()
    {
        m_UIControl.BulliesNeeded = Constants.BULLY_LEVEL_REQUIREMENT + currLevel_;
    }

    void UpdateLevel()
    {
        /*if(SpawnEnemies.bossSpawned_ == true)
        {
            m_EnemySpawner.gameObject.SetActive(false);
        }
        else
        {
            m_EnemySpawner.gameObject.SetActive(true);
        }*/
    }

    #region Save/Load
    void Save()
    {
        //Check if the save file exists
        //If it doesn't, make it
        if(!File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "fuSaveData.dat"))
        {
            Debug.Log("Creating file");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + Path.DirectorySeparatorChar + "fuSaveData.dat");
            SaveData sData = new SaveData();

            //Insert save code here
            

            bf.Serialize(file, sData);
            file.Close();
        }
        //If It does, save to the existing file
        else
        {
            Debug.Log("Saving to " + Application.persistentDataPath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar + "fuSaveData.dat", FileMode.Open);
            SaveData sData = new SaveData();

            //Insert save code here

            bf.Serialize(file, sData);
            file.Close();
        }
    }

    void Load()
    {
        if(File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "fuSaveData.dat"))
        {
            Debug.Log("Loading from " + Application.persistentDataPath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar + "fuSaveData.dat", FileMode.Open);
            SaveData sData = (SaveData)bf.Deserialize(file);

            //Insert load code here

            file.Close();
        }
        else
        {
            Debug.Log("Failed to load, file doesn't exist");
        }
    }
    #endregion
}