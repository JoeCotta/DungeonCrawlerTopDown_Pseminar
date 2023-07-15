using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimNumberGen1 : MonoBehaviour
{
    void Start()
    {
        //Debug.Log(Prim(1000000000));
        //Debug.Log(ReturnHighestPrim(1000000000));
        //Debug.Log(ReturnRandomPrimWithNDigits(9));
    }


    public List<int> Prim(int end)
    {
        List<int> primNumbers = new List<int>();
        bool[] marker;
        if (end > 1) marker = new bool[end];
        else { Debug.LogError("Below two *angry*"); return null; }

        for (int i = 2; i <= end; i++)
        {
            if (!marker[i - 1])
            {
                primNumbers.Add(i);
                for (int j = i; j * i <= end && j*i > 0; j++) // j*i > 0 is a hack not a fix for kind wierd bug going on with int overflow
                {
                    if (!marker[i * j - 1] && i * j <= end) marker[i * j - 1] = true;
                }
            }
        }
        return primNumbers;
    }

    public int ReturnHighestPrim(int end)
    {
        List<int> tempList = Prim(end);
        return tempList[tempList.Count-1];
    }

    public int ReturnRandomPrimWithNDigits(int amountOfDigits)
    {
        int tempLimitUpper = Mathf.RoundToInt(Mathf.Pow(10, amountOfDigits));//cant go higher than 9 digits cause int overflow
        int tempLimitLower = Mathf.RoundToInt(Mathf.Pow(10, amountOfDigits-1));//"
        List<int> tempWholeListRandom = Prim(tempLimitUpper);
        List<int> tempListRandom = new List<int>();

        for(int h = tempWholeListRandom.Count-1; h > 0; h--)
        {
            if(tempWholeListRandom[h] > tempLimitLower)
            {
                tempListRandom.Add(tempWholeListRandom[h]);
            }
        }
        return tempListRandom[Random.Range(0, tempListRandom.Count)];
    }
}
