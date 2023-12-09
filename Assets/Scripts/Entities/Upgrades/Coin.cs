using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    [SerializeField] private AudioSource pickUpSound;
    [SerializeField] private float SpawnForce;
    bool isPickedUp = false;
    Renderer rend;
    Rigidbody2D rb;
    [HideInInspector] public int valueCoin;

    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        // give random Force;
        rb.AddForce(Random.insideUnitCircle.normalized * Random.Range(SpawnForce * 0.3f, SpawnForce));
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag != "Player" || isPickedUp) return;
        isPickedUp = true;
        pickUpSound.Play();
        rend.enabled = false;
        Invoke("onAudioEnd", 0.2f);
        GameManager.player.playerGold += valueCoin;
    }

    void onAudioEnd()
    {
        if(!pickUpSound.isPlaying) Destroy(gameObject);
        else Invoke("onAudioEnd", 0.2f);
    }
}
