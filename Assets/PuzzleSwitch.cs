using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSwitch : MonoBehaviour
{
    public List<PuzzleSwitch> adjacentSwitches;

    //[HideInInspector] 
    public bool active;
    [HideInInspector] public Material on;
    [HideInInspector] public Material off;

    public MeshRenderer meshRenderer;
    public bool highlight;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Use()
    {
        Switch();

        foreach (PuzzleSwitch n in adjacentSwitches)
        {
            n.Switch();
        }

        if (SwitchPuzzleMaster.instance.CheckComplete())
        {
            Debug.Log("Complete!");
        }
    }

    public void Switch()
    {
        if (!SwitchPuzzleMaster.instance.complete)
        {
            active = !active;
            meshRenderer.material = active ? on : off;
        }
    }

    private void Update()
    {
        if (MouseOverHighlight.instance.highlightedSwitch == this)
        {
            meshRenderer.material.SetFloat("Boolean_55E471DA", 1.0f);
        }
        else
        {
            meshRenderer.material.SetFloat("Boolean_55E471DA", 0.0f);
        }
    }
}
