using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    WhaleInfo whaleInfo;
    ResourceDisplayScript rds;

    public List<GameObject> supplyObjs;
    public List<GameObject> provisionObjs;


    // Start is called before the first frame update
    void Start()
    {
        whaleInfo = CallbackHandler.instance.whaleInfo;
        rds = ResourceDisplayScript.instance;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
