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

        Vector3 dir = new Vector3(player.position.x, 0, player.position.z) - new Vector3(target.position.x, 0, target.position.z);
        float angle = Vector3.Angle(player.forward, dir) + Camera.main.transform.localRotation.eulerAngles.y;
        pin.localRotation = Quaternion.Euler(new Vector3(0, -angle, 0));
    }

    public void SetQuestTracker(GameObject _target)
    {
        target = _target.transform;
    }
}
