using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armour : MonoBehaviour
{
    public int level;

    // Start is called before the first frame update
    void Start()
    {
        Mathf.Clamp(level, 0, 10);
    }

    void setLevel(int level)
    {
        this.level = Mathf.Clamp(level, 0, 10);
    }
}
