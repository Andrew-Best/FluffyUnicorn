using UnityEngine;
using System.Collections;

public class BullyCountdown : MonoBehaviour 
{
    public GameObject m_Jocky;

    private float activate_;

	
	void Start () 
    {
        activate_ = 10.0f;
        
	}
	
	
	void Update ()
    {
        activate_ -= Time.deltaTime;
        if(activate_ <= 0.0f)
        {
            m_Jocky.SetActive(true);
        }
	}
}
