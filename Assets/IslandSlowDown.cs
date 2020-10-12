using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSlowDown : MonoBehaviour
{
    [Header("Setup Fields")]
    [HideInInspector] public bool playerInRange = false;

    private void Start()
    {
        Invoke("SwitchConvex", 1.0f);
    }

    public void SwitchConvex()
    {
        GetComponent<MeshCollider>().convex = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Movement player = other.GetComponent<Movement>();

        if (player)
        {
            playerInRange = true;
            player.inRange = true;
            player.maxDistance = GetComponent<SphereCollider>().bounds.extents.x;
            player.orbit.leashObject = this.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Movement player = other.GetComponent<Movement>();

        if (player)
        {
            playerInRange = false;
            player.inRange = false;
        }
    }
}
