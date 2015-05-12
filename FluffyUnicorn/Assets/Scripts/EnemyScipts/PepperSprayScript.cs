using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PepperSprayScript : MonoBehaviour 
{
//	new List<GameObject> bullets;

	void OnTriggerEnter2D(Collider2D collision)
	{
	//	List<GameObject> shots = GetComponent<UniqueAttackScript>().bullets;

		if (collision.tag == "Player")
		{
			Destroy(this.gameObject);

			/*for (int i = 0; i < shots.Count; ++i)
			{
				GetComponent<UniqueAttackScript>().bullets.Remove(GetComponent<UniqueAttackScript>().bullets[i].gameObject);
			}*/
			//Harm player
		}

/*		for (int i = 0; i < shots.Count; ++i)
		{
			if (collision.tag == "Player")
			{
				Destroy(this.gameObject);
				GetComponent<UniqueAttackScript>().bullets.Remove(GetComponent<UniqueAttackScript>().bullets[i].gameObject);
				//Harm player
			}
		}*/
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
