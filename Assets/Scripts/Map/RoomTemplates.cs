using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    //Array for list of posible rooms
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject closedRoom;

    // preset Variables/References
    public GameObject boss;
    public GameObject[] chests;
    public float waitTime;
    public float size;
    public int roomsNumber;//how many rooms there will be

    //variables for functions
    public List<GameObject> rooms;
    private bool spawnedBoss;

    private void Update()
    {
        if(waitTime <= 0 && spawnedBoss == false)
        {
            Instantiate(boss, rooms[roomsNumber-1].transform.position, Quaternion.identity);
            spawnedBoss = true;
        }else if(waitTime > 0){
            waitTime -= Time.deltaTime;
        }

        if(rooms.Count > roomsNumber){
            for(int i = 0; rooms.Count > roomsNumber; i++){
                Destroy(rooms[rooms.Count-1]);
                rooms.RemoveAt(rooms.Count-1);
            }
        }
    }
}
