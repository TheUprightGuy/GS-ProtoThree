using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandTrigger : MonoBehaviour
{

    static bool PopUpDone = false;

    [Header("Setup Fields")]
    public float lineWidth = 0.2f;
    public Material material;
    // Local Variables
    [HideInInspector] public LineRenderer lineRenderer;
    [HideInInspector] public bool playerInRange = false;
    [HideInInspector] public float radius;
    int vertexCount = 40;
    private Color lerpColor;
    private bool lerp;
    private float lerpTimer;

    #region Setup
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (!lineRenderer)
        {
            lineRenderer = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
            lineRenderer.material = material;
            lineRenderer.positionCount = 0;
            lineRenderer.loop = true;
        }
    }
    #endregion Setup
    #region Callbacks
    private void Start()
    {
        CallbackHandler.instance.turnOffOrbit += TurnOffOrbit;
    }

    private void OnDestroy()
    {
        CallbackHandler.instance.turnOffOrbit -= TurnOffOrbit;
    }
    #endregion Callbacks

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
            if (!PopUpDone)
            {
                PopUpDone = true;
                PopUpHandler.instance.QueuePopUp("Press Space to orbit the island", KeyCode.Space);
                PopUpHandler.instance.QueuePopUp("Any resources found will be collected while orbiting an island", 7.0f);
            }

            playerInRange = true;
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
            SetupCircle(Vector3.Distance(player.transform.position, transform.position), player.transform.position.y - 1.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WhaleMovementScript player = other.GetComponent<WhaleMovementScript>();

        if (player)
        {
            playerInRange = false;
            player.inRange = false;
            //player.orbit.leashObject = null;
            //lineRenderer.positionCount = 0;
            CallbackHandler.instance.LandingTooltip(false);
            //ToggleLeashed(false);
        }
    }

    public void TurnOffOrbit()
    {
        lineRenderer.positionCount = 0;
        ToggleLeashed(false);
    }

    public void SetupCircle(float _radius, float _y)
    {
        if (lineRenderer)
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
    }

    public void BlendLineColor()
    {
        if (lineRenderer)
        {
            lineRenderer.material.color = Color.Lerp(lineRenderer.material.color, lerpColor, Time.deltaTime);
            if (lerpTimer <= 0)
            {
                lerp = false;
            }
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
