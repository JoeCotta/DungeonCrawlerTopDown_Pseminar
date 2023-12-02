using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SussyScript : MonoBehaviour
{
    private Sprite sussy_model;

    void Start()
    {
        StartCoroutine(SussyRoutine());
    }

    private void SetSprite()
    {
        if (!GameManager.enableSusMode) return;
        sussy_model = GameManager.sus_Small;
        gameObject.GetComponent<SpriteRenderer>().sprite = sussy_model;
    }

    IEnumerator SussyRoutine()
    {
        yield return new WaitForEndOfFrame();
        SetSprite();
    }
}
