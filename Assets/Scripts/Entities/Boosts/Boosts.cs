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

    private bool obtained = false;

    // buffs
    float speedBuff = 0;
    float healBuff = 0; 

    float timeResetBuff = 0;
    [SerializeField] private AudioSource[] bootsEffects;

    void Start() {
        boostType = Random.Range(0, 2);
        gameObject.GetComponent<SpriteRenderer>().sprite = textures[boostType];
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        manager = GameObject.FindWithTag("Manager").GetComponent<GameManager>();
        hud = GameObject.FindWithTag("Hud");
    }


    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag != "Player" || obtained) return;

        // initializes ui animation
        GameObject boost = Instantiate(uiCoolDownPrefab, Vector2.zero, Quaternion.identity, hud.transform);

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
        }

        // playSound
        bootsEffects[boostType].Play();

        // only sets the buff which is not zero
        player.speedBuff += speedBuff;
        player.healBuff += healBuff;

        // initializes the reset
        Invoke("resetBuff", timeResetBuff);

        // starts the ui animation
        boost.GetComponent<BoostTime>().StartCooldown(timeResetBuff, textures[boostType]);

        // makes it known for other scripts that there is another boost
        manager.activeBoosts += 1;

        // makes the boost invisible and unobtainable until the buff can be destroyed after the reset
        gameObject.GetComponent<SpriteRenderer>().sprite = null;
        obtained = true;
    }

    void resetBuff()
    {
        // only resets the buff which is not zero
        player.speedBuff -= speedBuff;
        player.healBuff -= healBuff;

        // deletes the buff
        manager.activeBoosts -= 1;
        Destroy(gameObject);
    }
}