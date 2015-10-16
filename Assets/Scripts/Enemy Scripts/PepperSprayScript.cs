using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PepperSprayScript : MonoBehaviour 
{
//	new List<GameObject> bullets;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			Destroy(this.gameObject);
			//Harm player
		}
	}
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
