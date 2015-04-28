using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    public float m_Damage;
    public string m_Enemy = "";

    private GameObject player_;

    void Start()
    {
        player_ = GameObject.Find("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        m_Damage = player_.GetComponent<PlayerController>().m_PlayerDamage;
        DestructableObject hitObject;
        if (other.tag == m_Enemy)
        {
            //do stuff
        }
        else if (other.tag == "Trashcan")
        {
            ObjectPool.Instance.PoolObject(this.gameObject);
            hitObject = other.gameObject.GetComponentInChildren<DestructableObject>();
            hitObject.Destroy(m_Damage);
            hitObject.Wiggle();
        }
    }
}
