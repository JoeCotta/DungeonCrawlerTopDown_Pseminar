using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public bool isVrt;
    
    void Start()
    {
        //change if door size generall change
        if (isVrt) gameObject.transform.localScale *= DataBase.size;
        else gameObject.transform.localScale *= DataBase.size;

    }
}
