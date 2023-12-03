using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    static public GameObject door, doorVrt, doorFix, doorFixVrt, boss, altar, ammoBox, portal;
    static public float size;

    static public int maxGold, weaponDropChance, boostDropChance;


    static public float[] weaponBase(int weaponType, int rarity)
    {
        float[] temp = new float[7];
        switch (weaponType)
        {
            case 0://pistol
                temp[0] = 6;    //dmg
                temp[1] = 12;   //mag
                temp[2] = 45;   //reserve
                temp[3] = 2.5f;    //rate
                temp[4] = 0.90f;//accuracy
                temp[5] = 1.5f;    // fov
                temp[6] = -1;   // speed
                break;
            case 1://rifel
                temp[0] = 4;    //dmg
                temp[1] = 30;   //mag
                temp[2] = 90;   //reserve
                temp[3] = 4;    //rate
                temp[4] = 0.75f;//accuracy
                temp[5] = 1.5f;    // fov
                temp[6] = -2;   // speed
                break;
            case 2://sniper
                temp[0] = 35;   //dmg
                temp[1] = 5;    //mag
                temp[2] = 15;   //reserve
                temp[3] = 0.3f; //rate
                temp[4] = 1.00f;//accuracy
                temp[5] = 3;    // fov
                temp[6] = -3;   // speed
                break;
            default:
                temp[0] = 5;
                temp[1] = 12;
                temp[2] = 36;
                temp[3] = 2;
                temp[4] = 0.90f;
                temp[5] = 2;
                temp[6] = -1;   // speed
                break;
        }
        switch (rarity)
        {
            case 0:
                //no stats changed
                break;
            case 1:
                temp[0] *= 1.25f;
                temp[1] *= 1.25f;
                temp[2] *= 1.25f;
                break;
            case 2:
                temp[0] *= 1.5f;
                temp[1] *= 1.5f;
                temp[2] *= 2f;
                temp[3] *= 1.25f;
                temp[4] *= 1.25f; if (temp[4] > 1.0f) temp[4] = 1.0f;
                break;
            case 3:
                temp[0] *= 2f;
                temp[1] *= 2f;
                temp[2] *= 4f;
                temp[3] *= 1.5f;
                temp[4] *= 1.25f; if (temp[4] > 1.0f) temp[4] = 1.0f;
                temp[5] *= 1.5f;
                break;
            default:
                //same as 0
                break;
        }
        return temp;
    }
    static public int rarity(int chestLvl)
    {
        //-Switch is for Different level of chests for the different drop chances of raritys
        //-in case: if hits chance in if -> rare, elsewise epic       <common dont drop from chests>
        float randomizer = Random.value; int value;
        switch (chestLvl)
        {
            case 0:
                if (randomizer < 0.8f) value = 1;
                else value = 2;
                break;
            case 1:
                if (randomizer < 0.6f) value = 1;
                else value = 2;
                break;
            case 2:
                if (randomizer < 0.3f) value = 1;
                else value = 2;
                break;
            default:
                value = 1;
                break;
        }
        return value;
    }
    static public int weaponType(int chestLvl)
    {
        float randomizer = Random.value; int value;
        switch (chestLvl)
        {
            case 0:
                if (randomizer < 0.8f) value = 0;
                else value = 1;
                break;
            case 1:
                if (randomizer < 0.9f) value = 0;
                if (randomizer < 0.4f) value = 1;
                else value = 2;
                break;
            case 2:
                if (randomizer < 0.6f) value = 0;
                if (randomizer < 0.3f) value = 1;
                else value = 2;
                break;
            default:
                value = 1;
                break;
        }
        return value;
    }
    static public int chestLevel()
    {
        float randomizer = Random.value; 
        int value = 0;
        if (randomizer < 0.4f) value = 1;
        if (randomizer < 0.2f) value = 2;
        return value;
    }
}
