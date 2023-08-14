using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapPlayer : MonoBehaviour
{
    //changes size and destorys script
    private void Update() { if (DataBase.size > 0) { transform.localScale = transform.localScale * DataBase.size; Destroy(this); } }
}
