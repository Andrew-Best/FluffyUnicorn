using UnityEngine;
using System.Collections;

public class ReturnToLevel : MonoBehaviour 
{
    public string m_LevelName = "";

    public void EnterArea(string areaName)
    {
        //load scene based on name 
        Application.LoadLevel(areaName);
    }

    void OnTriggerEnter(Collider other)
    {
        //switch back to level when you reach the exit area
        if (other.tag == "Player")
        {
            EnterArea(m_LevelName);
        }
    }
}
