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

    private void OnMouseOver()
    {

        meshRenderer.material.SetFloat("Boolean_55E471DA", 1.0f);
    }

    private void OnMouseExit()
    {
        meshRenderer.material.SetFloat("Boolean_55E471DA", 0.0f);
    }

}
