using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private GameObject player_;
    private GameController gc_;
    private bool positionSet_ = false;

    void Start()
    {
        gc_ = Camera.main.GetComponent<GameController>();
        player_ = GameObject.Find("Player");
    }

    void LateUpdate()
    {
        if(!gc_.m_BossFight)
        {
            if(player_.GetComponent<PlayerController>().InSecretArea)
            {
                if(!positionSet_)
                {
                    transform.position = new Vector3(player_.transform.position.x, 2, 19);
                    positionSet_ = true;
                }
            }
            else
            {
                transform.position = new Vector3(player_.transform.position.x, 2, -7);
            }
        }
    }
}
