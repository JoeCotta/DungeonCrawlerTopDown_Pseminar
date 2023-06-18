using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    //Array for list of posible rooms
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    // preset Variables/References
    public GameObject boss;
    public GameObject[] chests;
    public float waitTime;
    public float size;
    public int roomsNumber;//how many rooms there will be

    //variables for functions
    public List<GameObject> rooms;
    private bool spawnedBoss;
    private bool onceFix = false;

    private void Update()
    {
        //pick boss room
        if(waitTime <= 0 && spawnedBoss == false)
        {
            rooms[roomsNumber - 1].GetComponent<RoomManagment>().isBossR = true;
            spawnedBoss = true;
        }else if(waitTime > 0 && spawnedBoss == false){
            waitTime -= Time.deltaTime;
        }

        //destroy execess Rooms and fix spawns, when desired amount reached
        if(rooms.Count > roomsNumber){
            for(int i = 0; rooms.Count > roomsNumber; i++){
                Destroy(rooms[rooms.Count-1]);
                rooms.RemoveAt(rooms.Count-1);
            }
        }else if(rooms.Count == roomsNumber && onceFix == false)
        {
            foreach (var item in rooms)
            {
                item.GetComponent<RoomManagment>().fixSpawns();
            }
            onceFix = true;
        }
    }
}
