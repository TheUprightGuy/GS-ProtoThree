using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandTrigger : MonoBehaviour
{
    [HideInInspector]
    public bool playerInRange = false;
    private void OnTriggerEnter(Collider other)
    {
        WhaleMovementScript player = other.GetComponent<WhaleMovementScript>();

        if (player)
        {
            playerInRange = true;
            player.inRange = true;
            player.maxDistance = GetComponent<SphereCollider>().bounds.extents.x;
            player.orbit.leashObject = this.gameObject;

            CallbackHandler.instance.LandingTooltip(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WhaleMovementScript player = other.GetComponent<WhaleMovementScript>();

        if (player)
        {
            playerInRange = false;
            player.inRange = false;
            //player.orbit.leashObject = null;

            CallbackHandler.instance.LandingTooltip(false);
        }
    }
}
