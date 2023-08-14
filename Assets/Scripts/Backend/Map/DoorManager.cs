using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public bool isVrt;
    
    void Start()
    {
        if(isVrt){
            gameObject.transform.localScale *= DataBase.size;//change if door size generall change
        }else{
            gameObject.transform.localScale *= DataBase.size;
        }
    }
}
