using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassRotation : MonoBehaviour
{
    Camera cam;

    public Transform whale;
    public Transform goal;
    public GameObject objective;

    public Vector3 dirToGoal;
    public Vector3 pointDir;
    public float angle;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles =  new Vector3(0, 0, cam.transform.eulerAngles.y);

        dirToGoal = new Vector3(goal.position.x, 0, goal.position.z) - new Vector3(whale.position.x, 0, whale.position.z);
        Vector3 forwardVec = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
        angle = -Vector3.SignedAngle(forwardVec, dirToGoal, Vector3.up) - cam.transform.localRotation.eulerAngles.y;
        
        objective.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
