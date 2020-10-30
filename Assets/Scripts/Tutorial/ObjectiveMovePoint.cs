using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveMovePoint : MonoBehaviour
{
    public Transform lookAtTransform;
    public int ObjectiveIndex;
    void Start()
    {
        var objectiveData = new CinematicController.ObjectiveData
        {
            moveTo = transform, lookAt = lookAtTransform, index = ObjectiveIndex
        };
        CinematicController.instance.objectives.Add(objectiveData);
    }
}
