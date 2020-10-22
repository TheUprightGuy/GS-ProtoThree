using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    Animator animator;
    bool sitting, waving, walking;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        sitting = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimState();   
    }

    void UpdateAnimState()
    {
        animator.SetBool("Sitting", sitting);
        animator.SetBool("Waving", waving);
        animator.SetBool("Walking", walking);
    }
}
