using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassUIScript : MonoBehaviour
{
    public Vector3 northDirection;
    public Transform player;
    public Quaternion targetDirection;

    public RectTransform northLayer;
    public RectTransform targetLayer;
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
        UpdateUI();      
    }

    public void UpdateUI()
    {
        northDirection.z = player.eulerAngles.y;
        northLayer.localEulerAngles = northDirection;

        if (target == null) return;
        Vector3 dir = player.transform.position - target.position;
        targetDirection = Quaternion.LookRotation(dir);

        targetDirection.z = -targetDirection.y;
        targetDirection.x = 0;
        targetDirection.y = 0;
        targetLayer.localRotation = targetDirection;
    }

    public void SetQuestTracker(GameObject _target)
    {
        target = _target.transform;
    }
}
