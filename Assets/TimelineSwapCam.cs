using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TimelineSwapCam : MonoBehaviour
{
    public CinemachineBrain brain;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDisable()
    {
        brain.ActiveBlend.CamA.Priority = 0;
        brain.ActiveBlend.CamB.Priority = 1;
    }
}
