using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    //INFOS
    //weaponDropChance as 1 in X chance


    public GameObject[] doors;
    public GameObject boss;
    public float size;

    //Enemy
    public int maxGold, weaponDropChance;

    void Start()
    {
        DataBase.door = doors[0]; DataBase.doorVrt = doors[1]; DataBase.doorFix = doors[2]; DataBase.doorFixVrt = doors[3];
        DataBase.boss = boss;
        DataBase.size = size;

        DataBase.maxGold = maxGold;
        DataBase.weaponDropChance = weaponDropChance + 1; //because its exclusive
    }
}
