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

    [Header("Debug")]
    public float angle;

    #region Callbacks
    private void Start()
    {
        CallbackHandler.instance.setQuestObjective += SetQuestTracker;
        if (!target)
        {
            pin.gameObject.SetActive(false);
        }
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
        if (target == null) return;

        Vector3 dir = new Vector3(player.position.x, 0, player.position.z) - new Vector3(target.position.x, 0, target.position.z);

        Vector3 path = Vector3.Normalize(Vector3.Cross(dir, Vector3.up));

        if (Vector3.Dot(player.forward, path) <= 0.0f)
        {
            angle = -(Vector3.Angle(player.forward, dir)) - Camera.main.transform.localRotation.eulerAngles.y;
        }
        else
        {
            angle = Vector3.Angle(player.forward, dir) - Camera.main.transform.localRotation.eulerAngles.y;
        }

        pin.localRotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }

    public void SetQuestTracker(GameObject _target)
    {
        if (_target != null)
        {
            pin.gameObject.SetActive(true);
            target = _target.transform;
        }
        else
        {
            pin.gameObject.SetActive(false);
        }
    }
}
