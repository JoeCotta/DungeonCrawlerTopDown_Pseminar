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
        float[] temp = new float[8];
        switch (weaponType)
        {
            case 0://pistol
                temp[0] = 6;    //dmg
                temp[1] = 12;   //mag
                temp[2] = 75;   //reserve
                temp[3] = 2f;   //rate
                temp[4] = 0.90f;//accuracy
                temp[5] = 1.5f; // fov
                temp[6] = 0;   // speed
                temp[7] = 1;
                break;
            case 1://rifel
                temp[0] = 4;    //dmg
                temp[1] = 30;   //mag
                temp[2] = 90;   //reserve
                temp[3] = 5;    //rate
                temp[4] = 0.75f;//accuracy
                temp[5] = 1.5f; // fov
                temp[6] = -1;   // speed
                temp[7] = 1;
                break;
            case 2://sniper
                temp[0] = 35;   //dmg
                temp[1] = 5;    //mag
                temp[2] = 15;   //reserve
                temp[3] = 0.3f; //rate
                temp[4] = 1.00f;//accuracy
                temp[5] = 3;    // fov
                temp[6] = -2;   // speed
                temp[7] = 1;
                break;
            case 3://shotgun
                temp[0] = 4;   //dmg
                temp[1] = 5;   //mag
                temp[2] = 25;  //reserve
                temp[3] = 1;   //rate
                temp[4] = 0.7f;//accuracy
                temp[5] = 1.5f;// fov
                temp[6] = -3;  // speed
                temp[7] = 5;   //bulletcount
                break;
            case 4://golden ak only legy
                temp[0] = 5;    //dmg
                temp[1] = 45;   //mag
                temp[2] = 270;  //reserve
                temp[3] = 5f;  //rate
                temp[4] = 0.55f;//accuracy
                temp[5] = 1.5f; // fov
                temp[6] = -2;   // speed
                temp[7] = 2;
                break;
            case 5://minigun only legy
                temp[0] = 4;    //dmg
                temp[1] = 250;  //mag
                temp[2] = 125;  //reserve
                temp[3] = 40;   //rate
                temp[4] = 0.15f; //accuracy
                temp[5] = 1.75f;// fov
                temp[6] = -3;   // speed
                temp[7] = 1;
                break;
            default:
                temp[0] = 5;
                temp[1] = 12;
                temp[2] = 36;
                temp[3] = 2;
                temp[4] = 0.90f;
                temp[5] = 2;
                temp[6] = -1;   // speed
                temp[7] = 1;
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
                if (weaponType == 3) temp[7] *= 1.25f;
                break;
            case 2:
                temp[0] *= 1.5f;
                temp[1] *= 1.5f;
                temp[2] *= 2f;
                temp[3] *= 1.25f;
                temp[4] *= 1.25f; if (temp[4] > 1.0f) temp[4] = 1.0f;
                if (weaponType == 3) temp[7] *= 1.5f;
                break;
            case 3:
                temp[0] *= 2f;
                temp[1] *= 2f;
                temp[2] *= 4f;
                temp[3] *= 1.5f;
                temp[4] *= 1.25f; if (temp[4] > 1.0f) temp[4] = 1.0f;
                temp[5] *= 1.5f;
                if (weaponType == 3) temp[7] *= 2f;
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
            case -2:
                value = 0;
                break;
            case 0:
                if (randomizer <= 0.8f) value = 1;
                else value = 2;
                break;
            case 1:
                if (randomizer <= 0.6f) value = 1;
                else value = 2;
                break;
            case 2:
                if (randomizer <= 0.3f) value = 1;
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
            case -2: //Barrel 
                if (randomizer <= 0.8f) value = 0;
                else if (0.7f < randomizer && randomizer <= 0.8f) value = 1;
                else if (0.8f < randomizer && randomizer <= 0.9f) value = 2;
                else value = 3;
                break;
            case 0: //40 to 30 to 30
                if (randomizer <= 0.4f) value = 0;
                else if (0.4f < randomizer && randomizer <= 0.7f) value = 1;
                else value = 3;
                break;
            case 1: //40 to 30 to 10 to 20
                if (randomizer <= 0.4f) value = 0;
                else if (0.4f < randomizer && randomizer <= 0.7f) value = 1;
                else if (0.7f < randomizer && randomizer <= 0.8f) value = 2;
                else value = 3;
                break;
            case 2: //20 to 30 to 20 to 30
                if (randomizer <= 0.2f) value = 0;
                else if (0.2f < randomizer && randomizer <= 0.5f) value = 1;
                else if (0.5f < randomizer && randomizer <= 0.7f) value = 2;
                else value = 3;
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
