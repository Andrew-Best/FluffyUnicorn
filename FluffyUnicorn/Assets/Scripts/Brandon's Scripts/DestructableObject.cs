using UnityEngine;
using System.Collections;

public class DestructableObject : MonoBehaviour
{
    #region public variables
    public GameObject m_SpawnPoint;

    public float m_Health = 0;
    public float m_ReactForce = 0.4f;

    public bool m_HasParticleEffect = false;

    public string m_ParticleName = "";
    #endregion 

    #region private variables
    private GameObject player_;
    private GameObject upgradeManager_;
    private Animator objectAnimator_;
    private Vector3 deadPos_;

    private Vector3 startPos_;

    private bool dead_ = false;
    private bool isDamaged_ = false;
    private bool canSetDeadPos_ = true;

    private float damage;
    #endregion

    void Start()
    {
        player_ = GameObject.FindGameObjectWithTag("Player");
        upgradeManager_ = GameObject.FindGameObjectWithTag("UpgradeManager");
        objectAnimator_ = this.GetComponent<Animator>();
        damage = m_Health / 2;
        startPos_ = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
    }

    void Update()
    {
        //this.transform.position = startPos_;//new Vector3(this.transform.position.x, startPos_.y, this.transform.position.z);
        UpdateAnimationValues();
        SetDeadPos();
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

    void SetDeadPos()
    {
        //if the object is dead then get the current position and assign it to the deadPos var and then set the object's position to the deadPos to keep it in its dead position
        if (dead_)
        {
            //destroys the colliders and rigidbody so it doesn't move after the object has been destoryed 
            Destroy(this.GetComponent<Rigidbody2D>());
            if (this.GetComponent<BoxCollider2D>() != null)
            {
                Destroy(this.GetComponent<BoxCollider2D>());
            }
            if (this.GetComponent<CircleCollider2D>() != null)
            {
                Destroy(this.GetComponent<CircleCollider2D>());
            }
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
            player_.GetComponent<PlayerData>().m_PlayerHealth += health;
            //if the player's health is past the max health cap, set it back to the highest value it is allow to be at
            if (player_.GetComponent<PlayerData>().m_PlayerHealth > Constants.MAX_PLAYER_HEALTH)
            {
                player_.GetComponent<PlayerData>().m_PlayerHealth = Constants.MAX_PLAYER_HEALTH;
            }
        }
        else if(num == 2)
        {
        
            //increase the player's currency by the random number generated * the player's currency scalar 
            int currency = Random.Range(currencyMin, currencyMax);
            player_.GetComponent<PlayerData>().m_Currency += currency * player_.GetComponent<PlayerData>().m_CurrencyScalar;    
        }
        else
        {
            //do nothing. no item found 
        }
    }

    public bool HasDied()
    {
        //gets the living state of the object
        if(dead_)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
