using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public RoomTemplates templates;
    public bool isVrt;
    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        if(isVrt){
            gameObject.transform.localScale = new Vector3(1,3,0)* templates.size;//change if door size generall change
        }else{
            gameObject.transform.localScale = new Vector3(3,1,0)* templates.size;
        }
    }
}
