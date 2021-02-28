﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InputState
{
    KEYDOWN,
    KEYHELD,
    KEYUP
}

[System.Serializable]
public class InputEvent : UnityEvent<InputState> {}

public class VirtualInputs : MonoBehaviour
{
    


    [System.Serializable]
    public struct InputListener
    {
        public string NameForInput;
        public KeyCode KeyToListen;
       /// <summary>
       /// MethodToCall must have parameter of type InputState
       /// </summary>
        public InputEvent MethodToCall;
    }

    public List<InputListener> PlayerInputs = new List<InputListener>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        foreach (InputListener IJ in PlayerInputs)
        {
            if (IJ.NameForInput == "") //Leave name blank to stop listener
            {
                continue;
            }

            if (Input.GetKeyDown(IJ.KeyToListen))
            {
                IJ.MethodToCall.Invoke(InputState.KEYDOWN);
            }
            else if (Input.GetKey(IJ.KeyToListen))
            {
                IJ.MethodToCall.Invoke(InputState.KEYHELD);
            }
            else if (Input.GetKeyUp(IJ.KeyToListen))
            {
                IJ.MethodToCall.Invoke(InputState.KEYUP);
            }

        }
    }

    public InputListener GetInputListener(string _ILName)
    {
        for (int i = 0; i < PlayerInputs.Count; i++)
        {
            if (PlayerInputs[i].NameForInput == _ILName)
            {
                return PlayerInputs[i];
            }
        }

        Debug.LogWarning(_ILName + " does not exist.");
        return new InputListener();
    }
}
