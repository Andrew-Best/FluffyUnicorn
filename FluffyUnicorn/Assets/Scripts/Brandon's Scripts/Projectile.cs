using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    public float m_Damage;
    public string m_Enemy = "";

    private GameObject player_;

    void OnTriggerEnter(Collider other)
    {
        m_Damage = player_.GetComponent<PlayerController>().m_PlayerDamage;
        if (other.tag == m_Enemy)
        {
            //do stuff
        }
    }
}
