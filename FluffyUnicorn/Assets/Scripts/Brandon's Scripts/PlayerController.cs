using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour 
{
    public GameObject m_SpawnPoint;
    private GameObject player_;

    public string m_ProjectileName = "PlayerProjectile";

    public float m_MaxSpeed = 5.0f;
    public float m_Acceleration = 1.0f;
    public float m_ShotSpeed = 10.0f;
    public float m_FireRate = 1.0f;
    public float m_Deceleration = 1.0f;

    public float move_;
    private float nextFire_;

    private bool facingRight_ = true;

    private Rigidbody2D playerRigidBody_;

    private Animator playerAnimator_;

    void SetValues()
    {    
        player_ = GameObject.Find("Player");
        playerRigidBody_ = player_.GetComponent<Rigidbody2D>();
        playerAnimator_ = player_.GetComponent<Animator>();
    }

	void Start () 
    {
        SetValues();
	}

    void FixedUpdate()
    {
        UpdateControls();
    }

    void UpdateControls()
    {
        //Set move_ to be the horizontal axis keys
        move_ = Input.GetAxis("Horizontal");
        playerAnimator_.SetFloat("Speed", Mathf.Abs(move_));
        player_.GetComponent<Rigidbody2D>().velocity = new Vector2(move_ * m_MaxSpeed, player_.GetComponent<Rigidbody2D>().velocity.y);
         
        //check if player is moving right or left and flip sprite. 1 is right, -1 is left
        if (move_ > 0 && !facingRight_)
        {
            Flip();     
        }
        else if(move_ < 0 && facingRight_)
        {
            Flip();
        }
        //Attack if you press space and you're not in cooldown
        if (Input.GetKeyUp(KeyCode.Space) && Time.time > nextFire_)
        {
            Attack();
        }
    }
    
    void Attack()
    {
        nextFire_ = Time.time + m_FireRate;
        //Get a bullet from the ObjectPool
        GameObject bullet = ObjectPool.Instance.GetObjectForType(m_ProjectileName, true);
        bullet.transform.position = m_SpawnPoint.transform.position;
        //Determine which direction to fire in
        if(facingRight_)
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
}
