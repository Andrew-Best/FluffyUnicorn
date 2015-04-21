using UnityEngine;
using System.Collections;

public class EnemyProjectileScript : MonoBehaviour 
{
	public GameObject projectile;
	public int VX = -2;

	public virtual void InitProjectile(int startX, int startY)
	{
		this.transform.position = new Vector2(startX, startY);
	//	this.GetComponent

	}

}
