using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        WhaleMovementScript player = other.GetComponent<WhaleMovementScript>();

        if (player)
        {
            player.inRange = true;
            player.orbit.leashObject = this.gameObject;

            CallbackHandler.instance.LandingTooltip(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WhaleMovementScript player = other.GetComponent<WhaleMovementScript>();

        if (player)
        {
            player.inRange = false;
            player.orbit.leashObject = null;

            CallbackHandler.instance.LandingTooltip(false);
        }
    }
}
