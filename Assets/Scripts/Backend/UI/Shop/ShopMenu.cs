using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Data;
using System;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    private float dumpVariable; //only used for not used variables
    private int dumpVariableInt;


    public DataPersistenceManager dataPersistenceManager;
    public float basepriceMaxHealth;
    public float priceMaxHealth = 10;
    public float basepriceArmor;
    public float priceArmor = 15;
    public float basepriceRevive;
    public float priceRevive = 50;

    public GameObject shopItemPrefab;
    public int maxItemsInRow = 5;

    public GameObject emptyText;

    private ShopItems[] shopItemsList;
    private int countShopItems;
    private List<GameObject> shopItemsGameObjectsList = new List<GameObject>();

    [SerializeField] private AudioSource buySound;
    private void Start()
    {
        dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();
        //priceArmor = Mathf.Pow(1.5f, dataPersistenceManager.gameData.startArmor) * basepriceArmor;
        
        // extracts the needed variables out of the dataHandler
        shopItemsList = GameObject.FindWithTag("dataHandler").GetComponent<dataHandler>().shopItemsList;
        countShopItems = GameObject.FindWithTag("dataHandler").GetComponent<dataHandler>().countShopItems;

        loadShop();
    }
    private void showText(string text) {
        emptyText.GetComponent<TextMeshProUGUI>().text = text;
        emptyText.SetActive(true);
        Invoke("disableText", 1.5f);
    }
    private void disableText() 
    {
        emptyText.SetActive(false);
    }

    public void buy(int ListElement)
    {
        // gets needed values
        int price = calculatePrice(ListElement);
        int maxLevel = shopItemsList[ListElement].maxLevel;

        // if the player has not enough coins
        if(dataPersistenceManager.gameData.currentCoins < price) return;
        // if the level is already at the maximum level
        if(maxLevel > 0 && maxLevel <= dataPersistenceManager.gameData.getLevel(shopItemsList[ListElement].name))
        {
            showText("Sorry - it's already maxed out!");
            return;
        }

        // lowers the player's coins
        dataPersistenceManager.gameData.currentCoins = dataPersistenceManager.gameData.currentCoins - price;
        // increases the level
        dataPersistenceManager.gameData.getLevel(shopItemsList[ListElement].name)++;
        // updates the Players Stats
        dataPersistenceManager.gameData.updateValues();
        dataPersistenceManager.gameData.setValuesToMax();
        // saves the new Stats
        dataPersistenceManager.SaveGame();
        // updates the displayed shop Item
        updateShopItem(ListElement);
        
        // play buy Sound
        buySound.Play();
    }

    public void loadShop()
    {
        int row = -1;

        for(int i = 0; i < countShopItems; i++)
        {
            // increases the row, if there are more elements than maxItemsInRow in a row
            if(i % maxItemsInRow == 0) row++;

            // creates the new Shop Item
            GameObject item = Instantiate(shopItemPrefab, transform);

            // adds the item to the list
            shopItemsGameObjectsList.Add(item);

            // ---------------- modifies the ShopItem ----------------
            // positions the Item
            item.GetComponent<RectTransform>().anchoredPosition = new Vector2((200 * (i%maxItemsInRow)) - 300, 95 - row*100);

            // add the eventListener to call a function when the button is clicked
            int ListElement = i;
            item.GetComponent<Button>().onClick.AddListener(() => buy(ListElement));

            // things that have to be done repeatedly are in the update function
            updateShopItem(i);
        }
    }

    public void updateShopItem(int ListElement)
    {
        // gets the GameObject
        GameObject item = shopItemsGameObjectsList[ListElement];

        // gets the item's name
        string name = shopItemsList[ListElement].name;

        int cost = calculatePrice(ListElement);

        // gets current level
        int level = dataPersistenceManager.gameData.getLevel(name);

        // sets the button Text -> name \n cost \n current level
        item.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name + "\ncost: " + cost.ToString() + "\nlevel/left: " + level.ToString();

    }

    private int calculatePrice(int ListElement)
    {
        string name = shopItemsList[ListElement].name;

        // gets current level
        int level = dataPersistenceManager.gameData.getLevel(name);

        // calculates the price of the item with the price function
        // replaces the x with th current level
        string function = shopItemsList[ListElement].priceFunction.Replace("x", level.ToString());

        // computes the function
        DataTable table = new DataTable();
        int cost = Convert.ToInt32(table.Compute(function, null));  
        return cost;
    }

    //Menu Managment
    public void openMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
