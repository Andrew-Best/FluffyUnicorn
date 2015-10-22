using UnityEngine;
using System.Collections;

public class DoorCollision : MonoBehaviour
{
    private SecretArea secretArea_;

	void Start ()
    {
        secretArea_ = transform.parent.gameObject.GetComponent<SecretArea>();
	}

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            secretArea_.Collision(gameObject.name);
        }
    }
}
