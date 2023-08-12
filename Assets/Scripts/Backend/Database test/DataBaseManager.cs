using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public GameObject[] doors;
    public GameObject boss;
    public float size;

    //Enemy
    public int maxGold;
    public int WeaponDropChance; //as a 1 in x chance
    public int boostDropChance; // as a 1 in x chance 

    void Start()
    {
        DataBase.door = doors[0]; DataBase.doorVrt = doors[1]; DataBase.doorFix = doors[2]; DataBase.doorFixVrt = doors[3];
        DataBase.boss = boss;
        DataBase.size = size;

        DataBase.maxGold = maxGold;
        DataBase.weaponDropChance = WeaponDropChance + 1; //because its exclusive

        DataBase.boostDropChance = boostDropChance + 1; //because in Random.Range the second param. is exclusive
    }
}
