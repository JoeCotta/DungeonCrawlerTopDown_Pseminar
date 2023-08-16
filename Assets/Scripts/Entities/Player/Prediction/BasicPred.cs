using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPred : MonoBehaviour
{
    //INFO
    //positions[0] is the current position
    //movement[0] is the most recent


    public GameObject trackedObj, indicator;
    private Vector3[] positions, movement;
    private float[] times;
    private Vector3 pred1Frame;
    public float estimatedSpeed;

    private void Start()
    {
        positions = new Vector3[50];
        movement = new Vector3[positions.Length-1];
        times = new float[positions.Length];
    }
    private void LateUpdate()
    {
        StartCoroutine(Check());
    }

    IEnumerator Check()
    {
        //wait for after player movement and reset speed estmimate
        yield return new WaitForEndOfFrame();
        estimatedSpeed = 0;

        //reorder arrays so that newest value is in 0 and oldest gets kicked
        //after that add newest value in 0
        for(int i = positions.Length - 2; i >= 0; i--)
        {
            positions[i+1] = positions[i];
            times[i + 1] = times[i];
        }
        positions[0] = trackedObj.transform.position;
        times[0] = Time.deltaTime;

        //calculate Vector 3 between positions 
        //finding avarage direction: add to total, normalize later 
        //also avrage out speed of last movements in units/second
        for(int i = 0; i < positions.Length-1; i++)
        {
            movement[i] = positions[i] - positions[i+1];
            pred1Frame += movement[i];
            estimatedSpeed += movement[i].magnitude / times[i];
        }
        pred1Frame /= movement.Length;
        estimatedSpeed /= movement.Length;
        
        //just for viusalisation and will throw like 44 errors every time but dont worry about those they are only on start up
        //indicator.transform.position = predictPosition(1); 
    }

    //acces this function from other scripts to use position
    public Vector3 predictPosition(float time)
    {
        Vector3 temp = new Vector3();
        temp = pred1Frame.normalized * time * estimatedSpeed;
        temp += trackedObj.transform.position;
        return temp;
    }
}
