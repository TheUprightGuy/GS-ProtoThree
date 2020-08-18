using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIScript : MonoBehaviour
{
    [Header("Required Fields")]
    public RectTransform scale;

    public GameObject LampBeam;
    // Local Variables
    bool showMe = false;
    bool ready = false;
    float scalar = 0.0f;

    #region Callbacks
    private void Start()
    {
        CallbackHandler.instance.toggleShop += ToggleShop;
    }

    private void OnDestroy()
    {
        CallbackHandler.instance.toggleShop -= ToggleShop;
    }
    #endregion Callbacks

    private void Update()
    {
        if (!ready)
        {
            if (showMe)
            {
                ShowMe();
            }
            else
            {
                HideMe();
            }
        }
    }

    public void ToggleShop(bool _toggle)
    {
        ready = false;
        showMe = _toggle;
    }

    public void ShowMe()
    {
        scalar += Time.deltaTime * 4;
        scale.localScale = new Vector3(scalar, scalar, 1.0f);
        if (scalar >= 1.0f)
        {
            scalar = 1.0f;
            scale.localScale = new Vector3(scalar, scalar, 1.0f);
            ready = true;
        }
    }

    public void HideMe()
    {
        scalar -= Time.deltaTime * 4;
        scale.localScale = new Vector3(scalar, scalar, 1.0f);
        if (scalar <= 0.0f)
        {
            scalar = 0.0f;
            scale.localScale = new Vector3(scalar, scalar, 1.0f);
            ready = true;
        }
    }

    public void SuppliesToProvisions()
    {
        if (ResourceDisplayScript.instance.SpendSupplies(1) && ResourceDisplayScript.instance.provisions < ResourceDisplayScript.instance.provisionsMax)
        {
            ResourceDisplayScript.instance.AddProvisions(1);
            Debug.Log("Purchased provisions");
        }
    }

    public void Saddle()
    {
        if (ResourceDisplayScript.instance.SpendSupplies(3))
        {
            Debug.Log("Purchased a Saddle");
        }
    }

    public void Lamp()
    {
        if (ResourceDisplayScript.instance.SpendSupplies(8))
        {
            Debug.Log("Purchased a Lamp");
            LampBeam.SetActive(true);
        }
    }

    public void Tools()
    {
        if (ResourceDisplayScript.instance.SpendSupplies(2))
        {
            Debug.Log("Purchased some Tools");
        }
    }
}
