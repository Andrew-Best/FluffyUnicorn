﻿using UnityEngine;
using System.Collections;

public class DestructableObject : MonoBehaviour
{
    #region public variables
    public GameObject m_SpawnPoint;

    public float m_Health = 0;
    public float m_ReactForce = 0.4f;

    public bool m_HasParticleEffect = false;
    public bool m_ReactToDamage = true;

    public string m_ParticleName = "";
    #endregion 

    #region private variables
    private GameObject player_;
    private GameObject upgradeManager_;
    private Animator objectAnimator_;

    private bool dead_ = false;
    private bool isDamaged_ = false;

    private float damage;
    #endregion

    void Start()
    {
        player_ = GameObject.FindGameObjectWithTag("Player");
        upgradeManager_ = GameObject.FindGameObjectWithTag("UpgradeManager");
        objectAnimator_ = this.GetComponent<Animator>();
        damage = m_Health / 2;

    }

    void Update()
    {
        UpdateAnimationValues();
    }

    #region DestroyObject
    public void Destroy(float damage)
    {
        m_Health -= damage;
        if (m_Health <= 0)
        {
            if (!dead_)
            {
                RandomItem(1, 4);   //go one number past the one you want for the max
            }
            dead_ = true;
        }
        if (m_HasParticleEffect && !dead_)
        {
            MoveParticle();
        }
    }

    void UpdateAnimationValues()
    {
        if (m_Health <= damage)
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

    public void Wiggle()
    {
        //if you want the object to move when hit and it hasn't been destroyed
        if (!dead_ && m_ReactToDamage)
        {
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(m_ReactForce, 0.0f));
        }
    }
    #endregion
    
    void RandomItem(int min, int max)
    {
        int currencyMin = 1, currencyMax = 15, healthMin = 1, healthMax = 3;        //min and max for the different item random drops
        int num = Random.Range(min, max);
        if(num == 1)
        {
            //increase the player's health by the number generated
            int health = Random.Range(healthMin, healthMax);
            player_.GetComponent<PlayerController>().m_PlayerHealth += health;
            //if the player's health is past the max health cap, set it back to the highest value it is allow to be at
            if( player_.GetComponent<PlayerController>().m_PlayerHealth > upgradeManager_.GetComponent<UpgradeManager>().m_MaxHealth)
            {
                player_.GetComponent<PlayerController>().m_PlayerHealth = upgradeManager_.GetComponent<UpgradeManager>().m_MaxHealth;
            }
        }
        else if(num == 2)
        {
        
            //increase the player's currency by the random number generated * the player's currency scalar 
            int currency = Random.Range(currencyMin, currencyMax);
            player_.GetComponent<PlayerController>().m_Currency += currency * player_.GetComponent<PlayerController>().m_CurrencyScalar;    
        }
        else
        {
            //do nothing. no item found 
        }
    }
}