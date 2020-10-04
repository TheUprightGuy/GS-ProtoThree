﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPuzzleMaster : MonoBehaviour
{
    public static SwitchPuzzleMaster instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Another Puzzle Master exists!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public List<PuzzleSwitch> switches;
    public Material on;
    public Material off;
    public bool complete;

    private void Start()
    {
        foreach (Transform n in transform)
        {
            switches.Add(n.GetComponent<PuzzleSwitch>());
        }

        foreach (PuzzleSwitch n in switches)
        {
            n.active = Random.value > 0.5f;
            n.on = on;
            n.off = off;
            n.Switch();
        }
    }

    public bool CheckComplete()
    {
        foreach(PuzzleSwitch n in switches)
        {
            if (!n.active)
            {
                return false;
            }
        }
        complete = true;
        return true;
    }
}