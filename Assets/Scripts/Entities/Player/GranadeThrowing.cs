using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeThrowing : MonoBehaviour
{
    [SerializeField] GameObject Granades;
    void Update()
    {
        if (Input.GetKeyDown("g"))
        {
            GameObject temp = Instantiate(Granades, transform.position, Quaternion.identity);
            if (temp.GetComponent<Rigidbody2D>()) temp.GetComponent<Rigidbody2D>().AddForce( (transform.up * Mathf.Sin( Mathf.Deg2Rad * (gameObject.GetComponent<Player>().angleToMouse - 180) ) + transform.right * Mathf.Cos( Mathf.Deg2Rad * (gameObject.GetComponent<Player>().angleToMouse -180) )) * 1000);
        }
    }
}
