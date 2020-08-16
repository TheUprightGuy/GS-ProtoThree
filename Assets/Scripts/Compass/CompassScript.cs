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

    #region Callbacks
    private void Start()
    {
        CallbackHandler.instance.setQuestObjective += SetQuestTracker;
    }

    private void OnDestroy()
    {
        CallbackHandler.instance.setQuestObjective -= SetQuestTracker;
    }
    #endregion Callbacks

    private void Update()
    {
        UpdateTarget();
    }

    public void UpdateTarget()
    {
        northDirection.y = player.eulerAngles.y;
        compass.localEulerAngles = northDirection;
        if (target == null) return;
        Vector3 dir = target.position - player.transform.position;
        pin.transform.rotation = Quaternion.LookRotation(dir);
    }

    public void SetQuestTracker(GameObject _target)
    {
        target = _target.transform;
    }
}
