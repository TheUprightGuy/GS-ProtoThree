using System;
using System.Collections;
using System.Collections.Generic;
using Puzzle;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public PuzzleGenerator puzzleGenerator;
    void Update()
    {
        transform.Rotate(0f, 1f, 0f, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collectable found!");
        puzzleGenerator.FoundCollectable();
        Destroy(gameObject);
    }
}
