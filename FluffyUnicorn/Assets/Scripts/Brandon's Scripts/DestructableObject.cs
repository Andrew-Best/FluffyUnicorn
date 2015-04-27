using UnityEngine;
using System.Collections;

public class DestructableObject : MonoBehaviour
{
    #region public variables
    public GameObject m_SpawnPoint;

    public float m_Health = 0;

    public bool m_HasParticleEffect = false;

    public string m_ParticleName = "";
    #endregion 

    #region private variables
    private GameObject player_;
    private Animator objectAnimator_;

    private bool dead_ = false;
    private bool isDamaged_ = false;

    private float damage;
    #endregion

    void Start()
    {
        player_ = GameObject.FindGameObjectWithTag("Player");
        objectAnimator_ = this.GetComponent<Animator>();
        damage = m_Health / 2;
    }

    void Update()
    {
        UpdateAnimationValues();
    }

    public void Destroy(float damage)
    {
        m_Health -= damage;
        if (m_Health <= 0)
        {
            dead_ = true;
            RandomItem();
        }
        if (m_HasParticleEffect && !dead_)
        {
            MoveParticle();
        }     
    }

    void UpdateAnimationValues()
    {
        if(m_Health <= damage)
        {
            isDamaged_ = true;
        }
        objectAnimator_.SetBool("isdead", dead_);
        objectAnimator_.SetBool("damaged", isDamaged_);
    }

    void MoveParticle()
    {
        GameObject particle = ObjectPool.Instance.GetObjectForType(m_ParticleName, true);
        particle.transform.position = m_SpawnPoint.transform.position;
        particle.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(0, 1, 0));
    }

    void RandomItem()
    {

    }
}
