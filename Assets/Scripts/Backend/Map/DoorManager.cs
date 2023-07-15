using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public bool isVrt;
    
    void Start()
    {
        if(isVrt){
            gameObject.transform.localScale = new Vector3(1,2,0)* DataBase.size;//change if door size generall change
        }else{
            gameObject.transform.localScale = new Vector3(2,1,0)* DataBase.size;
        }
    }
}
