using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    #region Singleton
    public static Fader instance;
    Animator animator;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Fader exists!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            animator = GetComponent<Animator>();
        }
    }
    #endregion Singleton

    GetVertex transitionScript;

    public void FadeOut(GetVertex _transitionScript)
    {
        transitionScript = _transitionScript;
        animator.SetTrigger("FadeOut");
    }

    public void MovePlayer()
    {
        transitionScript.MoveToClosestPoint();
        FadeIn();
    }

    public void FadeIn()
    {
        animator.SetTrigger("FadeIn");
    }
}
