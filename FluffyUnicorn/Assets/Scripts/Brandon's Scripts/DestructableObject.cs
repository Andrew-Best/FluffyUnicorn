using UnityEngine;
using System.Collections;

public class DestructableObject : MonoBehaviour 
{
    public float m_Health = 0;

    private GameObject player_;

    void Start()
    {
        player_ = GameObject.FindGameObjectWithTag("Player");
    }

    public void Destroy(float damage)
    {
        m_Health -= damage;
        if (m_Health <= 0)
        {
            ObjectPool.Instance.PoolObject(this.gameObject);
        }
    }
}
