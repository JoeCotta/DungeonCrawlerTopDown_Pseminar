using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManagment : MonoBehaviour
{
    //preset References
    public GameObject enemySpawner;
    public GameObject enemy;
    public GameObject mapHidder;

    //working variables and references
    private GameObject door;
    private GameObject doorvrt;
    public GameObject doorFix;
    public GameObject doorvrtFix;
    private RoomTemplates templates;
    private GameObject thisRoom;
    public string[] doors;
    public bool oben;
    public bool rechts;
    public bool unten;
    public bool links;
    public GameObject chestPrefab;

    //spawn enemys
    public GameObject[] closedDoors;
    public GameObject[] chests;
    private float xCoord;
    private float yCoord;
    public int enemysCount;
    public bool spawned;
    public int enemysDead = 0;
    public int maxEnemys = 1;
    public int minEnemys = 4;    

    void Start()
    {
        door = GameObject.FindGameObjectWithTag("Door"); 
        doorvrt = GameObject.FindGameObjectWithTag("Doorvrt");
        doorFix = GameObject.FindGameObjectWithTag("DoorFix");
        doorvrtFix = GameObject.FindGameObjectWithTag("DoorvrtFix");
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        chests = templates.chests;
        chestPrefab = GameObject.FindGameObjectWithTag("chest");

        //add room to list of rooms
        templates.rooms.Add(this.gameObject);
        thisRoom = gameObject;
        thisRoom.transform.localScale = thisRoom.transform.localScale * templates.size;
    }

    void Update(){
        //delete doors if all enemys dead
        if (enemysDead == enemysCount && spawned == true && enemysCount > 0){
            roomFinished();
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        //Test for player entering and call functions
        if(other.CompareTag("Player")&&spawned == false){
            spawned = true;
            if(mapHidder != null)Destroy(mapHidder); mapHidder = null;
            closeEntrances();
            spawnEnemys();
        }
    }


    //--------------------------------------------------------------------------------------------------------------------------------------
    public void fixSpawns(){
        //Check for every room if they have matching doors
        for (int i = 0; i < doors.Length; i++){
            if(doors[i] == "U"){
                for(int i2 = 0; i2 < templates.rooms.Count; i2++){
                    //check for every existing room if next to each other
                    if(templates.rooms[i2].transform.position.y - thisRoom.transform.position.y == 10*templates.size && templates.rooms[i2].transform.position.x == thisRoom.transform.position.x){
                        for(int i3 = 0; i3 < templates.rooms[i2].GetComponent<RoomManagment>().doors.Length; i3++){//für jeder der türen dieses Raumes Checken ob es die passenende gegen tür ist
                            if(templates.rooms[i2].GetComponent<RoomManagment>().doors[i3] == "D"){
                                oben = true;
                                break;
                            }
                        }
                    }
                }
                if(oben == false){
                    //Fix door and delete, room "loses" this door => new Room as if there never was a door
                    Instantiate(doorFix, transform.position + new Vector3(0,4.5f*templates.size,0), Quaternion.identity);
                    doors[i] = " "; 
                }
            }

            if(doors[i] == "R"){
                for(int i2 = 0; i2 < templates.rooms.Count; i2++){
                    if(templates.rooms[i2].transform.position.x - thisRoom.transform.position.x == 10*templates.size && templates.rooms[i2].transform.position.y == thisRoom.transform.position.y){
                        for(int i3 = 0; i3 < templates.rooms[i2].GetComponent<RoomManagment>().doors.Length; i3++){
                            if(templates.rooms[i2].GetComponent<RoomManagment>().doors[i3] == "L"){
                                rechts = true;
                                break;
                            }
                        }
                    }
                }
                if(rechts == false){
                    Instantiate(doorvrtFix, transform.position + new Vector3(4.5f*templates.size,0,0), Quaternion.identity);
                    doors[i] = " ";
                }
            }

            if(doors[i] == "D"){
                for(int i2 = 0; i2 < templates.rooms.Count; i2++){
                    if(templates.rooms[i2].transform.position.y - thisRoom.transform.position.y == -10*templates.size && templates.rooms[i2].transform.position.x == thisRoom.transform.position.x){
                        for(int i3 = 0; i3 < templates.rooms[i2].GetComponent<RoomManagment>().doors.Length; i3++){
                            if(templates.rooms[i2].GetComponent<RoomManagment>().doors[i3] == "U"){
                                unten = true;
                                break;
                            }
                        }
                    }
                }
                if(unten == false){
                    Instantiate(doorFix, transform.position + new Vector3(0,-4.5f*templates.size,0), Quaternion.identity);
                    doors[i] = " ";
                }
            }

            if(doors[i] == "L"){
                for(int i2 = 0; i2 < templates.rooms.Count; i2++){
                    if(templates.rooms[i2].transform.position.x - thisRoom.transform.position.x == -10*templates.size && templates.rooms[i2].transform.position.y == thisRoom.transform.position.y){
                        for(int i3 = 0; i3 < templates.rooms[i2].GetComponent<RoomManagment>().doors.Length; i3++){
                            if(templates.rooms[i2].GetComponent<RoomManagment>().doors[i3] == "R"){
                                links = true;
                                break;
                            }
                        }
                    }
                }
                if(links == false){
                    Instantiate(doorvrtFix, transform.position + new Vector3(-4.5f*templates.size,0,0), Quaternion.identity);
                    doors[i] = " ";    
                }
            }

        }

    }

    void closeEntrances(){
        //close room off and save door Gameobjects in array
        closedDoors = new GameObject[doors.Length];
        for(int i = 0; i < doors.Length; i++){
            switch(doors[i]){
                case "U":
                    closedDoors[i] = Instantiate(door, transform.position + new Vector3(0,4.5f*templates.size,0), Quaternion.identity);
                    break;
                case "R":
                    closedDoors[i] = Instantiate(doorvrt, transform.position + new Vector3(4.5f*templates.size,0,0), Quaternion.identity);
                    break;
                case "D":
                    closedDoors[i] = Instantiate(door, transform.position + new Vector3(0,-4.5f*templates.size,0), Quaternion.identity);
                    break;
                case "L":
                    closedDoors[i] = Instantiate(doorvrt, transform.position + new Vector3(-4.5f*templates.size,0,0), Quaternion.identity);
                    break;
            }
        }
    }

    void spawnEnemys(){
        //pick random amount of enemys and if spawnpoint positive or negativ coordinate
        enemysCount = Random.Range(minEnemys,maxEnemys);
        
        for (int i = 0; i < enemysCount; i++){

            int xrand = Random.Range(-1, 2); while (xrand == 0) xrand = Random.Range(-1, 2); Debug.Log(xrand);
            int yrand = Random.Range(-1, 2); while (yrand == 0) yrand = Random.Range(-1, 2); Debug.Log(yrand);
            //pick coordinate in the room/offset
            xCoord = xrand * Random.Range(1,3.5f);
            yCoord = yrand * Random.Range(1,3.5f);

            //spawn enemy and add to array
            GameObject tempSpawner = Instantiate(enemySpawner,transform.position + new Vector3(xCoord*templates.size, yCoord*templates.size, 0),Quaternion.identity);
            tempSpawner.GetComponent<EnemySpawner>().manager = this;
            tempSpawner.GetComponent<EnemySpawner>().enemy = enemy;
        }
    }

    public void killEnemy(GameObject deadEnemy){
        //called by enemy on death => increase int for "roomFinished"
        enemysDead ++;
        Destroy(deadEnemy);
    }

    void roomFinished(){
        //open room again
        for(int i = 0; i < doors.Length; i++){
            Destroy(closedDoors[i]);
        }

        //decide if spawn chest and which one
        int randomChestGen = Random.Range(1,5);
        if(randomChestGen == 1){
            // get the amount of available chests
            int countChests = GameObject.FindWithTag("dataHandler").GetComponent<dataHandler>().countChests;

            chestStats[] chestsStatsList = GameObject.FindWithTag("dataHandler").GetComponent<dataHandler>().chestsStatsList;
            List<int> chestList = new List<int>();

            // creates a list with 1000 elements to simulate the different Spawn chances of the chests
            for(int i = 0; i < countChests; i++)
            {
                for(int j = 0; j < chestsStatsList[i].chestSpawnChance * 1000; j++)
                {
                    chestList.Add(chestsStatsList[i].chestLevel);
                }
            }

            // checks if all the spawn chances are add together is 1 otherwise the chances are not correct
            if(chestList.Count != 1000) Debug.Log("Chest Spawn Chances are not correct");

            // select a random chest
            int randomChest = Random.Range(0, chestList.Count);
            int chestLevel = chestList[randomChest];

            // creates a chest and sets the level
            GameObject chest = Instantiate(chestPrefab,transform.position,Quaternion.identity);
            chest.GetComponent<Chest>().chestLevel = chestLevel;
        }
        enemysCount = 0;
    }
}
