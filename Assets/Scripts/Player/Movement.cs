using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float maxMovementSpeed;
    public Rigidbody2D rb;
    public float acceleration;
    public Camera cam;

    private Vector2 inputMovement;
    private Vector2 mousePosition;
    private Vector2 lookDir;
    private float angleToMouse;

    void Start()
    {
        cam.orthographic = true;
    }

    void Update()
    {
        // getting Keyboard Input
        inputMovement = Input.GetKey("w") ? Vector2.up : Vector2.zero;
        inputMovement += Input.GetKey("a") ? Vector2.left : Vector2.zero;
        inputMovement += Input.GetKey("s") ? Vector2.down : Vector2.zero;
        inputMovement += Input.GetKey("d") ? Vector2.right : Vector2.zero;

        // getting mouse Input
        // transform the screen mouse Position to a world point
        mousePosition =  cam.ScreenToWorldPoint(Input.mousePosition);

        // calculating the angle between the player and the mouse
        lookDir = rb.position - mousePosition;
        angleToMouse = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        // set the rotation of the player
        rb.SetRotation(angleToMouse);
    }

    void FixedUpdate()
    {
        // calculating the horizontal speed the rb should have
        float targetSpeedHorizontal = inputMovement.x * maxMovementSpeed;
        // difference between the speed the rb should have and the actual one
        float speedDifferenceHorizontal = targetSpeedHorizontal - rb.velocity.x;
        // F = m*a (m=1) and a = v/t (t=1) => F = v 
        // ==> force is the speed difference multiplied by an optional factor to speed up and brake faster
        float forceHorizontal = speedDifferenceHorizontal * acceleration;
        rb.AddForce(forceHorizontal * Vector2.right);

        // same thing for the y Axis
        float targetSpeedVertical = inputMovement.y * maxMovementSpeed;
        float speedDifferenceVertical = targetSpeedVertical - rb.velocity.y;
        float forceVertical = speedDifferenceVertical * acceleration;
        rb.AddForce(forceVertical * Vector2.up);
    }
}
