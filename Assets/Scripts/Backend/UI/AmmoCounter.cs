using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCounter : MonoBehaviour
{
    private Player player;
    private void Start()
    {
        Invoke("References", 1f);
    }
    void FixedUpdate()
    {
<<<<<<< HEAD

        if (player && player.weapon && player.weapon.GetComponent<AlternateWS>()) gameObject.GetComponent<TextMeshProUGUI>().text = player.weapon.GetComponent<AlternateWS>().GetAmmo().x.ToString() + "/" + player.weapon.GetComponent<AlternateWS>().GetAmmo().y.ToString();
=======
        if (player && GameManager.player.weapon != null && player.weapon.GetComponent<AlternateWS>()) gameObject.GetComponent<TextMeshProUGUI>().text = player.weapon.GetComponent<AlternateWS>().GetAmmo().x.ToString() + "/" + player.weapon.GetComponent<AlternateWS>().GetAmmo().y.ToString();
>>>>>>> 0aa1fda9837901be8e34c8900026599092aa7697
        else gameObject.GetComponent<TextMeshProUGUI>().text = "";
    }

    void References()
    {
        player = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().returnPlayer();
    }
}
