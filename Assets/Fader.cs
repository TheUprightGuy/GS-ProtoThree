﻿using System.Collections;
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

    //GetVertex transitionScript;
    Movement movement;

    public void FadeOut(Movement _movement)
    {
        movement = _movement;
        animator.SetTrigger("FadeOut");
    }

    public void MovePlayer()
    {
        movement.MoveCharacter();
        FadeIn();
    }

    public void FadeIn()
    {
        animator.SetTrigger("FadeIn");
    }
}
