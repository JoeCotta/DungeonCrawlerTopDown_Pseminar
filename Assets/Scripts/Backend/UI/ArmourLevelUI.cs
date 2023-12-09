using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ArmourLevelUI : MonoBehaviour
{
    [SerializeField] private Sprite[] textures;
    [SerializeField] private TMP_Text text;
    private int level = 0;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.player) level = GameManager.player.armourLevel;

        if (level == 0)
        {
            gameObject.GetComponent<Image>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<Image>().enabled = true;
            gameObject.GetComponent<Image>().sprite = textures[level - 1];
        }
        text.text = level == 0 ? "" : level.ToString();
    }
}
