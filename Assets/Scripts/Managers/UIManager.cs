using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject landingButton;

    // Start is called before the first frame update
    void Start()
    {
        CallbackHandler.instance.landingTooltip += LandingToggle;
        landingButton.SetActive(false);
    }

    private void OnDestroy()
    {
        CallbackHandler.instance.landingTooltip -= LandingToggle;
    }

    public void LandingToggle(bool _toggle)
    {
        landingButton.SetActive(_toggle);
    }
}
