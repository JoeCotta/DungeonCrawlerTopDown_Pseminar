using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /* needs more testing
        if (player.gameObject.GetComponent<Player>().isDashing)
        {
            smoothSpeed = 0.8f;
        }
        else
        {
            smoothSpeed = 0.125f;
        }
        */
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position ,desiredPosition ,smoothSpeed);
        transform.position = smoothPosition;
    }
}
