using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public RoomTemplates templates; //reference for Size multiplier
    public bool isVrt;
    public bool isFixDoor;
    
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        if(isVrt){
            gameObject.transform.localScale = new Vector3(1,2,0)* templates.size;//change if door size generall change
        }else{
            gameObject.transform.localScale = new Vector3(2,1,0)* templates.size;
        }
        if (isFixDoor)
        {
            //change color to match walls
        }
    }
}
