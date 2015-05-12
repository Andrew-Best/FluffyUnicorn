using UnityEngine;
using System.Collections;

public class PepperSprayScript : MonoBehaviour 
{
	void OnCollisionEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			Destroy(this.gameObject);//destroy bullet
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
