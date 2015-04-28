using UnityEngine;
using System.Collections;

public class EnemyProjectileScript : Projectile 
{
	public int VX = -2; // Constant to be added to constant files later

	public virtual void FireProjectile(GameObject bully)
	{
		float startX = bully.GetComponent<Rigidbody2D>().position.x;
		float startY = bully.GetComponent<Rigidbody2D>().position.y;

		this.transform.position = new Vector2(startX, startY);

	//	this.transform.position = new Vector2(startX, startY);
	//	this.GetComponent

	}

	public virtual void UpdateProjectile(GameObject projectile)
	{
	//	this.rigidbody2D.ve
	}


}
