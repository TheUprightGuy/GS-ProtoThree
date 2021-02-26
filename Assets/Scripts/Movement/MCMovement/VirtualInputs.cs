using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class VirtualInputs : MonoBehaviour
{
    public enum InputState
    {
        KEYDOWN,
        KEYHELD,
        KEYUP
    }
    [System.Serializable]
    public struct InputListener
    {
        public string NameForInput;
        public KeyCode KeyToListen;
        public InputState TypeToListen;
        public UnityEvent MethodToCall;
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

            switch (IJ.TypeToListen)
            {
                case InputState.KEYDOWN:
                    if (Input.GetKeyDown(IJ.KeyToListen))
                    {
                        IJ.MethodToCall.Invoke();
                    }
                    break;
                case InputState.KEYHELD:
                    if (Input.GetKey(IJ.KeyToListen))
                    {
                        IJ.MethodToCall.Invoke();
                    }
                    break;
                case InputState.KEYUP:
                    if (Input.GetKeyUp(IJ.KeyToListen))
                    {
                        IJ.MethodToCall.Invoke();
                    }
                    break;
                default:
                    Debug.LogWarning(IJ.NameForInput + " Has not been assigned an InputType.");
                    break;
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
