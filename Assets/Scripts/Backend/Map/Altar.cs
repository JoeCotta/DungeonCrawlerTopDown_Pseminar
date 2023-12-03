using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        transform.GetChild(0).gameObject.SetActive(true);
        if (Input.GetKey("e"))
        {
            GameManager.player.playerGold += 100;
            GameManager.resetRunData();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //disable text
        if (!collision.gameObject.CompareTag("Player")) return;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
