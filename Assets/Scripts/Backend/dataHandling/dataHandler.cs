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
    public ShopItems[] shopItemsList;
    public int countWeapons;
    public int countChests;
    public int countShopItems;

    public void Start()
    {
        // path to assets/data/weaponStats.json
        string path = Application.dataPath + "/data/data.json";

        // reads the json file
        StreamReader reader = new StreamReader(path); 
        string json = reader.ReadToEnd();

        // stores the data in the lists
        Data<weaponStats, chestStats, ShopItems> dataObject = CreateFromJSON<weaponStats, chestStats, ShopItems>(json);
        weaponStatsList = dataObject.weapons;
        chestsStatsList = dataObject.chests;
        shopItemsList = dataObject.shop;

        // sets the amount of the items
        countWeapons = weaponStatsList.Length;
        countChests = chestsStatsList.Length;
        countShopItems = shopItemsList.Length;
    }

    // converts the json into an c# object
    public static Data<weaponList, chestStats, ShopItems> CreateFromJSON<weaponList, chestStats, ShopItems>(string jsonString)
    {
        Data<weaponList, chestStats, ShopItems> items = JsonUtility.FromJson<Data<weaponList, chestStats, ShopItems>>(jsonString);
        return items;
    }
}

// the json will be converted to a c# object like these
[System.Serializable]
public class Data<weaponList, chestStats, ShopItems>
{
    public weaponList[] weapons;
    public chestStats[] chests;
    public ShopItems[] shop;
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

[System.Serializable]
public class ShopItems
{
    public string name;
    public int startPrice;
    public string priceFunction;
    public int maxLevel;
    public float incrementPerUpgrade;
}
