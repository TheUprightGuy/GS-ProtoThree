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

    public Button lampButton;
    public Button toolsButton;
    public Button tradeButton;
    public TMPro.TextMeshProUGUI provisions;
    public TMPro.TextMeshProUGUI supplies;

    Color interactable = new Color(1,1,1,1);
    Color nonInteractable = new Color(1, 1, 1, 0.5f);

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

        UpdateButtons();
    }

    public void UpdateButtons()
    {
        int provisionCount = ResourceDisplayScript.instance.provisions;
        int supplyCount = ResourceDisplayScript.instance.supplies;
        provisions.SetText(provisionCount.ToString());
        supplies.SetText(supplyCount.ToString());

        lampButton.interactable = (supplyCount >= 5);
        // DON'T JUDGE ME 
        if (!lampButton.interactable)
        {
            Image[] images = lampButton.gameObject.GetComponentsInChildren<Image>();
            foreach (Image n in images)
            {
                n.color = nonInteractable;
            }
        }
        else
        {
            Image[] images = lampButton.gameObject.GetComponentsInChildren<Image>();
            foreach (Image n in images)
            {
                n.color = interactable;
            }
        }
        toolsButton.interactable = (supplyCount >= 2);
        if (!toolsButton.interactable)
        {
            Image[] images = toolsButton.gameObject.GetComponentsInChildren<Image>();
            foreach (Image n in images)
            {
                n.color = nonInteractable;
            }
        }
        else
        {
            Image[] images = toolsButton.gameObject.GetComponentsInChildren<Image>();
            foreach (Image n in images)
            {
                n.color = interactable;
            }
        }
        tradeButton.interactable = (supplyCount >= 1);
        if (!tradeButton.interactable)
        {
            Image[] images = tradeButton.gameObject.GetComponentsInChildren<Image>();
            foreach (Image n in images)
            {
                n.color = nonInteractable;
            }
        }
        else
        {
            Image[] images = tradeButton.gameObject.GetComponentsInChildren<Image>();
            foreach (Image n in images)
            {
                n.color = interactable;
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
        if (ResourceDisplayScript.instance.SpendSupplies(5))
        {
            Debug.Log("Purchased a Lamp");
            EventHandler.instance.OnLampBought();
            CallbackHandler.instance.SetQuestObjective(null);
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
