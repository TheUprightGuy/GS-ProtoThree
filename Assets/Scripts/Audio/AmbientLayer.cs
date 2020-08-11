using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientLayer : MonoBehaviour
{
    public List<AudioClip> ambientSounds;

    public AudioClip RandomAudioClip()
    {
        var length = ambientSounds.Count;
        var randomIndex = Random.Range(0, length);
        return ambientSounds[randomIndex];
    }
}
