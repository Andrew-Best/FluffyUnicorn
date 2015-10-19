using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private Transform player_;
    private GameController gc_;

    void Start()
    {
        gc_ = Camera.main.GetComponent<GameController>();
        player_ = GameObject.Find("Player").GetComponent<Transform>();
    }

    void LateUpdate()
    {
        if(!gc_.m_BossFight)
        {
            transform.position = new Vector3(player_.position.x, 2, -7);
        }
    }
}
