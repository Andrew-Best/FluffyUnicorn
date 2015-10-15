using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JockCollision : MonoBehaviour 
{
    private RefereeBully Ref_;
    private List<GameObject> m_JockHorde = new List<GameObject>();
	
	void Start () 
    {
        Ref_ = GameObject.FindGameObjectWithTag("Referee").GetComponent<RefereeBully>();
        m_JockHorde = Ref_.m_JockHorde;
	}
	
	void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            for (int i = 0; i < m_JockHorde.Count; ++i)
            {
                m_JockHorde[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            }
        }
    }
}
