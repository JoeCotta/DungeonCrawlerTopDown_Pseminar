using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Vector3 desiredPos = transform.position;
        Vector3 movementDir = new Vector3();
        if (Input.GetKey("w")) movementDir += transform.up;
        if (Input.GetKey("s")) movementDir -= transform.up;
        if (Input.GetKey("d")) movementDir += transform.right;
        if (Input.GetKey("a")) movementDir -= transform.right;
        desiredPos += movementDir.normalized * Time.deltaTime * speed;
        rb.MovePosition(desiredPos);
    }
}
