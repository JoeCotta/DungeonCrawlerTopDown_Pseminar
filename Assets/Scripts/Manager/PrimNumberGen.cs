using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public int endNumber;
    public List<int> primNumbers;
    public bool[] marker;
    void Start()
    {
        Prim();
    }

    
    public void Prim()
    {
        if (endNumber > 1) marker = new bool[endNumber];
        else Debug.LogError("Below one *angry*");

        for(int i = 2; i <= endNumber; i++)
        {
            if (!marker[i-1])
            {
                primNumbers.Add(i);
                for(int j = i; j * i <= endNumber; j++)
                {
                    if(!marker[i*j-1] && i*j <= endNumber) marker[i * j-1] = true;
                }
            }
        }
    }
}
