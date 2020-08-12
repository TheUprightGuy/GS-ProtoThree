using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState",menuName = "GameSpecific/GameState")]
public class GameState : ScriptableObject
{
    public bool inMenu;
    public bool gamePaused;
    public bool inCinematic = false;
}
