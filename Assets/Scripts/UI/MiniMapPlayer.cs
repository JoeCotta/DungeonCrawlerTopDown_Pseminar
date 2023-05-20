using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapPlayer : MonoBehaviour
{
    public RoomTemplates roomTemplates;

    void Start()
    {
        roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        transform.localScale = transform.localScale * roomTemplates.size;
    }
}
