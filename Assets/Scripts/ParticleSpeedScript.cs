using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpeedScript : MonoBehaviour
{
    public ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = WhaleMovementScript.instance.currentSpeed / 200;
        //Debug.Log(alpha);
        ps.startColor = new Color(ps.startColor.r, ps.startColor.g, ps.startColor.b, alpha);
        ps.startLifetime = alpha * 200;

    }
}
