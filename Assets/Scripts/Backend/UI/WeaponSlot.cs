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
        weaponImage.GetComponent<Image>().sprite = AlternateWS.texture[GameManager.player.weapon.GetComponent<AlternateWS>().weaponType];
        weaponImage.GetComponent<Image>().SetNativeSize();

        /*if (GameManager.player.weapon.GetComponent<AlternateWS>().weaponType == 0)weaponImage[0].SetActive(true);
        else weaponImage[0].SetActive(false);
        if (GameManager.player.weapon.GetComponent<AlternateWS>().weaponType == 1) weaponImage[1].SetActive(true);
        else weaponImage[1].SetActive(false);
        if (GameManager.player.weapon.GetComponent<AlternateWS>().weaponType == 2) weaponImage[2].SetActive(true);
        else weaponImage[2].SetActive(false);
        if (GameManager.player.weapon.GetComponent<AlternateWS>().weaponType == 3) weaponImage[3].SetActive(true);
        else weaponImage[3].SetActive(false);*/
    }
}