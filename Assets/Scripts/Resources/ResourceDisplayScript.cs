using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDisplayScript : MonoBehaviour
{
    #region Singleton
    public static ResourceDisplayScript instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one ResourceDisplay exists!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion Singleton

    // temp
    [HideInInspector] public int supplies;
    [HideInInspector] public int suppliesMax;
    float suppliesPercentage;
    [HideInInspector] public int provisions;
    [HideInInspector] public int provisionsMax;
    float provisionsPercentage;

    public List<GameObject> supplyObjs;
    public List<GameObject> provisionObjs;

    private void Start()
    {
        suppliesMax = supplyObjs.Count;
        provisionsMax = provisionObjs.Count;
        DisplayResources();
    }

    private void Update()
    {
        // testing
        if (Input.GetKeyDown(KeyCode.C))
        {
            AddSupplies(1);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            AddProvisions(1);
        }
    }

    public void DisplayResources()
    {
        DisplayProvisions();
        DisplaySupplies();
    }


    public void DisplaySupplies()
    {
        for (int i = 0; i < supplyObjs.Count; i++)
        {
            if (i < Mathf.FloorToInt((float)supplyObjs.Count * suppliesPercentage))
            {
                supplyObjs[i].SetActive(true);
            }
            else
            {
                supplyObjs[i].SetActive(false);
            }
        }
    }

    public void DisplayProvisions()
    {
        for (int i = 0; i < provisionObjs.Count; i++)
        {
            if (i < Mathf.FloorToInt((float)provisionObjs.Count * provisionsPercentage))
            {
                provisionObjs[i].SetActive(true);
            }
            else
            {
                provisionObjs[i].SetActive(false);
            }
        }
    }


    public void AddSupplies(int _supplies)
    {
        supplies += _supplies;
        if (supplies > suppliesMax)
        {
            supplies -= supplies % suppliesMax;
        }
        suppliesPercentage = (float)supplies / (float)suppliesMax;
        DisplaySupplies();
    }

    public void AddProvisions(int _provisions)
    {
        provisions += _provisions;
        if (provisions > provisionsMax)
        {
            provisions -= provisions % provisionsMax;
        }
        provisionsPercentage = (float)provisions / (float)provisionsMax;
        DisplayProvisions();
    }

    public bool SpendSupplies(int _supplies)
    {
        if (supplies - _supplies < 0)
        {
            return false;
        }

        supplies -= _supplies;
        suppliesPercentage = (float)supplies / (float)suppliesMax;
        DisplaySupplies();
        return true;
    }

    public bool SpendProvisions(int _provisions)
    {
        if (provisions - _provisions < 0)
        {
            return false;
        }

        provisions -= _provisions;
        DisplayProvisions();
        provisionsPercentage = (float)provisions / (float)provisionsMax;
        return true;
    }

}
