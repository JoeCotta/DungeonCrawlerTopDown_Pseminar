using Pathfinding.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletCalc : MonoBehaviour
{
    public int maxIterations;
    private Vector3 tempPos;

    //could be better but should work
    public Vector3 CalcPath(float speed, GameObject target)
    {
        //reset variable to random low time
        float currentTravelTime = 0;

        //find position of player at that point in the future 
        //then calculate time bullet needs to there 
        //calculate new position of player with new traveltime => travel time changes; repeat
        for(int iteration = 0;  iteration <= maxIterations; iteration++) {
            tempPos = new Vector3();
            float tempDistance = 0;
            tempPos = target.GetComponent<BasicPred>().predictPosition(currentTravelTime);
            tempDistance += Mathf.Abs(tempPos.x - transform.position.x);
            tempDistance += Mathf.Abs(tempPos.y - transform.position.y);
            currentTravelTime = tempDistance / speed;
        }
        if (currentTravelTime > 100) return target.transform.position;
        else return tempPos;
        //Debug.DrawLine(transform.position, tempPos, Color.red, 10f);

    }
}
