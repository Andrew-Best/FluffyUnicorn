using UnityEngine;
using System.Collections;

public class PoolObject : MonoBehaviour {

    private float lifeTimer_ = 0.0f;
    public float m_MaxLifeTime = 0.0f;

    void Start()
    {
        lifeTimer_ = 0.0f;
    }

    void Update()
    {
        //Pools an object based on the Max Life Time Specified 
        lifeTimer_ += Time.deltaTime;
        if (lifeTimer_ >= m_MaxLifeTime)
        {
            lifeTimer_ = 0.0f;
            ObjectPool.Instance.PoolObject(this.gameObject);
        }
    }
}
