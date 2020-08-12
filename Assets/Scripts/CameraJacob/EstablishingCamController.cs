using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstablishingCamController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventHandler.instance.startEstablishingShot += () => transform.GetChild(0).gameObject.SetActive(true);
        EventHandler.instance.play += () => transform.GetChild(0).gameObject.SetActive(false);
    }
}
