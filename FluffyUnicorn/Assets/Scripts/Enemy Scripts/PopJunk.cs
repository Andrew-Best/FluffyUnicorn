using UnityEngine;
using System.Collections;

public class PopJunk : MonoBehaviour 
{
    public GameObject m_Pop;

    private float death_;
    private bool isPoped_;

    void Start()
    {
        death_ = 6.0f;
        isPoped_ = false;
    }

    void Update()
    {
        if(isPoped_ == true)
        {
            death_ -= Time.deltaTime;
            if (death_ <= 0.0f)
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
