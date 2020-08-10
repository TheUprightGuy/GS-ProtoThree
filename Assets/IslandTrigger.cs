using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        WhaleRotateScript player = other.GetComponent<WhaleRotateScript>();

        if (player)
        {
            player.inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WhaleRotateScript player = other.GetComponent<WhaleRotateScript>();

        if (player)
        {
            player.inRange = false;
        }
    }
}
