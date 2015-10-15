using UnityEngine;
using System.Collections;

public class Beaker : MonoBehaviour 
{
    //private Rigidbody2D Rigid_;
    public AudioClip m_Break;

    void Start()
    {
        //Rigid_ = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            gameObject.transform.position = new Vector2(100.0f, 0.0f);
            AudioSource.PlayClipAtPoint(m_Break, transform.position);
        }
 
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerProjectile")
        {
            Vector2 Up = new Vector2(0.0f, 600.0f);
            gameObject.GetComponent<Rigidbody2D>().AddForce(Up);
        }
    }

}
