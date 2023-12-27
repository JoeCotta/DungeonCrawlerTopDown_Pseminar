using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    public GameObject weaponImage;
    [SerializeField] private GameObject effect;
    void Update()
    {
        if (GameManager.player == null) return;
        if (GameManager.player.weapon == null)
        {
            weaponImage.SetActive(false);
            effect.SetActive(false);
            return;
        }
        else weaponImage.SetActive(true);
        
        weaponImage.GetComponent<Image>().sprite = GameManager.player.GetComponent<Player>().weapon.GetComponent<SpriteRenderer>().sprite;
        weaponImage.GetComponent<Image>().SetNativeSize();
    

        // Particles
        int rarity = GameManager.player.weapon.GetComponent<AlternateWS>().rarity;
        effect.SetActive(true);
        switch(rarity)
        {
            case 1:
                effect.GetComponent<ParticleSystem>().startColor = Color.cyan;
                break;
            case 2:
                effect.GetComponent<ParticleSystem>().startColor = Color.magenta;
                break;
            case 3:
                effect.GetComponent<ParticleSystem>().startColor = Color.red;
                break;
            default:
                effect.SetActive(false);
                break;
            
        }
    }
}
