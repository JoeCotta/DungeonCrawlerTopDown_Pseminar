using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boosts : MonoBehaviour
{
    public int boostType;
    public Sprite[] textures;

    private float toReset;
    private Player player;

    void Start() {
        boostType = Random.Range(0, 2);
        gameObject.GetComponent<SpriteRenderer>().sprite = textures[boostType];
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }


    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag != "Player") return;

        switch (boostType)
        {
            // speedBoost
            case 0:
                player.speedBuff += 5;
                Invoke("resetBuff", 3);
                break;
            // healBoost
            case 1:
                player.healBuff += 3f;
                Invoke("resetBuff", 3);
                break;
        }

        gameObject.GetComponent<SpriteRenderer>().sprite = null;
    }

    void resetBuff()
    {
        switch (boostType)
        {
            // speedBoost
            case 0: player.speedBuff = 0; break;
            // healBoost
            case 1: player.healBuff = 0; break;
        }
        Destroy(gameObject);
    }
}