using UnityEngine;
using System.Collections;

[System.Serializable]
public class SaveData
{
    //This is where the save data will be held, there is nothing to save at the moment
    //Once there is, this file will be populated with said data.
    public int m_LevelsUnlocked;
    public int m_StagesUnlocked;

    public int m_PlayerHealth;
    public int m_PlayerDamage;
    public int m_Currency;
    public int m_CurrencyScalar;

    public bool[] m_UnlockedMeleeCombos = new bool[3];
    public bool[] m_UnlockedProjectileCombos = new bool[3];
    public bool[] m_UnlockedCombinedCombos = new bool[3];
}