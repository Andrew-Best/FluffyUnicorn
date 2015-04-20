using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    public float m_Damage = 1.0f;
    public string m_Enemy = ""; 

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == m_Enemy)
        {
            //do stuff
        }
    }
}
