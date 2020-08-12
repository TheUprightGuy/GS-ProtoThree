using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassScript : MonoBehaviour
{
    Vector3 northDirection;
    Quaternion targetDirection;

    [Header("Setup Fields")]
    public Transform player;
    public Transform compass;
    public Transform pin;
    public Transform target;

    private void Update()
    {
        UpdateTarget();
    }

    public void UpdateTarget()
    {
        northDirection.y = player.eulerAngles.y;
        compass.localEulerAngles = northDirection;

        Vector3 dir = target.position - transform.position;
        pin.transform.rotation = Quaternion.LookRotation(dir);
    }
}
