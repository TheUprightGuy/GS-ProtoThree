using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandTrigger : MonoBehaviour
{
    public int vertexCount = 40;
    public float lineWidth = 0.2f;
    public float radius;

    public LineRenderer lineRenderer;
    private Color lerpColor;
    private bool lerp;
    private float lerpTimer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (lerp)
        {
            lerpTimer -= Time.deltaTime;
            BlendLineColor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        WhaleMovementScript player = other.GetComponent<WhaleMovementScript>();

        if (player)
        {
            player.inRange = true;
            player.maxDistance = GetComponent<SphereCollider>().bounds.extents.x;
            player.orbit.leashObject = this.gameObject;

            CallbackHandler.instance.LandingTooltip(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        WhaleMovementScript player = other.GetComponent<WhaleMovementScript>();

        if (player && !player.whaleInfo.leashed)
        {
            SetupCircle(Vector3.Distance(player.transform.position, transform.position), player.transform.position.y);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WhaleMovementScript player = other.GetComponent<WhaleMovementScript>();

        if (player)
        {
            player.inRange = false;
            //player.orbit.leashObject = null;
            //lineRenderer.positionCount = 0;
            CallbackHandler.instance.LandingTooltip(false);
            //ToggleLeashed(false);
        }
    }

    public void SetupCircle(float _radius, float _y)
    {
        radius = _radius;
        lineRenderer.widthMultiplier = lineWidth;

        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        lineRenderer.positionCount = vertexCount;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 pos = new Vector3(transform.position.x + radius * Mathf.Cos(theta), _y, transform.position.z + radius * Mathf.Sin(theta));
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }

    public void BlendLineColor()
    {
        lineRenderer.material.color = Color.Lerp(lineRenderer.material.color, lerpColor, Time.deltaTime);
        if (lerpTimer <= 0)
        {
            lerp = false;
        }
    }

    public void ToggleLeashed(bool _toggle)
    {
        if (_toggle)
        {
            lerpColor = Color.yellow;
        }
        else
        {
            lerpColor = Color.white;
        }
        lerpColor.a = 0.95f;
        lerp = true;
        lerpTimer = 2.0f;
    }
}
