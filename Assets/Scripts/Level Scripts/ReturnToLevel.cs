using UnityEngine;
using System.Collections;

public class ReturnToLevel : MonoBehaviour 
{
    /// <summary>Name of the door to teleport to</summary>
    public string m_LevelDoor;
    /// <summary>Door on the main level</summary>
    private GameObject levelDoor_;
    /// <summary>Player object</summary>
    private GameObject player_;
    /// <summary>Offset the player by this Vector when moving them to a secret area</summary>
    private Vector3 doorOffset_;

    void Start()
    {
        doorOffset_ = new Vector3(0.0f, 0.0f, 1.0f);
        levelDoor_ = GameObject.Find(m_LevelDoor);
        player_ = GameObject.Find("Player");
    }

    public void EnterArea()
    {
        player_.transform.position = levelDoor_.transform.position - doorOffset_;
    }

    void OnTriggerEnter(Collider other)
    {
        //switch back to level when you reach the exit area
        if (other.tag == "Player")
        {
            if(Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
            {
                EnterArea();
            }
        }
    }
}
