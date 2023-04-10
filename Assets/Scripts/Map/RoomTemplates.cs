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
    public float waitTime;
    public GameObject boss;

    //variables for functions
    public List<GameObject> rooms;
    private bool spawnedBoss;

    private void Update()
    {
        if(waitTime <= 0 && spawnedBoss == false)
        {
            /*for(int i = 0; i < rooms.Count; i++)
            {
                if(i == rooms.Count - 1)
                {
                    Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                    spawnedBoss = true;
                    break;
                }
            }*/
            Instantiate(boss, rooms[rooms.Count-1].transform.position, Quaternion.identity);
            spawnedBoss = true;

        }else{
            waitTime -= Time.deltaTime;
        }
    }
}
