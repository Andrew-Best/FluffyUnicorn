using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private Transform player_;

    void Start()
    {
        player_ = GameObject.Find("Player").GetComponent<Transform>();
    }

    void LateUpdate()
    {
        transform.position = new Vector3(player_.position.x, 0, -10);
    }
}
