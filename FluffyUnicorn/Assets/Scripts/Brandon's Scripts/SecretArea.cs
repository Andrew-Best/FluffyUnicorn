using UnityEngine;
using System.Collections;

public class SecretArea : MonoBehaviour
{
    #region public
 
    #endregion
    private GameObject player_;
    private GameObject enemySpawner_;
    #region private

    #endregion
    // Use this for initialization
	void Start ()
    {
        player_ = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
