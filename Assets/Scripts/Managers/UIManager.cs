using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject landingButton;
    RectTransform scale;
    float scalar = 0.0f;
    float maxScalar = 0.2f;
    float timer = 0.0f;
    [HideInInspector] public bool showMe = false;
    [HideInInspector] public bool hideMe = false;
    [HideInInspector] public bool ready = false;

    // Start is called before the first frame update
    void Start()
    {
        CallbackHandler.instance.landingTooltip += LandingToggle;
        scale = landingButton.GetComponent<RectTransform>();
    }

    private void OnDestroy()
    {
        CallbackHandler.instance.landingTooltip -= LandingToggle;
    }

    public void LandingToggle(bool _toggle)
    {
        if (_toggle)
        {
            showMe = true;
            hideMe = false;
        }
        else
        {
            hideMe = true;
            showMe = false;
        }
    }

    private void Update()
    {
        if (showMe)
        {
            ShowMe();
        }
        else if (hideMe)
        {
            HideMe();
        }
        else if (ready)
        {
            PingPong();
        }
    }

    public void ShowMe()
    {
        scalar += Time.deltaTime * 2;
        scale.localScale = new Vector3(scalar, scalar, 1.0f);
        if (scalar >= 1.0f)
        {
            scalar = 1.0f;
            showMe = false;
            ready = true;
            timer = scalar - 0.8f;
        }
    }

    public void HideMe()
    {
        scalar -= Time.deltaTime * 2;
        scale.localScale = new Vector3(scalar, scalar, 1.0f);
        if (scalar <= 0)
        {
            scalar = 0;
            scale.localScale = new Vector3(scalar, scalar, 1.0f);
            hideMe = false;
            ready = false;
        }
    }

    public void PingPong()
    {
        scalar = Mathf.PingPong(timer, maxScalar) + 0.8f;
        timer += Time.deltaTime / 4;

        scale.localScale = new Vector3(scalar, scalar, 1.0f);
    }

    public void Orbit()
    {
        CallbackHandler.instance.Orbit(true);
    }
}
