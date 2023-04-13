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

    // Update is called once per frame
    void fixSpawns(){
        for(int i = 0; i < doors.Length; i++){
            //one segment to check if both rooms have doors
            if(doors[i] == "U"){//erst obberer
                for(int i2 = 0; i2 < templates.rooms.Count; i2++){//für jeden einzelnen Raum dden es gibt
                    //überprüfen ober er raum über diesen raum liegt
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

    void Update(){
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

        if(allEnemysDead){//delete doors if all enemys dead
            for(int i = 0; i < 4; i++){
                Destroy(closedDoors[i]);
            }
        }
        
    }

    void playerEnters(){
        spawned = true;
        //close of room
        closedDoors = new GameObject[4];
        closedDoors[0] = Instantiate(door, transform.position + new Vector3(0,4.5f*templates.size,0), Quaternion.identity);
        closedDoors[1] = Instantiate(doorvrt, transform.position + new Vector3(4.5f*templates.size,0,0), Quaternion.identity);
        closedDoors[2] = Instantiate(door, transform.position + new Vector3(0,-4.5f*templates.size,0), Quaternion.identity);
        closedDoors[3] = Instantiate(doorvrt, transform.position + new Vector3(-4.5f*templates.size,0,0), Quaternion.identity);

        //pick how many enemys and spawn them with random location
        enemysCount = Random.Range(1,4);
        enemys = new GameObject[enemysCount];
        enemysDead = new bool[enemysCount];
        for(int i = 0; i < enemysCount; i++){
            int leftOrRight = Random.Range(0,2);
            if(leftOrRight == 0){
                xCoord = Random.Range(-3.5f,-1);
            }else{
                xCoord = Random.Range(1,3.5f);
            }
            int upOrDown = Random.Range(0,2);
            if(upOrDown == 0){
                yCoord = Random.Range(-3.5f,-1);
            }else{
                yCoord = Random.Range(1,3.5f);
            }
        enemys[i] = Instantiate(enemy,transform.position + new Vector3(xCoord*templates.size, yCoord*templates.size, 0),Quaternion.identity);
        }
    }
    

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")&&spawned == false){
            playerEnters();
        }
    }
}
