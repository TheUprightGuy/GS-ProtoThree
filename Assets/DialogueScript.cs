using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScript : MonoBehaviour
{
    #region Setup
    TMPro.TextMeshProUGUI speaker;
    TMPro.TextMeshProUGUI dialogue;
    private void Awake()
    {
        speaker = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        dialogue = transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
    }
    private void Start()
    {
        CallbackHandler.instance.setDialogue += SetDialogue;
    }

    private void OnDestroy()
    {
        CallbackHandler.instance.setDialogue -= SetDialogue;
    }
    #endregion Setup
    string text;
    string[] dialogueList;

    public void SetDialogue(string _speaker, string _text)
    {
        text = _text;
        speaker.SetText(_speaker);
        dialogue.SetText("");

        StartCoroutine(WriteDialogue());
    }

    IEnumerator WriteDialogue()
    {
        dialogue.SetText("");

        foreach (char n in text)
        {
            dialogue.text += n;
            yield return new WaitForSeconds(0.03f);
        }
    }

    public void NextLine()
    {
        // move to next line in dialogueList
        // if not close 
        CallbackHandler.instance.ToggleText(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SetDialogue("Wayd","Go there homie");
        }
    }

    private void OnMouseDown()
    {
        NextLine();
    }
}
