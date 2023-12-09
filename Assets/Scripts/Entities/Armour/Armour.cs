using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armour : MonoBehaviour
{
    public int level;
    [SerializeField] private Sprite[] textures;
    
    void Start()
    {
        Mathf.Clamp(level, 0, 10);
    }

    void setLevel(int level)
    {
        this.level = Mathf.Clamp(level, 0, 10);
        setSprite();
    }

    void setSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = textures[level - 1];
    }
}
