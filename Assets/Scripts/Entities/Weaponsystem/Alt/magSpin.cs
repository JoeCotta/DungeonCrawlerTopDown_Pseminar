using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magSpin : MonoBehaviour
{
    private float rotationSpeed;
    private void Start() { rotationSpeed = Random.Range(300, 400); }
    void Update()
    {
        GetComponent<Rigidbody2D>().SetRotation(GetComponent<Rigidbody2D>().rotation + rotationSpeed * Time.deltaTime);
        if (rotationSpeed > 0) rotationSpeed -= 150 * Time.deltaTime;
        else Destroy(this);
    }
}
