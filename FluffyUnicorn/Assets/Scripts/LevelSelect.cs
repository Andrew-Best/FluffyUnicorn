using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour
{
    public void ChooseLevel(GameObject go)
    {
        //Debug.Log(go.name);
        Application.LoadLevel(go.name);
    }
}
