using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScalePingPong : MonoBehaviour
{
    RectTransform scale;
    public float scalar = 0.0f;
    public float maxScalar = 1.0f;
    private void Start()
    {
        scale = GetComponent<RectTransform>();
    }


    // Update is called once per frame
    void Update()
    {
        scalar = Mathf.PingPong(Time.time / 2, maxScalar) + 2.5f;

        scale.localScale = new Vector3(scalar, scalar, 1.0f);
    }
}
