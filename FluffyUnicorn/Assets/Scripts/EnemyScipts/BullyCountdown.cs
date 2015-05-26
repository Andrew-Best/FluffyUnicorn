using UnityEngine;
using System.Collections;

public class BullyCountdown : MonoBehaviour 
{
    public GameObject m_Jocky;

    private float Activate_;

	
	void Start () 
    {
        Activate_ = 10.0f;
        
	}
	
	
	void Update ()
    {
        Activate_ -= Time.deltaTime;
        if(Activate_ <= 0.0f)
        {
            m_Jocky.SetActive(true);
        }
	}
}
