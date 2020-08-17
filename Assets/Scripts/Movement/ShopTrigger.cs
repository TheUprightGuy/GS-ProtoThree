using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        WhaleMovementScript player = other.GetComponent<WhaleMovementScript>();

        if (player)
        {
            CallbackHandler.instance.ToggleShop(true);
            EventHandler.instance.HighlightObjective(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WhaleMovementScript player = other.GetComponent<WhaleMovementScript>();

        if (player)
        {
            CallbackHandler.instance.ToggleShop(false);
        }
    }
}
