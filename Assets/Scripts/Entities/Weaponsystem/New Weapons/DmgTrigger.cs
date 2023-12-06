using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgTrigger : MonoBehaviour
{
    float   dmg = 1;
    float   range = 1;
    float   duration = 0;
    float   time = 0;
    bool    dmgOverTime = false;

    bool triggerExtraFunction;
    string functionName;
    public void assingVar(float dmg, float range, float effectDuration,bool dmgOverTime, bool triggerExtraScript, string functionName)
    {
        this.range          = range;
        this.duration       = effectDuration;
        this.dmgOverTime    = dmgOverTime;
        if (dmgOverTime)    this.dmg = dmg / 50; //dmg per second -> 50 fixed updates
        else                this.dmg = dmg;
        this.triggerExtraFunction = triggerExtraScript;
        this.functionName = functionName;
    }

    private void Start()
    {
        if (transform.localScale != new Vector3(range, range, 1)) transform.localScale = new Vector3(range, range, 1);
        if (dmgOverTime) GetComponent<SpriteRenderer>().enabled = true;
    }

    private void LateUpdate()
    {
        if (!dmgOverTime || time >= duration)
        {
            StartCoroutine(DestroyOnFixedUpdate());
        }
        time += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" && collision.tag != "Enemy") return;
        float distance = (transform.position - collision.transform.position).magnitude;
        collision.SendMessage("hit", dmg / distance);

        //Here it should trigger the extra funktion i think
        //for example a nade that pulls enemys towards the middle
        if (triggerExtraFunction && functionName != null) gameObject.SendMessage(functionName);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Player" && collision.tag != "Enemy") return;
        collision.SendMessage("hit", dmg);
        Debug.Log("Hit");
    }

    IEnumerator DestroyOnFixedUpdate()
    {
        yield return new WaitForFixedUpdate();
        Destroy(gameObject);
    }
}
