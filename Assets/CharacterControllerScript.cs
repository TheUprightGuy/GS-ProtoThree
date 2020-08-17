using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    public Animator animator;
    public bool moving, standing, sitting, steering;
    public bool stand, sit, sitTransition, feed;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateAnimState();
    }

    public void UpdateAnimState()
    {
        animator.SetBool("Moving", moving);
        animator.SetBool("Standing", standing);
        animator.SetBool("Sitting", sitting);
        animator.SetBool("Steering", steering);

        if (stand)
        {
            animator.SetTrigger("StandUp");
            stand = false;
            standing = true;
            sitting = false;
        }
        if (sit)
        {
            animator.SetTrigger("SitDown");
            sit = false;
            sitting = true;
            standing = false;
        }
        if (sitTransition)
        {
            animator.SetTrigger("SitTransition");
            sitTransition = false;
        }
        if (feed)
        {
            animator.SetTrigger("FeedWhale");
            feed = false;
        }
    }
}
