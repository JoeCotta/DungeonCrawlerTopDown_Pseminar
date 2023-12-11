using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondweaponSlot : MonoBehaviour
{
    public GameObject weaponImage;
    void Update()
    {
        if (GameManager.player == null) return;
        if (GameManager.player.secondWeapon == null)
        {
            weaponImage.SetActive(false);
            return;
        }
        else weaponImage.SetActive(true);
        weaponImage.GetComponent<Image>().sprite = GameManager.player.GetComponent<Player>().secondWeapon.GetComponent<SpriteRenderer>().sprite;
        weaponImage.GetComponent<Image>().SetNativeSize();
    }
}
