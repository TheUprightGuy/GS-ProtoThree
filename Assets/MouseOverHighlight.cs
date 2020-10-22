using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverHighlight : MonoBehaviour
{
    public static MouseOverHighlight instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one MouseOverHighlight Exists!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public LayerMask layerMask;
    public PuzzleSwitch highlightedSwitch;
    public ShopItem highlightedShopItem;

    // Update is called once per frame
    void Update()
    {
        highlightedSwitch = null;
        highlightedShopItem = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);

        foreach (RaycastHit n in hits)
        {
            PuzzleSwitch tempSwitch = n.collider.gameObject.GetComponent<PuzzleSwitch>();
            ShopItem tempItem = null;
            if (CallbackHandler.instance.inShopRange)
            {
                tempItem = n.collider.gameObject.GetComponent<ShopItem>();
            }

            if (tempSwitch)
            {
                highlightedSwitch = tempSwitch;
            }
            if (tempItem)
            {
                highlightedShopItem = tempItem;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (highlightedSwitch)
            {
                highlightedSwitch.Use();
            }
            if (highlightedShopItem)
            {
                highlightedShopItem.ShowUI();
            }
        }
    }
}
