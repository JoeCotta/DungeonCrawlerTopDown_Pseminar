using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomManagment : MonoBehaviour
{
    //preset References
    public GameObject enemySpawner, enemy, mapHidder;

    //working variables and references
    private GameObject door, doorvrt, doorFix, doorvrtFix, thisRoom, chestPrefab;
    private RoomTemplates templates;
    public string[] doors;
    public bool oben = false, rechts = false, unten = false, links = false;

    //spawn enemys
    public GameObject[] closedDoors, chests;
    private float xCoord, yCoord;
    public int enemysCount, enemysDead, maxEnemys, minEnemys;
    public bool spawned, isBossR = false;

    // Sounds
    [SerializeField] private AudioSource killSound;
    [SerializeField] private AudioSource enemySpawnSound;
    [SerializeField] private AudioSource closeDoorSound;
    [SerializeField] private AudioSource openDoorSound;
    [SerializeField] private AudioSource bossDeathSound;

    void Start()
    {
        door = DataBase.door; doorvrt = DataBase.doorVrt; doorFix = DataBase.doorFix; doorvrtFix = DataBase.doorFixVrt;
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        chests = templates.chests;
        chestPrefab = GameObject.FindGameObjectWithTag("chest");

        //add room to list of rooms
        templates.rooms.Add(this.gameObject);
        thisRoom = gameObject;
        thisRoom.transform.localScale = thisRoom.transform.localScale * templates.size;
    }

    void Update() {
        //delete doors if all enemys dead
        if (enemysDead == enemysCount && spawned == true && enemysCount > 0 && isBossR == false) {
            roomFinished();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        //Test for player entering and call functions
        if (other.CompareTag("Player") && spawned == false) {
            spawned = true;
            if (mapHidder != null) Destroy(mapHidder); mapHidder = null;
            closeEntrances();
            spawnEnemys();
        }
    }


    //--------------------------------------------------------------------------------------------------------------------------------------
    public void fixSpawns()
    {
        Vector3 tempPosition;
        //Check for every room if they have matching doors
        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i] == "U")
            {
                for (int i2 = 0; i2 < templates.rooms.Count; i2++)
                {
                    tempPosition = templates.rooms[i2].transform.position;
                    if (Mathf.Round(tempPosition.y - thisRoom.transform.position.y) == 10 * templates.size && tempPosition.x == thisRoom.transform.position.x)
                    {
                        for (int i3 = 0; i3 < templates.rooms[i2].GetComponent<RoomManagment>().doors.Length; i3++)
                        {
                            if (templates.rooms[i2].GetComponent<RoomManagment>().doors[i3] == "D")
                            {
                                oben = true;
                                break;
                            }
                        }
                    }
                }
                if (!oben)
                {
                    Instantiate(doorFix, transform.position + new Vector3(0, 4.5f * templates.size, 0), Quaternion.identity);
                    doors[i] = " ";
                }
            }


            if (doors[i] == "R")
            {
                for (int i2 = 0; i2 < templates.rooms.Count; i2++)
                {
                    tempPosition = templates.rooms[i2].transform.position;
                    if (Mathf.Round(tempPosition.x - thisRoom.transform.position.x) == 10 * templates.size && tempPosition.y == thisRoom.transform.position.y)
                    {
                        for (int i3 = 0; i3 < templates.rooms[i2].GetComponent<RoomManagment>().doors.Length; i3++)
                        {
                            if (templates.rooms[i2].GetComponent<RoomManagment>().doors[i3] == "L")
                            {
                                rechts = true;
                                break;
                            }
                        }
                    }
                }
                if (!rechts)
                {
                    Instantiate(doorvrtFix, transform.position + new Vector3(4.5f * templates.size, 0, 0), Quaternion.identity);
                    doors[i] = " ";
                }
            }

            if (doors[i] == "D")
            {
                for (int i2 = 0; i2 < templates.rooms.Count; i2++)
                {
                    tempPosition = templates.rooms[i2].transform.position;
                    if (Mathf.Round(tempPosition.y - thisRoom.transform.position.y) == -10 * templates.size && tempPosition.x == thisRoom.transform.position.x)
                    {
                        for (int i3 = 0; i3 < templates.rooms[i2].GetComponent<RoomManagment>().doors.Length; i3++)
                        {
                            if (templates.rooms[i2].GetComponent<RoomManagment>().doors[i3] == "U")
                            {
                                unten = true;
                                break;
                            }
                        }
                    }
                }
                if (!unten)
                {
                    Instantiate(doorFix, transform.position + new Vector3(0, -4.5f * templates.size, 0), Quaternion.identity);
                    doors[i] = " ";
                }
            }

            if (doors[i] == "L")
            {
                for (int i2 = 0; i2 < templates.rooms.Count; i2++)
                {
                    tempPosition = templates.rooms[i2].transform.position;
                    if (Mathf.Round(tempPosition.x - thisRoom.transform.position.x) == -10 * templates.size && tempPosition.y == thisRoom.transform.position.y)
                    {
                        for (int i3 = 0; i3 < templates.rooms[i2].GetComponent<RoomManagment>().doors.Length; i3++)
                        {
                            if (templates.rooms[i2].GetComponent<RoomManagment>().doors[i3] == "R")
                            {
                                links = true;
                                break;
                            }
                        }
                    }
                }
                if (!links)
                {
                    Instantiate(doorvrtFix, transform.position + new Vector3(-4.5f * templates.size, 0, 0), Quaternion.identity);
                    doors[i] = " ";
                }
            }

        }

        
        /*GameObject tempRoom = null; Vector3 tempPosition;
        for (int i = 0; i < doors.Length; i++)
        {
            switch (doors[i])
            {
                case "U":
                    tempRoom = null;
                    for (int i2 = 0; i2 < templates.rooms.Count; i2++)
                    {
                        tempPosition = templates.rooms[i2].transform.position;
                        if (tempRoom == null && tempPosition.y - thisRoom.transform.position.y > 1 || tempPosition.x == thisRoom.transform.position.x && tempPosition.y - thisRoom.transform.position.y > 1 && tempPosition.y < tempRoom.transform.position.y)
                        {
                            tempRoom = templates.rooms[i2];
                            //Debug.Log(tempPosition.x - thisRoom.transform.position.x);
                        }
                    }
                    if (tempRoom == null) break; // failsave
                    foreach (string item in tempRoom.GetComponent<RoomManagment>().doors)
                    {
                        if (item == "D") { oben = true; Debug.Log(tempRoom + " " + item); break; }
                    }
                    if (oben == false) { Instantiate(doorFix, transform.position + new Vector3(0, 4.5f * templates.size, 0), Quaternion.identity); doors[i] = " "; break; }
                    break;

                case "R":
                    tempRoom = null;
                    for (int i2 = 0; i2 < templates.rooms.Count; i2++)
                    {
                        tempPosition = templates.rooms[i2].transform.position;
                        if ( tempRoom == null && tempPosition.x - thisRoom.transform.position.x > 1 || tempPosition.y == thisRoom.transform.position.y && tempPosition.x - thisRoom.transform.position.x > 1 && tempPosition.x < tempRoom.transform.position.x)
                        {
                            tempRoom = templates.rooms[i2];
                            //Debug.Log(tempPosition.x - thisRoom.transform.position.x);
                        }
                    }
                    if (tempRoom == null) break; // failsave
                    foreach (string item in tempRoom.GetComponent<RoomManagment>().doors)
                    {
                        if (item == "L") { rechts = true; break; }
                    }
                    if (rechts == false) { Instantiate(doorFix, transform.position + new Vector3(0, 4.5f * templates.size, 0), Quaternion.identity); doors[i] = " "; break; }
                    break;

                case "D":

                    break;
                case "L":

                    break;
            }
        }*/
    }

    void closeEntrances() {
            //close room off and save door Gameobjects in array
        closedDoors = new GameObject[doors.Length];
        for (int i = 0; i < doors.Length; i++) {
            switch (doors[i]) {
                case "U":
                    closedDoors[i] = Instantiate(door, transform.position + new Vector3(0, 4.5f * templates.size, 0), Quaternion.identity);
                    break;
                case "R":
                    closedDoors[i] = Instantiate(doorvrt, transform.position + new Vector3(4.5f * templates.size, 0, 0), Quaternion.identity);
                    break;
                case "D":
                    closedDoors[i] = Instantiate(door, transform.position + new Vector3(0, -4.5f * templates.size, 0), Quaternion.identity);
                    break;
                case "L":
                    closedDoors[i] = Instantiate(doorvrt, transform.position + new Vector3(-4.5f * templates.size, 0, 0), Quaternion.identity);
                    break;
            }
        }
        // play door close sound
        closeDoorSound.Play();
    }

    void spawnEnemys() {
        //spawn boss
        if (isBossR) 
        { 
            GameObject boss = Instantiate(DataBase.boss, transform.position + new Vector3(0, 2, 0) * templates.size, Quaternion.identity);
            boss.GetComponent<Boss>().manager = this;
        }
        else 
        {
            //pick random amount of enemys and if spawnpoint positive or negativ coordinate
            enemysCount = Random.Range(minEnemys, maxEnemys);

            for (int i = 0; i < enemysCount; i++) {

                int xrand = Random.Range(-1, 2); while (xrand == 0) xrand = Random.Range(-1, 2);
                int yrand = Random.Range(-1, 2); while (yrand == 0) yrand = Random.Range(-1, 2);
                //pick coordinate in the room/offset
                xCoord = xrand * Random.Range(1, 3.5f);
                yCoord = yrand * Random.Range(1, 3.5f);

                //spawn enemy and add to array
                GameObject tempSpawner = Instantiate(enemySpawner, transform.position + new Vector3(xCoord * templates.size, yCoord * templates.size, 0), Quaternion.identity);
                tempSpawner.GetComponent<EnemySpawner>().manager = this;
                tempSpawner.GetComponent<EnemySpawner>().enemy = enemy;
            }
        }
    }

    public void playSoundEnemySpawn()
    {
        // SpawnSound
        enemySpawnSound.Play();
    }

    public void killEnemy(GameObject deadEnemy) {
        // kill Sound
        killSound.Play();

        //called by enemy on death => increase int for "roomFinished"
        enemysDead++;
        Destroy(deadEnemy);
    }

    public void roomFinished() {
        //open room again
        for (int i = 0; i < doors.Length; i++) {
            Destroy(closedDoors[i]);
        }
        // play door open sound
        openDoorSound.Play();

        if(isBossR) {
            Instantiate(DataBase.portal, transform.position + new Vector3(0, 2, 0) * templates.size, Quaternion.identity);
            bossDeathSound.Play();
        }

        //decide if spawn chest and which one
        int randomChestGen = Random.Range(1, 5);
        if (randomChestGen == 1 && !isBossR) {
            // creates a chest and sets the level
            GameObject chest = Instantiate(chestPrefab, transform.position, Quaternion.identity);
            chest.GetComponent<Chest>().chestLevel = DataBase.chestLevel();
        }

        int randomAltar; randomAltar = Random.Range(1, 21);
        if(randomAltar == 1) Instantiate(DataBase.altar, transform.position + new Vector3(0,-2,0) * DataBase.size, Quaternion.identity);

        int randomAmmo; randomAmmo = Random.Range(1, 11);
        if (randomAmmo == 1) Instantiate(DataBase.ammoBox, transform.position + new Vector3(-2, 0, 0) * DataBase.size, Quaternion.identity);

        enemysCount = 0;
    }
}
