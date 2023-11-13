using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boosts : MonoBehaviour
{
    public int boostType;
    public Sprite[] textures;
    public static Sprite texture;
    [SerializeField]
    private GameObject uiCoolDownPrefab;

    private GameObject hud;

    private float toReset;
    private Player player;
    private GameManager manager;
    private DataPersistenceManager dataPersistenceManager;

    private bool obtained = false;
    private GameObject boost;

    // buffs
    float speedBuff = 0;
    float healBuff = 0; 
    float damageMultiplier = 0;

    float timeResetBuff = 0;
    [SerializeField] private AudioSource[] bootsEffects;

    void Start() {
        if(boostType == -1) boostType = 0;
        else if(boostType == -2) boostType = 1;
        else if(boostType == -3) boostType = 2;
        else boostType = Random.Range(0, textures.Length);
    
        gameObject.GetComponent<SpriteRenderer>().sprite = textures[boostType];
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        manager = GameObject.FindWithTag("Manager").GetComponent<GameManager>();
        hud = GameObject.FindWithTag("Hud");
        if(GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>()) dataPersistenceManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataPersistenceManager>();

    }


    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag != "Player" || obtained) return;

        // initializes ui animation
        boost = Instantiate(uiCoolDownPrefab, Vector2.zero, Quaternion.identity, hud.transform);

        switch (boostType)
        {
            // speedBoost
            case 0:
                speedBuff = 5;
                timeResetBuff = 3;
                break;

            // healBoost
            case 1:
                healBuff = 3;
                timeResetBuff = 3;
                break;

            // tmp damageMultiplier
            case 2:
                damageMultiplier = 2;
                timeResetBuff = 10;
                break;
        }

        // playSound
        bootsEffects[boostType].Play();

        // only sets the buff which is not zero
        player.speedBuff += speedBuff;
        player.healBuff += healBuff;
        if(damageMultiplier != 0) dataPersistenceManager.gameData.currentDamageMultiplier *= damageMultiplier;

        // initializes the reset
        Invoke("resetBuff", timeResetBuff);

        // starts the ui animation
        boost.GetComponent<BoostTime>().StartCooldown(timeResetBuff, textures[boostType]);

        // makes it known for other scripts that there is another boost
        manager.activeBoosts += 1;
        manager.boostList.Add(boost);

        // makes the boost invisible and unobtainable until the buff can be destroyed after the reset
        gameObject.GetComponent<SpriteRenderer>().sprite = null;
        obtained = true;
    }

    void resetBuff()
    {
        // only resets the buff which is not zero
        player.speedBuff -= speedBuff;
        player.healBuff -= healBuff;
        if(damageMultiplier != 0) dataPersistenceManager.gameData.currentDamageMultiplier /= damageMultiplier;
        
        // deletes the buff
        manager.activeBoosts -= 1;
        manager.boostList.Remove(boost);
        Destroy(gameObject);
    }
}