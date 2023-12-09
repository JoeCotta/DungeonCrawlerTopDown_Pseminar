using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{

    [SerializeField] private GameObject coinPrefab;

    [HideInInspector] public int amountCoins;
    int amountCoinsValue10;
    int amountCoinsValue1;

    void Start()
    {
        amountCoinsValue10 = (int)(amountCoins - 1) / 10;
        amountCoinsValue1 = amountCoins - amountCoinsValue10 * 10;
        Invoke("spawnCoin", 0.1f);
    }

    void spawnCoin()
    {
        if (amountCoinsValue10 <= 0 && amountCoinsValue1 <= 0) Destroy(gameObject);
        if (amountCoinsValue10 > 0){
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            coin.GetComponent<Coin>().valueCoin = 10;
            amountCoinsValue10--;
            Invoke("spawnCoin", 0.04f);
        }
        else if (amountCoinsValue1 > 0){
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            coin.GetComponent<Coin>().valueCoin = 1;
            amountCoinsValue1--;
            Invoke("spawnCoin", 0.04f);
        }
    }
}
