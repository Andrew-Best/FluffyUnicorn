using UnityEngine;
using System.Collections;

public class Beaker : MonoBehaviour 
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            gameObject.transform.position = new Vector2(100.0f, 0.0f);
        }

        if (collision.collider.tag == "PlayerProjectile")
        {
            gameObject.transform.position = new Vector2(100.0f, 0.0f);
        }
    }

}
