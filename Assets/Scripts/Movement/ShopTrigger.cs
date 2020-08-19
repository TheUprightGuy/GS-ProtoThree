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
                PopUpHandler.instance.QueuePopUp("Use the <b>scroll wheel</b> to switch to first person view", 6.0f);
                PopUpHandler.instance.QueuePopUp("You can purchase items while orbiting in this view", 6.0f);
                PopUpHandler.instance.QueuePopUp("A <b>lamp</b> can help guide us to objectives", 6.0f);
            }
            StartCoroutine(EventHandler.instance.HighlightObjective(1));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WhaleMovementScript player = other.GetComponent<WhaleMovementScript>();

        if (player)
        {
            shopPopUpDone = true;
            CallbackHandler.instance.ToggleShop(false);
        }
    }
}
