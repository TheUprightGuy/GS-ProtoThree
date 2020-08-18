using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    static bool shopPopUpDone = false;
    private void OnTriggerEnter(Collider other)
    {
        WhaleMovementScript player = other.GetComponent<WhaleMovementScript>();

        if (player)
        {
            if (!shopPopUpDone)
            {
                PopUpHandler.instance.QueuePopUp("Use the scroll wheel to switch to first person view", 6.0f);
                PopUpHandler.instance.QueuePopUp("You can purchase items from the shop in this view", 6.0f);
            }
            CallbackHandler.instance.ToggleShop(true);
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
