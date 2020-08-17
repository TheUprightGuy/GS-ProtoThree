using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    WhaleInfo whaleInfo;
    ResourceDisplayScript rds;
    IslandTrigger trig;
    //public List<GameObject> supplyObjs;
    //public List<GameObject> provisionObjs;
    public GameObject SuppliesParent;
    public GameObject ProvisionsParent;

    private float CollectedTime = 0.0f;
    //[Tooltip("In Seconds")]
    public float OverallCollectTime;
    public int SupplyCount;
    public int ProvisionCount;

    public float ProvisionsMoveY = -5.0f;
    private Vector3 provsStart;
    private int provsMaxCount;

    public float SuppliesMoveY = -5.0f;
    private Vector3 suppliesStart;
    private int suppliesMaxCount;
    // Start is called before the first frame update
    void Start()
    {
        whaleInfo = CallbackHandler.instance.whaleInfo;
        rds = ResourceDisplayScript.instance;
        trig = GetComponent<IslandTrigger>();

        suppliesStart =  SuppliesParent.transform.localPosition;
        suppliesMaxCount = SupplyCount;

        provsStart = ProvisionsParent.transform.localPosition;
        provsMaxCount = ProvisionCount;

        Vector3 newPos = transform.position;
        newPos.y = WhaleMovementScript.instance.transform.position.y;
        transform.position = newPos;
    }

    public float time = 0.0f;
    // Update is called once per frame
    void Update()
    {
        

        if (trig.playerInRange && whaleInfo.leashed && time <= 1.0f)
        {
            time += Time.deltaTime / OverallCollectTime;

            if (SupplyCount > 0)
            {
                int temp = SupplyCount;
                SupplyCount = suppliesMaxCount - Mathf.RoundToInt(suppliesMaxCount * time);

                if (temp == SupplyCount + 1)
                {
                    rds.AddSupplies(1);
                }

                if (SuppliesParent != null)
                {
                    Vector3 target = suppliesStart;
                    target.y += SuppliesMoveY;

                    SuppliesParent.transform.localPosition = Vector3.Lerp(suppliesStart, target, time);
                }
            }

            if (ProvisionCount > 0)
            {
                int temp = ProvisionCount;
                ProvisionCount = provsMaxCount - Mathf.RoundToInt(provsMaxCount * time);

                if (temp == ProvisionCount + 1)
                {
                    rds.AddProvisions(1);
                }

                if (ProvisionsParent != null)
                {
                    Vector3 target = provsStart;
                    target.y += ProvisionsMoveY;

                    ProvisionsParent.transform.localPosition = Vector3.Lerp(provsStart, target, time);
                }
            }
        }
    }


}
