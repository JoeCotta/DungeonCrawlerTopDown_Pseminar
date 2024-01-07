using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeThrowing : MonoBehaviour, IDataPersistence
{
    [SerializeField] GameObject Granades;
    public int grenades;

    void Update()
    {
        if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("GrenadeThrow"))) && grenades > 0)
        {
            GameObject temp = Instantiate(Granades, transform.position, Quaternion.identity);
            if (temp.GetComponent<Rigidbody2D>()) temp.GetComponent<Rigidbody2D>().AddForce( (transform.up * Mathf.Sin( Mathf.Deg2Rad * (gameObject.GetComponent<Player>().angleToMouse - 180) ) + transform.right * Mathf.Cos( Mathf.Deg2Rad * (gameObject.GetComponent<Player>().angleToMouse -180) )) * 1000);
            grenades--;
        }
    }
    public void LoadData(GameData data)
    {
        this.grenades = data.grenades;
    }

    public void SaveData(ref GameData data)
    {
        data.grenades = this.grenades;
    }
}
