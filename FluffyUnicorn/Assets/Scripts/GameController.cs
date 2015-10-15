﻿using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class GameController : MonoBehaviour 
{
    private PlayerData pData_;
    private UIController UIControl_;
    private UIController AndroidUI_;

    private int currLevel_; //Current level the player is on

	void Start () 
    {
        UIControl_ = GameObject.Find("UI").GetComponent<UIController>();
        AndroidUI_ = GameObject.Find("AndroidUI").GetComponent<UIController>();

        Screen.orientation = ScreenOrientation.Landscape;
#if UNITY_ANDROID
        AndroidUI_.gameObject.SetActive(true);
        UIControl_.gameObject.SetActive(false);
#endif

#if UNITY_EDITOR
        AndroidUI_.gameObject.SetActive(false);
        UIControl_.gameObject.SetActive(true);
#endif
        currLevel_ = 1; //Temporary default
        pData_ = GameObject.Find("Player").GetComponent<PlayerData>();
        StartLevel();
	}
	
	void Update ()
    {
	    
	}

    void StartLevel()
    {
        UIControl_.BulliesNeeded = Constants.BULLY_LEVEL_REQUIREMENT + currLevel_;
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

    //This function is called whenever you want to increase the gas level on the UI
    public void IncreaseGasLevel(float val)
    {
        UIControl_.GasLevel += val;
    }

    void EndLevel()
    {
        pData_.UnlockNextLevel();
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
            sData.m_LevelsUnlocked = pData_.m_LevelsUnlocked;
            sData.m_UnlockedProjectileCombos = pData_.m_UnlockedProjectileCombos;
            sData.m_UnlockedMeleeCombos = pData_.m_UnlockedMeleeCombos;
            sData.m_UnlockedCombinedCombos = pData_.m_UnlockedCombinedCombos;

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
            sData.m_LevelsUnlocked = pData_.m_LevelsUnlocked;
            sData.m_UnlockedProjectileCombos = pData_.m_UnlockedProjectileCombos;
            sData.m_UnlockedMeleeCombos = pData_.m_UnlockedMeleeCombos;
            sData.m_UnlockedCombinedCombos = pData_.m_UnlockedCombinedCombos;

            bf.Serialize(file, sData);
            file.Close();
        }
    }

    void Load()
    {
        if(pData_ == null)
        {
            Debug.LogError("PlayerData is null, cannot load.");
            return;
        }
        if(File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "fuSaveData.dat"))
        {
            Debug.Log("Loading from " + Application.persistentDataPath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar + "fuSaveData.dat", FileMode.Open);
            SaveData sData = (SaveData)bf.Deserialize(file);

            //Insert load code here
            pData_.m_LevelsUnlocked = sData.m_LevelsUnlocked;
            pData_.m_UnlockedProjectileCombos = sData.m_UnlockedProjectileCombos;
            pData_.m_UnlockedMeleeCombos = sData.m_UnlockedMeleeCombos;
            pData_.m_UnlockedCombinedCombos = sData.m_UnlockedCombinedCombos;

            file.Close();
        }
        else
        {
            Debug.Log("Failed to load, file doesn't exist");
        }
    }
    #endregion
}