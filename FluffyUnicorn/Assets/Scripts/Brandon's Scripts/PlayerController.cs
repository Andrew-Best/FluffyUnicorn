using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    #region public variables
    public GameController m_GameControl;    //Game Controller object
    public GameObject m_SpawnPoint;         //projectile spawn point
    public GameObject[] m_TargetPoints;     //where the player will move to when switching tracks
    public Collider2D[] m_Tracks;           //tracks the player will switch to 

    public string m_ProjectileName = "PlayerProjectile";

    public float m_MaxSpeed = 5.0f;
    public float m_Acceleration = 1.0f;
    public float m_ShotSpeed = 10.0f;
    public float m_FireRate = 1.0f;
    public float m_Deceleration = 1.0f;

    public int m_PlayerHealth = 3;
    public int m_PlayerDamage = 1;
    public int m_Currency = 0;
    public int m_CurrencyScalar = 1;        //use to determine how much curency the player gains 

    //used to keep track of what track the player is on
    public bool m_onFrontTrack = true;
    public bool m_onMiddleTrack = false;
    public bool m_onLastTrack = false;
    #endregion

    #region private variables
    private GameObject player_;

    private float horizontalMove_;
    private float verticalMove_;
    private float nextFire_;
    private float timer_;               //timer to count how long the player has lifted the key. This determines if he is idle or just switching directions
    private float idleTime_ = 0.1f;     //variable used with the timer to determine if the player is idle 
    private float trackTimer_;
    private float trackTime = 0.1f;

    private bool facingRight_ = true;
    private bool isMoving_ = false;
    private bool canSwitchTracks = true;
    private bool buttonHeld_ = false;

    private Rigidbody2D playerRigidBody_;

    private Animator playerAnimator_;
    #endregion
    

    void SetValues()
    {
        player_ = GameObject.Find("Player");
        playerRigidBody_ = player_.GetComponent<Rigidbody2D>();
        playerAnimator_ = player_.GetComponent<Animator>();
    }

    void Start()
    {
        SetValues();
    }

    void FixedUpdate()
    {
        UpdateControls();
        //ChangeTrack();
    }

    void Update()
    {
        UpdateMoveTimer();
        UpdateTrackTimer();
    }

    void UpdateMoveTimer()
    {
        //when a horizontal key is no longer pressed start a timer to determine if the player is stopped or not. 
        //Otherwise he will switch to idle then start running if you change directions so it looks weird. 
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            timer_ += Time.deltaTime;
        }
        if (timer_ >= idleTime_)
        {
            isMoving_ = false;
            timer_ = 0.0f;
        }
    }

    void UpdateTrackTimer()
    {
        //when a vertical key is no longer pressed start a timer to determine if the player is on the next track before allowing another key press
        if (Input.GetAxisRaw("Vertical") == 0)
        {
            trackTimer_ += Time.deltaTime;
        }
        if (trackTimer_ >= trackTime)
        {
            canSwitchTracks = true;
            trackTimer_ = 0.0f;
        }
    }

    #region Controls
    void UpdateControls()
    {
        //Set move_ to be the horizontal axis keys
        //horizontalMove_ = Input.GetAxis("Horizontal");
        playerAnimator_.SetFloat("Speed", Mathf.Abs(horizontalMove_));
        //player_.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalMove_ * m_MaxSpeed, player_.GetComponent<Rigidbody2D>().velocity.y);
        if (horizontalMove_ > 0 || horizontalMove_ < 0)
        {
            isMoving_ = true;
        }
        playerAnimator_.SetBool("IsMoving", isMoving_);
        //check if player is moving right or left and flip sprite. 1 is right, -1 is left
        if (horizontalMove_ > 0 && !facingRight_)
        {
            Flip();
        }
        else if (horizontalMove_ < 0 && facingRight_)
        {
            Flip();
        }
        //Attack if you press space and you're not in cooldown
        if (Input.GetKeyUp(KeyCode.Space) && Time.time > nextFire_)
        {
            Attack();
        }
    }

    public void OnPointerDown()
    {
        buttonHeld_ = true;
    }

    public void OnPointerUp()
    {
        buttonHeld_ = false;
        horizontalMove_ = 0.0f;
        isMoving_ = false;
    }

    public void MoveLeft()
    {
        if (buttonHeld_)
        {
            horizontalMove_ = -1.0f;
            player_.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalMove_ * m_MaxSpeed, player_.GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    public void MoveRight()
    {
        if (buttonHeld_)
        {
            horizontalMove_ = 1.0f;
            player_.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalMove_ * m_MaxSpeed, player_.GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    public void MoveUp()
    {
        verticalMove_ = 1.0f;
        ChangeTrack();
    }

    public void MoveDown()
    {
        verticalMove_ = -1.0f;
        ChangeTrack();
    }
    #endregion

    public void Attack()
    {
        nextFire_ = Time.time + m_FireRate;
        //Get a bullet from the ObjectPool
        GameObject bullet = ObjectPool.Instance.GetObjectForType(m_ProjectileName, true);
        bullet.transform.position = m_SpawnPoint.transform.position;
        //Determine which direction to fire in
        if (facingRight_)
        {
            bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(m_ShotSpeed, 0, 0));
        }
        else
        {
            bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(-m_ShotSpeed, 0, 0));
        }
    }

    void Flip()
    {
        facingRight_ = !facingRight_;
        // Multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void ChangeTrack()
    {
        //verticalMove_ = Input.GetAxisRaw("Vertical");
        //check if the player pressed an up key and determine which track to move to. 
        if(verticalMove_ > 0)
        {
            //if the player switches tracks, put the player on the track's target point, disable collison on the previous track and enable collison on the new track
            if (m_onFrontTrack && canSwitchTracks)
            {        
                canSwitchTracks = false;
                player_.GetComponent<Rigidbody2D>().transform.position = new Vector3(player_.transform.position.x, m_TargetPoints[1].transform.position.y, m_TargetPoints[1].transform.position.z);
                m_Tracks[0].enabled = false;         
                m_Tracks[1].enabled = true;
                m_onFrontTrack = false;
                m_onMiddleTrack = true;
            }
            else if (m_onMiddleTrack && canSwitchTracks)
            {
                canSwitchTracks = false;
                player_.GetComponent<Rigidbody2D>().transform.position = new Vector3(player_.transform.position.x, m_TargetPoints[2].transform.position.y, m_TargetPoints[2].transform.position.z);
                m_Tracks[2].enabled = true;
                m_Tracks[1].enabled = false;
                m_onMiddleTrack = false;
                m_onLastTrack = true;
            }
        }
        //check if the player pressed an down key and determine which track to move to.
        if(verticalMove_ < 0)
        {
            //if the player switches tracks, put the player on the track's target point, disable collison on the previous track and enable collison on the new track
            if (m_onLastTrack && canSwitchTracks)
            {
                canSwitchTracks = false;
                player_.GetComponent<Rigidbody2D>().transform.position = new Vector3(player_.transform.position.x, m_TargetPoints[1].transform.position.y, m_TargetPoints[1].transform.position.z);
                m_Tracks[1].enabled = true;
                m_Tracks[2].enabled = false;
                m_onLastTrack = false;
                m_onMiddleTrack = true;
            }
            else if (m_onMiddleTrack && canSwitchTracks)
            {
                canSwitchTracks = false;
                player_.GetComponent<Rigidbody2D>().transform.position = new Vector3(player_.transform.position.x, m_TargetPoints[0].transform.position.y, m_TargetPoints[0].transform.position.z);
                m_Tracks[0].enabled = true;
                m_Tracks[1].enabled = false;
                m_onMiddleTrack = false;
                m_onFrontTrack = true;
            }
        }
    }

    #region Collision
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Bean")
        {
            m_GameControl.m_UIControl.GasLevel += Constants.BEAN_VALUE;
            Destroy(other.gameObject);
        }
    }
    #endregion
}