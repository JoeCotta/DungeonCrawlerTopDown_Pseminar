using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapPlayer : MonoBehaviour
{
    void Start()
    {
        transform.localScale = transform.localScale * DataBase.size;
    }
}
