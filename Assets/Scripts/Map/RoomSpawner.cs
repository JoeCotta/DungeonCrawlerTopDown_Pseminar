using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    // Gameobjects
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
        
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>(); // grab list of rooms avaiable
        Invoke("Spawn",0.2f); //little pause before spawn of room
        Destroy(gameObject, waitTime); // delete after a amout of time
    }

    //Process of spawning rooms; reference ln 32 
    void Spawn()
    {
        if (spawned == false)
        {
            if(templates.rooms.Count >= templates.roomsNumber){
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

    //prevent spawning multiple rooms inside each other
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("SpawnPoint"))
        {
            if(other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                Destroy(other.gameObject);
            }
            spawned = true;
        }
    }
}
