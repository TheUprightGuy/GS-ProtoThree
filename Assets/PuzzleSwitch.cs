using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSwitch : MonoBehaviour
{

    public List<PuzzleSwitch> adjacentSwitches;

    [HideInInspector] public bool active;
    [HideInInspector] public Material on;
    [HideInInspector] public Material off;

    MeshRenderer meshRenderer;

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
    }

    public void Switch()
    {
        if (!SwitchPuzzleMaster.instance.complete)
        {
            active = !active;
            meshRenderer.material = active ? on : off;
        }
    }

    private void OnMouseDown()
    {
        Use();
        if (SwitchPuzzleMaster.instance.CheckComplete())
        {
            Debug.Log("Complete!");
        }
    }
}
