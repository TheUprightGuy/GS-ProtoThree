using UnityEngine;
using System.Collections;

public class SkyboxBlender : MonoBehaviour
{
    public Material material;
    public float blend;
    public float count;
    public float blendSpeed = 1.0f;

    void Start()
    {
        material.SetFloat("_Blend", 0f);
    }

    void Update()
    {
        count += Time.deltaTime * blendSpeed;

        blend = Mathf.PingPong(count, 1.0f);
        material.SetFloat("_Blend", blend);
    }
}

