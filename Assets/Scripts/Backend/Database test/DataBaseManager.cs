using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public GameObject[] doors;
    public GameObject boss;
    public float size;
    void Start()
    {
        DataBase.door = doors[0]; DataBase.doorVrt = doors[1]; DataBase.doorFix = doors[2]; DataBase.doorFixVrt = doors[3];
        DataBase.boss = boss;
        DataBase.size = size;
    }
}
