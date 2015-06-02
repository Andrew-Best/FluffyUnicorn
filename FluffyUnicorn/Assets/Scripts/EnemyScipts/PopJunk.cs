using UnityEngine;
using System.Collections;

public class PopJunk : MonoBehaviour 
{
    public GameObject m_Pop;

    private float Death_;
    private bool isPoped_;

    void Start()
    {
        Death_ = 6.0f;
        isPoped_ = false;
    }

    void Update()
    {
        if(isPoped_ == true)
        {
            Death_ -= Time.deltaTime;
            if(Death_ <= 0.0f)
            {
                m_Pop.SetActive(false);
                gameObject.transform.position = new Vector2(100.0f, 0.0f);
            }
        }
    }

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
            m_Pop.SetActive(true);
            isPoped_ = true;
        }
    }
}
