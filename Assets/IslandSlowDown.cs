using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSlowDown : MonoBehaviour
{
    [Header("Setup Fields")]
    [HideInInspector] public bool playerInRange = false;
    private bool lerp;
    public MeshCollider bottom;
    private void OnTriggerEnter(Collider other)
    {
        Movement player = other.GetComponent<Movement>();

        if (player)
        {
            playerInRange = true;
            player.inRange = true;
            player.maxDistance = GetComponent<SphereCollider>().bounds.extents.x;
            player.orbit.leashObject = this.gameObject;
            player.orbit.islandBase = bottom;
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
