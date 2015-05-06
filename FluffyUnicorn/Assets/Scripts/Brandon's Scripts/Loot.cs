using UnityEngine;
using System.Collections;
using System;

public class Loot : MonoBehaviour
{
    #region Public variables
    //item types. Add here for new items
    public enum m_Item
    {
        HEALTH = 0,
        MONEY
    };
    public m_Item m_ItemType;
    public PlayerController m_Player;

    //sound effect to be played when chest is opened 
    public AudioClip m_SFX; 

    //images for the different items
    public Sprite m_HealthSprite;
    public Sprite m_MoneySprite;

    //animations 
    public string m_AnimationName = "PlayFireWorks";
    public string m_ParticleName = "Firework";
    //item name
    public string m_ItemName = "";

    //how long you want the explosion to last when the chest is opened 
    public float m_ParticleLifeTime = 0.5f;

    //how many items you want to spawn when chest is opened 
    public int m_NumItems = 1;
    #endregion

    #region Private variables
    //only trigger chest once 
    private bool canGiveLoot_ = true;
    //check if you can play the finished animation 
    private bool playFinishedAnimation_ = false;
    //check if you can update particle effect
    private bool updateParticle_ = false;

    private float particleTimer_ = 0.0f;

    //animators and sprite renderers
    private Animator animator_;
    private Animator fireworkAnimator_;
    private SpriteRenderer fireworkRenderer_;
    #endregion

    void Start()
    {
        animator_ = this.GetComponent<Animator>();
    }

    void Update()
    {
        SetAnimationValues();   //update animation
        UpdateParticleTimer();  //update particles if allowed
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && canGiveLoot_)
        {
            canGiveLoot_ = !canGiveLoot_;
            AudioSource.PlayClipAtPoint(m_SFX, this.transform.position);
            PlayParticleEffect();
            playFinishedAnimation_ = true;
            //loop through the number of items you want to spawn
            for(int i = 0; i < m_NumItems; ++i)
            {
                //pool the item from the object pool and initialize and spawn the appropriate item
                GameObject item = ObjectPool.Instance.GetObjectForType(m_ItemName, true);
                SetItem(item.GetComponent<Item>());
                item.GetComponent<Item>().GiveItem();
            }            
        }
    }

    void SetAnimationValues()
    {
        animator_.SetBool("Finished", playFinishedAnimation_);
    }

    #region Particles
    void PlayParticleEffect()
    {
        //turn on renderers and animators and play the firework
        fireworkAnimator_ = GameObject.FindGameObjectWithTag(m_ParticleName).GetComponent<Animator>();
        fireworkRenderer_ = GameObject.FindGameObjectWithTag(m_ParticleName).GetComponent<SpriteRenderer>();
        fireworkRenderer_.enabled = true;
        fireworkAnimator_.enabled = true;
        fireworkAnimator_.Play(m_AnimationName);
        updateParticle_ = true;      
    }

    void UpdateParticleTimer()
    {
        //if the particle is playing increment a timer until it reaches the max time and then stop
        if (updateParticle_)
        {
            particleTimer_ += Time.deltaTime;
        }
        if(particleTimer_ >= m_ParticleLifeTime)
        {
            updateParticle_ = false;
            particleTimer_ = 0.0f;
            fireworkRenderer_.enabled = false;
            fireworkAnimator_.enabled = false;
        }
    }
    #endregion

    void SetItem(Item item)
    {
        //set the item based on what enum you picked in the inspector
        if(m_ItemType == m_Item.HEALTH)
        {
            item.MakeHealth();
            item.SetImage(m_HealthSprite);
        }
        else if(m_ItemType == m_Item.MONEY)
        {
            item.MakeMoney();
            item.SetImage(m_MoneySprite);
        }
    }
}
