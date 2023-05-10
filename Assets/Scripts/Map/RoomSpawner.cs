using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    //References
    public RoomTemplates templates;

    //previously set variables
    public float waitTime = 4f;
    public int doorDir;
    //1 -> needs Bottom
    //2 -> needs Top
    //3 -> needs Left
    //4 -> needs Right

    //temporary variables for functions
    public Vector3 offset;
    public Vector3 rotation;
    private int rand;
    public bool spawned = false;
    

    private void Start()
    {
        //grab list of rooms avaiable
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        //little pause before spawn of room
        Invoke("Spawn",0.2f);
        // delete after a amout of time
        Destroy(gameObject, waitTime); 
    }

    //prevent spawning multiple rooms inside each other
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                if (doorDir < other.gameObject.GetComponent<RoomSpawner>().doorDir)
                {
                    Destroy(other.gameObject);
                }
            }
            if (other.GetComponent<RoomSpawner>().spawned == true)
            {
                spawned = true;
            }
        }
    }

    //Process of spawning rooms; reference ln 28
    void Spawn()
    {
        if (spawned == false)
        {
            if (templates.rooms.Count >= templates.roomsNumber)
            {
                Destroy(gameObject);
            }
            if (doorDir == 1)
            {
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
            }
            else if (doorDir == 2)
            {
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
            }
            else if (doorDir == 3)
            {
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
            }
            else if (doorDir == 4)
            {
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
            }
            spawned = true;
        }
    }
}
