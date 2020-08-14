using UnityEngine;
using System.Collections;

public class SkyboxBlender : MonoBehaviour
{
    public Material material;
    public float blend;
    public float count;
    public float blendSpeed = 1.0f;

    public Color color1;
    public Color color2;
    public Color color;

    public Light light;

    void Start()
    {
        material.SetFloat("_Blend", 0f);
        color1 = material.GetColor("_Tint1");
        color2 = material.GetColor("_Tint2");
    }

    void Update()
    {
        count += Time.deltaTime * blendSpeed;

        blend = Mathf.PingPong(count, 1.0f);
        material.SetFloat("_Blend", blend);
        color = Color.Lerp(color1, color2, blend);
        light.color = color;
    }
}

