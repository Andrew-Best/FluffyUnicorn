using UnityEngine;
using System.Collections;

public class BoxJunk : MonoBehaviour 
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            //gameObject.transform.position = new Vector2(100.0f, 0.0f);
           
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerProjectile")
        {
            gameObject.transform.position = new Vector2(100.0f, 0.0f);
        }
    }
}
