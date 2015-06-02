using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    #region public variables
    public GameObject m_SpawnPoint;         //projectile spawn point
    public string m_ProjectileName = "PlayerProjectile";
    public string m_ProjectileName2 = "PlayerProjectile2";
    public string m_ProjectileName3 = "PlayerProjectile3";
    //used to keep track of what track the player is on
    public bool m_onFrontTrack = true;
    public bool m_onMiddleTrack = false;
    public bool m_onLastTrack = false;
    public bool m_IsHitting = false;     //determine if the player is using his physical attack

    public bool m_UpdateMelee = false;
    public bool m_UpdateProjectile = false;

    public BoxCollider2D m_MeleeCollider;
    #endregion

    #region private variables
    private GameObject player_;
    private GameObject startPosition_;

    private PlayerData playerData_;         //Player data script
    private Rigidbody2D rigidBody_;
    private GameController gameControl_;    //Game Controller object

    private float horizontalMove_;
    private float verticalMove_;
    private float timer_;               //timer to count how long the player has lifted the key. This determines if he is idle or just switching directions
    private float idleTime_ = 0.1f;     //variable used with the timer to determine if the player is idle 
    private float trackTimer_;
    private float trackTime = 0.1f;

    private bool facingRight_ = true;
    private bool isMoving_ = false;
    private bool canSwitchTracks = true;
    private bool buttonHeld_ = false;

    private List<GameObject> targetPoints_ = new List<GameObject>();     //where the player will move to when switching tracks
    private List<Collider2D> tracks_ = new List<Collider2D>();           //tracks the player will switch to 

    private Rigidbody2D playerRigidBody_;

    private Animator playerAnimator_;
    private BoxCollider2D playerBoxCollider_;
    #endregion

    #region Combo variables
    public enum ComboType
    {
        IDLE = 0,
        COMBOHIT1,
        COMBOHIT2,
        COMBOHIT3
    };

    public float m_ComboTimerLength = 4.0f;                     //how long you have between combos to move onto the next one
    public float m_PhysicalDamage = 1.0f;                       //melee attack damage
    public float[] m_PhysicalDamageIncreases = new float[4];    //as physical combos go up so will the physical damage
    public ComboType m_CurrentComboState;                       //current combo state

    //used for upgrades so player can buy different combos
    public bool[] m_UnlockedMeleeCombos = new bool[3];
    public bool[] m_UnlockedProjectileCombos = new bool[3];
    public bool[] m_UnlockedCombinedCombos = new bool[3];

    private bool activateComboTimerReset_ = false;      //combo timer reset boolean 
    private bool[] projectileCombo_ = new bool[3];      //array of bools for the projectiles combos
    private bool[] meleeCombo = new bool[3];            //array of bools for the melee combos          
    private bool[] combinedCombos = new bool[3];        //array of bools for the combined combos
    private bool canUseCombo_ = false;                  //if there is a combined combo this becomes true

    private int projectileChain_ = 0;   //combo chain for projectiles 
    private int meleeChain_ = 0;        //combo chain for melee

    private float comboAnimationTimer_ = 0.0f;  //keep track of how long it's been since the next state of the combo has happened 
    private float comboTimer_ = 4.0f;           //amount of time you have to reach next combo
    private float nextFire_;                    //next time you can use a projectile attack
    private float nextMeleeAttack_;             //next time you can use a melee attack
    #endregion

    public void SetValues()
    {
        targetPoints_.Clear();
        tracks_.Clear();
        player_ = GameObject.Find("Player");
        rigidBody_ = GetComponent<Rigidbody2D>();
        playerData_ = GetComponent<PlayerData>();
        playerRigidBody_ = player_.GetComponent<Rigidbody2D>();
        playerRigidBody_.velocity = new Vector2(0.0f, 0.0f);
        horizontalMove_ = 0.0f;
        verticalMove_ = 0.0f;
        playerAnimator_ = player_.GetComponent<Animator>();
        playerBoxCollider_ = player_.GetComponent<BoxCollider2D>();
        gameControl_ = GameObject.Find("Main Camera").GetComponent<GameController>();
        for (int i = 0; i < 3; ++i)
        {
            targetPoints_.Add(GameObject.FindGameObjectWithTag("Targetpoint" + i));
            tracks_.Add(GameObject.FindGameObjectWithTag("Track" + i).GetComponent<Collider2D>());
        }        
        comboTimer_ = m_ComboTimerLength;
        rigidBody_.transform.position = new Vector3(targetPoints_[0].transform.position.x, targetPoints_[0].transform.position.y, targetPoints_[0].transform.position.z);
    }

    void OnLevelWasLoaded(int level)
    {
        SetValues();
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
        ComboSystem();              //controls the combo system
        UpdateComboAnimations();    //update combo animations
        ResetComboAnimations(m_ComboTimerLength);
        UpdateMoveTimer();
        UpdateTrackTimer();
        ResetComboState(activateComboTimerReset_);
        EnableMeleeCollision();
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
        /*//Attack if you press space and you're not in cooldown
        if (Input.GetKeyUp(KeyCode.Space) && Time.time > nextFire_)
        {
            Attack();
        }*/
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
            player_.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalMove_ * playerData_.m_MaxSpeed, player_.GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    public void MoveRight()
    {
        if (buttonHeld_)
        {
            horizontalMove_ = 1.0f;
            player_.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalMove_ * playerData_.m_MaxSpeed, player_.GetComponent<Rigidbody2D>().velocity.y);
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
        if (verticalMove_ > 0)
        {
            //if the player switches tracks, put the player on the track's target point, disable collison on the previous track and enable collison on the new track
            if (m_onFrontTrack && canSwitchTracks)
            {
                canSwitchTracks = false;
                //rigidBody_.transform.Translate(new Vector3(player_.transform.position.x, targetPoints_[1].transform.position.y, targetPoints_[1].transform.position.z) * Time.deltaTime);
                rigidBody_.transform.position = new Vector3(player_.transform.position.x, targetPoints_[1].transform.position.y, targetPoints_[1].transform.position.z);
                this.gameObject.layer = 20;
               // tracks_[0].enabled = false;
                //tracks_[1].enabled = true;
                m_onFrontTrack = false;
                m_onMiddleTrack = true;
            }
            else if (m_onMiddleTrack && canSwitchTracks)
            {
                canSwitchTracks = false;
                //rigidBody_.transform.Translate(new Vector3(player_.transform.position.x, targetPoints_[2].transform.position.y, targetPoints_[2].transform.position.z) * Time.deltaTime);
                rigidBody_.transform.position = new Vector3(player_.transform.position.x, targetPoints_[2].transform.position.y, targetPoints_[2].transform.position.z);
                this.gameObject.layer = 21;
                //tracks_[2].enabled = true;
              //  tracks_[1].enabled = false;
                m_onMiddleTrack = false;
                m_onLastTrack = true;
            }
        }
        //check if the player pressed an down key and determine which track to move to.
        if (verticalMove_ < 0)
        {
            //if the player switches tracks, put the player on the track's target point, disable collison on the previous track and enable collison on the new track
            if (m_onLastTrack && canSwitchTracks)
            {
                canSwitchTracks = false;
                rigidBody_.transform.position = new Vector3(player_.transform.position.x, targetPoints_[1].transform.position.y, targetPoints_[1].transform.position.z);
                //rigidBody_.transform.Translate(new Vector3(player_.transform.position.x, targetPoints_[1].transform.position.y, targetPoints_[1].transform.position.z) * Time.deltaTime);
                this.gameObject.layer = 20;
               // tracks_[1].enabled = true;
              //  tracks_[2].enabled = false;
                m_onLastTrack = false;
                m_onMiddleTrack = true;
            }
            else if (m_onMiddleTrack && canSwitchTracks)
            {
                canSwitchTracks = false;
                rigidBody_.transform.position = new Vector3(player_.transform.position.x, targetPoints_[0].transform.position.y, targetPoints_[0].transform.position.z);
               // rigidBody_.transform.Translate(new Vector3(player_.transform.position.x, targetPoints_[0].transform.position.y, targetPoints_[0].transform.position.z) * Time.deltaTime);
                this.gameObject.layer = 19;
              //  tracks_[0].enabled = true;
             //   tracks_[1].enabled = false;
                m_onMiddleTrack = false;
                m_onFrontTrack = true;
            }
        }
    }

    #region Collision
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bean")
        {
            gameControl_.IncreaseGasLevel(Constants.BEAN_VALUE);
            Destroy(other.gameObject);
        }
        else if (m_IsHitting)
        {
            m_IsHitting = false;
            if(other.tag == "Trashcan")
            {
                DestructableObject hitObject;
                hitObject = other.gameObject.GetComponentInChildren<DestructableObject>();
                if (!hitObject.HasDied())
                {
                    hitObject.Destroy(playerData_.m_PunchDamage);
                }
            }
            //attack enemies and do your thing
        }
    }
    #endregion

    #region Combos
    public void ComboSystem()
    {
        #region Projectile Attack
        if ( Input.GetKeyUp(KeyCode.Q) || m_UpdateProjectile && Time.time > nextFire_)
        {
            m_UpdateProjectile = false;
            switch (projectileChain_)
            {
                //set the last combo to false and turn the current one one and update all the proper variables to what they need to be
                case 0:
                    if (m_UnlockedProjectileCombos[0])
                    {
                        projectileCombo_[2] = false;
                        projectileCombo_[0] = true;
                        BuildCombos();
                        Attack();
                        projectileChain_++;
                        m_CurrentComboState = ComboType.COMBOHIT1;
                        activateComboTimerReset_ = true;    //starts a countdown timer to determine when to stop keeping track of the combo                   
                    }
                    break;
                case 1:
                    if (m_UnlockedProjectileCombos[1])
                    {
                        comboTimer_ = m_ComboTimerLength;
                        projectileCombo_[1] = true;
                        BuildCombos();
                        Attack();
                        projectileChain_++;
                        m_CurrentComboState = ComboType.COMBOHIT2;
                    }      
                    break;
                case 2:
                    if (m_UnlockedProjectileCombos[2])
                    {
                        comboTimer_ = m_ComboTimerLength;
                        projectileCombo_[1] = false;
                        projectileCombo_[2] = true;
                        BuildCombos();
                        Attack();
                        projectileChain_++;
                    }
            
                    break;
            }
            //check if a combo attack has been created
            ComboAttack();
        }
        #endregion

        #region Melee Attack
        else if (Input.GetKeyUp(KeyCode.W) || m_UpdateMelee && Time.time > nextMeleeAttack_)
        {
            m_UpdateMelee = false;
            switch (meleeChain_)
            {
                //set the last combo to false and turn the current one one and update all the proper variables to what they need to be
                case 0:
                    if (m_UnlockedMeleeCombos[0])
                    {
                        comboTimer_ = m_ComboTimerLength;
                        meleeCombo[2] = false;
                        meleeCombo[0] = true;
                        BuildCombos();
                        PhysicalAttack();
                        meleeChain_++;
                        activateComboTimerReset_ = true;    //starts a countdown timer to determine when to stop keeping track of the combo
                    }   
                    break;

                case 1:
                    if (m_UnlockedMeleeCombos[1])
                    {
                        comboTimer_ = m_ComboTimerLength;
                        meleeCombo[1] = true;
                        BuildCombos();
                        PhysicalAttack();
                        meleeChain_++;
                    }
                    break;

                case 2:
                    if (m_UnlockedMeleeCombos[2])
                    {
                        comboTimer_ = m_ComboTimerLength;
                        meleeCombo[1] = false;
                        meleeCombo[2] = true;
                        BuildCombos();
                        PhysicalAttack();
                        meleeChain_++;
                    }
                   
                    break;
            }
            //check if a combo attack has been created
            ComboAttack();
      
     
        }
        #endregion
    }

    void ResetComboState(bool resetName)
    {
        //if the bool that you pass to the method is true
        if (resetName)
        {
            comboTimer_ -= Time.deltaTime;
            //If the parameter bool is set to true, a timer starts, when the timer runs out
            //m_CurrentComboState is set back to IDLE.
            if (comboTimer_ <= 0)
            {
                m_CurrentComboState = ComboType.IDLE;
                projectileChain_ = 0;
                meleeChain_ = 0;
                activateComboTimerReset_ = false;
                m_IsHitting = false;
                //combo is over so set player back to idle by setting all combo bools to false
                for (int i = 0; i < projectileCombo_.Length; ++i)
                {
                    projectileCombo_[i] = false;
                }
                for (int i = 0; i < meleeCombo.Length; ++i)
                {
                    meleeCombo[i] = false;
                }
                for (int i = 0; i < combinedCombos.Length; ++i)
                {
                    combinedCombos[i] = false;
                }
                UpdateComboAnimations();
                comboTimer_ = m_ComboTimerLength;
            }
        }
    }

    void UpdateComboAnimations()
    {
        //set the animation bools to the corresponding combo bools
        playerAnimator_.SetBool("IsAttacking1", projectileCombo_[0]);
        playerAnimator_.SetBool("IsAttacking2", projectileCombo_[1]);
        playerAnimator_.SetBool("IsAttacking3", projectileCombo_[2]);
        playerAnimator_.SetBool("IsPunching1", meleeCombo[0]);
        playerAnimator_.SetBool("IsPunching2", meleeCombo[1]);
        playerAnimator_.SetBool("IsPunching3", meleeCombo[2]);
        playerAnimator_.SetBool("CombinedCombo1", combinedCombos[0]);
        playerAnimator_.SetBool("CombinedCombo2", combinedCombos[1]);
        playerAnimator_.SetBool("CombinedCombo3", combinedCombos[2]);
    }

    void ResetComboAnimations(float maxTime)
    {
        //Update animations every time the timer reaches the max time 
        comboAnimationTimer_ += Time.deltaTime;
        if (comboAnimationTimer_ >= maxTime)
        {
            UpdateComboAnimations();
            comboAnimationTimer_ = 0.0f;
        }
    }

    public void Attack()
    {
        nextFire_ = Time.time + playerData_.m_FireRate;
        //use the right projectile based on how far the projectile combo is in the chain
        if (projectileChain_ == 0)
        {
            //Get a bullet from the ObjectPool
            GameObject bullet = ObjectPool.Instance.GetObjectForType(m_ProjectileName, true);
            bullet.transform.position = m_SpawnPoint.transform.position;
            //Determine which direction to fire in
            if (facingRight_)
            {
                bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(playerData_.m_ShotSpeed, 0, 0));
            }
            else
            {
                bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(-playerData_.m_ShotSpeed, 0, 0));
            }
        }
        else if (projectileChain_ == 1)
        {
            //Get a bullet from the ObjectPool
            GameObject bullet = ObjectPool.Instance.GetObjectForType(m_ProjectileName2, true);
            bullet.transform.position = m_SpawnPoint.transform.position;
            //Determine which direction to fire in
            if (facingRight_)
            {
                bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(playerData_.m_ShotSpeed, 0, 0));
            }
            else
            {
                bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(-playerData_.m_ShotSpeed, 0, 0));
            }
        }
        else if (projectileChain_ >= 2)
        {
            //Get a bullet from the ObjectPool
            GameObject bullet = ObjectPool.Instance.GetObjectForType(m_ProjectileName3, true);
            bullet.transform.position = m_SpawnPoint.transform.position;
            //Determine which direction to fire in
            if (facingRight_)
            {
                bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(playerData_.m_ShotSpeed, 0, 0));
            }
            else
            {
                bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector3(-playerData_.m_ShotSpeed, 0, 0));
            }
        }
    }

    public void PhysicalAttack()
    {
        nextMeleeAttack_ = Time.time + playerData_.m_PunchRate;
        //set the physical damage to the appropriate variable based on where the melee state is at 
        if (meleeChain_ == 0)
        {
            m_IsHitting = true;
            m_PhysicalDamage = m_PhysicalDamageIncreases[0];  //reset attack damage back to default
        }
        else if (meleeChain_ == 1)
        {
            m_IsHitting = true;
            m_PhysicalDamage += m_PhysicalDamageIncreases[1];
        }
        else if (meleeChain_ == 2)
        {
            m_IsHitting = true;
            m_PhysicalDamage += m_PhysicalDamageIncreases[2];
        }
    }

    void ComboAttack()
    {
        //combo has been reached so use a physical and projectile attack
        if (canUseCombo_)
        {
            canUseCombo_ = false;
            comboTimer_ = m_ComboTimerLength;
            PhysicalAttack();
            Attack();
            activateComboTimerReset_ = true;    //starts a countdown timer to determine when to stop keeping track of the combo 
        }
    }

    void BuildCombos()
    {
        //for different animations for the combos
        //set states based on what bools are true and false;    
        if (projectileCombo_[0] == true && meleeCombo[0] == true && m_UnlockedCombinedCombos[0])
        {
            combinedCombos[0] = true;
            canUseCombo_ = true;
        }
        if (projectileCombo_[0] == true && meleeCombo[1] == true && m_UnlockedCombinedCombos[1])
        {
            combinedCombos[0] = false;
            combinedCombos[1] = true;
            canUseCombo_ = true;
        }
        if (projectileCombo_[1] == true && meleeCombo[0] == true && m_UnlockedCombinedCombos[2])
        {
            combinedCombos[1] = false;
            combinedCombos[2] = true;
            canUseCombo_ = true;
        }      
    }

    void EnableMeleeCollision()
    {
        if(m_IsHitting)
        {
            m_MeleeCollider.enabled = true;
        }
        else
        {
            m_MeleeCollider.enabled = false;
        }
    }
    #endregion
}