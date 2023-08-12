using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    static public GameObject door;
    static public GameObject doorVrt;
    static public GameObject doorFix;
    static public GameObject doorFixVrt;
    static public GameObject boss;
    static public float size;

    static public int maxGold;
    static public int weaponDropChance;


    static public float[] weaponBase(int weaponType)
    {
        float[] temp = new float[6];
        switch (weaponType)
        {
            case 0://pistol
                temp[0] = 5; //dmg
                temp[1] = 12; //mag
                temp[2] = 36; //reserve
                temp[3] = 2; //rate
                temp[4] = 0.90f;//accuracy
                temp[5] = 0;
                break;
            case 1://rifel
                temp[0] = 4;
                temp[1] = 30;
                temp[2] = 90;
                temp[3] = 5;
                temp[4] = 0.80f;
                temp[5] = 0;
                break;
            case 2://sniper
                temp[0] = 40;
                temp[1] = 5;
                temp[2] = 15;
                temp[3] = 0.5f;
                temp[4] = 1.00f;
                temp[5] = 3;
                break;
            default:
                temp[0] = 5;
                temp[1] = 12;
                temp[2] = 36;
                temp[3] = 2;
                temp[4] = 0.90f;
                temp[5] = 0;
                break;
        }
        return temp;
    }
}
