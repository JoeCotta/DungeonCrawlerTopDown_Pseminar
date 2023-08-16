using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    void Start()
    {
        //change if door size generall change
        gameObject.transform.localScale *= DataBase.size;
    }
}
