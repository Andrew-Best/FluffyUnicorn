using UnityEngine;
using System.Collections;

public class FatBully : MonoBehaviour 
{

    public float m_rollSpeed;
    public float m_maxDistance;

    public float timeTraveled_;
	
	void Update () 
    {
        Roll();
        timeTraveled_ += Time.deltaTime;
        
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-1.0f, 0.0f);
        if (timeTraveled_ >= m_maxDistance)
        {
            InverseRoll();
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1.0f, 0.0f);
            
        }          
	}

    void Roll()
    {
        transform.Rotate(0.0f, 0.0f, m_rollSpeed * Time.deltaTime);        
    }

    void InverseRoll()
    {
        transform.Rotate(0.0f, 0.0f, -m_rollSpeed * Time.deltaTime);  
    }
}
