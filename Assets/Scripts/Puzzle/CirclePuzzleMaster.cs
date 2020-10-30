using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePuzzleMaster : MonoBehaviour
{
    public List<CirclePuzzle> circlePuzzles;
    public ObeliskRise obelisk;

    public float speed = 60;
    public bool inUse = false;
    public bool completed;

    private void Start()
    {
        int[] symbols = { 0, 0, 0 };

        foreach(CirclePuzzle n in circlePuzzles)
        {
            n.speed = speed;
            n.parent = this;

            foreach(CircleSymbol x in n.sharedObjects)
            {
                if (x.symbol == Symbol.NotAssigned)
                {
                    int temp = Random.Range(0, 3);
                    while (symbols[temp] > 3)
                    {
                        temp = Random.Range(0, 3);
                    }
                    symbols[temp] += 1;

                    x.Setup((Symbol)temp);
                }
            }
        }
    }

    public bool CheckComplete()
    {
        int temp = 0;

        foreach(CirclePuzzle n in circlePuzzles)
        {
            if (n.CheckFull())
            {
                temp++;
            }
        }

        // temp
        if (temp >= 2)
        {
            completed = true;
            obelisk.StartAnim();
        }

        return (temp >= 2);
    }
}
