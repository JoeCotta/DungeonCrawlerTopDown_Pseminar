using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class dataHandler : MonoBehaviour 
{   
    // in this List are all the weapons with the stats
    public weaponStats[] weaponStatsList;
    public chestStats[] chestsStatsList;
    public int countWeapons;
    public int countChests;

    public void Start()
    {
        // path to assets/data/weaponStats.json
        string path = Application.dataPath + "/data/data.json";

        // reads the json file
        StreamReader reader = new StreamReader(path); 
        string json = reader.ReadToEnd();

        // stores the data in the lists
        Data<weaponStats, chestStats> dataObject = CreateFromJSON<weaponStats, chestStats>(json);
        weaponStatsList = dataObject.weapons;
        chestsStatsList = dataObject.chests;

        // sets the amount of weapons
        countWeapons = weaponStatsList.Length;

        // sets the amount of chests
        countChests = chestsStatsList.Length;
    }

    // converts the json into an c# object
    public static Data<weaponList, chestStats> CreateFromJSON<weaponList, chestStats>(string jsonString)
    {
        Data<weaponList, chestStats> weaponItems = JsonUtility.FromJson<Data<weaponList, chestStats>>(jsonString);
        return weaponItems;
    }
}

// the json will be converted to a c# object like these
[System.Serializable]
public class Data<weaponList, chestStats>
{
    public weaponList[] weapons;
    public chestStats[] chests;
}

[System.Serializable]
public class weaponStats
{   
    public int id;
    public string name;
    public float fireCooldown;
    public float damage;
    public float shootForce;
    public float speedWhileWearing;
    public float FOVWhileWearing;
    public chestWeaponChance[] chestSpawnWeaponChances;
}

[System.Serializable]
public class chestWeaponChance
{
    public int level;
    public float chance;
}

[System.Serializable]
public class chestStats
{
    public int chestLevel;
    public float chestSpawnChance;    
}
