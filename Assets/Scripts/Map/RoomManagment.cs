using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManagment : MonoBehaviour
{
    //preset References
    public GameObject enemy;
    //working variables and references
    private GameObject door;
    private GameObject doorvrt;
    private RoomTemplates templates;
    private GameObject thisRoom;
    public string[] doors;
    public bool oben;
    public bool rechts;
    public bool unten;
    public bool links;

    //spawn enemys
    public GameObject[] enemys;
    public GameObject[] closedDoors;
    public float xCoord;
    public float yCoord;
    public int enemysCount;
    public bool spawned;
    public bool allEnemysDead;
    public bool[] enemysDead;
    

    // Start is called before the first frame update
    void Start()
    {
        door = GameObject.FindGameObjectWithTag("Door"); 
        doorvrt = GameObject.FindGameObjectWithTag("Doorvrt");
        //add room to list of rooms
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        templates.rooms.Add(this.gameObject);
        thisRoom = gameObject;
        thisRoom.transform.localScale = thisRoom.transform.localScale * templates.size;
        Invoke("fixSpawns",4f);
    }

    void Update(){
        if(allEnemysDead){//delete doors if all enemys dead
        roomFinished();
        }
        
        for(int i = 0; i < enemys.Length; i++){ //save state of dead enemys before destroying them
            if(enemys[i].GetComponent<Enemy>().isdead){
                enemysDead[i] = true;
            }
        }

        for(int i = 0; i < enemys.Length; i++){ // check if all enemys dead
            if(enemysDead[i]){
                allEnemysDead = true;
            }else{
                allEnemysDead = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")&&spawned == false){
            spawned = true;
            closeEntrances();
            spawnEnemys();
        }
    }


    //--------------------------------------------------------------------------------------------------------------------------------------
    void fixSpawns(){
        for(int i = 0; i < doors.Length; i++){
            //one segment to check if both rooms have doors
            if(doors[i] == "U"){
                for(int i2 = 0; i2 < templates.rooms.Count; i2++){//für jeden einzelnen Raum den es gibt
                    //überprüfen ob räume übereinander liegen
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
                    Instantiate(door, transform.position + new Vector3(0,4.5f*templates.size,0), Quaternion.identity);
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
                    Instantiate(doorvrt, transform.position + new Vector3(4.5f*templates.size,0,0), Quaternion.identity);
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
                    Instantiate(door, transform.position + new Vector3(0,-4.5f*templates.size,0), Quaternion.identity);
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
                    Instantiate(doorvrt, transform.position + new Vector3(-4.5f*templates.size,0,0), Quaternion.identity);
                }
            }

        }

    }

    void closeEntrances(){
        closedDoors = new GameObject[4];
        closedDoors[0] = Instantiate(door, transform.position + new Vector3(0,4.5f*templates.size,0), Quaternion.identity);
        closedDoors[1] = Instantiate(doorvrt, transform.position + new Vector3(4.5f*templates.size,0,0), Quaternion.identity);
        closedDoors[2] = Instantiate(door, transform.position + new Vector3(0,-4.5f*templates.size,0), Quaternion.identity);
        closedDoors[3] = Instantiate(doorvrt, transform.position + new Vector3(-4.5f*templates.size,0,0), Quaternion.identity);
    }

    void spawnEnemys(){
        //assing variables
        enemysCount = Random.Range(1,4);
        enemys = new GameObject[enemysCount];
        enemysDead = new bool[enemysCount];
        int xrand = Random.Range(-1,1);if(xrand == 0){xrand = Random.Range(-1,1);}
        int yrand = Random.Range(-1,1);if(yrand == 0){yrand = Random.Range(-1,1);}

        for(int i = 0; i < enemysCount; i++){
            //pick coordinate in the room/offset
            xCoord = xrand * Random.Range(1,3.5f);
            yCoord = yrand * Random.Range(1,3.5f);

            //spawn enemy and add to array
            enemys[i] = Instantiate(enemy,transform.position + new Vector3(xCoord*templates.size, yCoord*templates.size, 0),Quaternion.identity);
        }
    }

    void roomFinished(){
        Destroy(closedDoors[0]);Destroy(closedDoors[1]);Destroy(closedDoors[2]);Destroy(closedDoors[3]); //open room again

        int randomChest = Random.Range(1,5);
        if(randomChest == 1){
            //spawn chest or smth
        }
    }
}
