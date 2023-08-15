using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    public GameObject weaponImage;
    void Update()
    {
        if (GameManager.player == null) return;
        if (GameManager.player.weapon == null)
        {
            weaponImage.SetActive(false);
            return;
        }
        else weaponImage.SetActive(true);
        weaponImage.GetComponent<Image>().sprite = GameManager.player.GetComponent<Player>().weapon.GetComponent<SpriteRenderer>().sprite;
        weaponImage.GetComponent<Image>().SetNativeSize();
    }
}
