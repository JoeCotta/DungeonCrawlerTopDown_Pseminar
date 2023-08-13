using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  

public class BoostTime : MonoBehaviour
{
    [SerializeField]
    private Image imageCooldown;
    [SerializeField]
    private GameObject imageObject;

    // timer
    private float coolDownTime = 3f;
    private float coolDownTimer = 0f;

    private int activeBoostsStart;
    int activeBoostsLastRound = 0;

    void Update()
    {
        // if the time is over
        if (coolDownTimer <= 0) Destroy(gameObject);

        // sets the animation to the percentage the time elapsed 
        imageCooldown.fillAmount = coolDownTimer / coolDownTime;
        coolDownTimer -= Time.deltaTime;

        // updates the position if the UI image if necessary 
        int activeBoosts = GameObject.FindWithTag("Manager").GetComponent<GameManager>().activeBoosts;

        // print((activeBoosts, activeBoostsLastRound, activeBoostsStart));
        if (activeBoosts < activeBoostsLastRound)
        {
            // calculates the new position of the image - f.ex. 3rd ui image
            int shift = activeBoostsLastRound - activeBoosts;
            int newBoostPosition = activeBoostsStart - shift;

            // updates the postion
            Vector2 imagePosition = new Vector2(370, 55 - 50 * newBoostPosition);
            imageCooldown.GetComponent<RectTransform>().anchoredPosition = imagePosition;

            // updates the "start" position for the next update
            activeBoostsStart = newBoostPosition;
        }

        activeBoostsLastRound = activeBoosts;
    }

    // Starts the coolDown with the animation 
    public void StartCooldown(float coolDownTime, Sprite sprite)
    {
        activeBoostsStart = GameObject.FindWithTag("Manager").GetComponent<GameManager>().activeBoosts;

        // sets the position of the image Component
        Vector2 imagePosition = new Vector2(370, 55 - 50 * activeBoostsStart);
        imageCooldown.GetComponent<RectTransform>().anchoredPosition = imagePosition;

        // sets the position of the parent object to 0 0 0 to align the image
        gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        // sets the image
        imageCooldown.sprite = sprite;

        this.coolDownTime = coolDownTime;
        coolDownTimer = coolDownTime;
        imageObject.SetActive(true);
    }

// wenn oberster ui boost fertig, andere nach oben rutschen lassen
}