using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WhaleData", menuName = "InfoObjects/WhaleData")]
public class WhaleInfo : ScriptableObject
{
    public bool leashed = false;
    public GameObject whale;
    public GameObject target;

    public Thought currentThought;

    public float hunger = 0.0f;
    public float hungerModifier;
    public float weight = 0.0f;

    public void UpdateHunger(float _time)
    {
        hunger -= _time;
        if (hunger < 0){hunger = 0;}

        if (hunger < 20)
        {
            // Overwrite any other thoughts
            if (currentThought != Thought.Food)
            {
                ThoughtsScript.instance.ShowThought(Thought.Food, true);
            }
            hungerModifier = 0.75f;
        }
        else if (hunger < 70)
        {
            hungerModifier = 1.0f;
        }
        else
        {
            hungerModifier = 1.25f;
        }

        // Not currently Used
        CheckWeight();
    }

    public void CheckWeight()
    {
        // Too much Weight & not Hungry
        if (weight > 70.0f && ((currentThought != Thought.Weight) && (currentThought != Thought.Food)))
        {
            ThoughtsScript.instance.ShowThought(Thought.Weight, true);
        }
        if (weight <= 70.0f && currentThought == Thought.Weight)
        {
            ThoughtsScript.instance.ShowThought(Thought.None, true);
        }
    }

    public void FeedWhale()
    {
        hunger += 10.0f;
        if (hunger > 100)
        {
            hunger = 100;
        }
    }

    public void ResetOnPlay()
    {
        leashed = false;
        whale = null;
        target = null;
        hunger = 69.0f; // LMAO
        weight = 0.0f;
        currentThought = Thought.None;
    }

    public void ToggleLeashed(bool _toggle)
    {
        leashed = _toggle;
    }
}
