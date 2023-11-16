using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    //INFOS
    //weapon-/boostDropChance as 1 in X chance


    public GameObject[] doors;
    public GameObject boss, altar, ammoBox, portal;
    public float size;

    //Enemy
    public int maxGold, weaponDropChance, boostDropChance;

    void Start()
    {
        DataBase.door = doors[0]; DataBase.doorVrt = doors[1]; DataBase.doorFix = doors[2]; DataBase.doorFixVrt = doors[3];
        DataBase.boss = boss;
        DataBase.altar = altar;
        DataBase.ammoBox = ammoBox;
        DataBase.size = size;
        DataBase.portal = portal;

        DataBase.maxGold = maxGold;
        DataBase.weaponDropChance = weaponDropChance + 1; //because in Random.Range the second param. is exclusive
        DataBase.boostDropChance = boostDropChance + 1;
    }
}
