using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PopUp
{
    public string PopUpText = "";
    public KeyCode KeyCheck = KeyCode.None;
    public float TimeCheck = Mathf.Infinity;
    
}
public class PopUpHandler : MonoBehaviour
{
    Text textToChange;
    Animator transitions;
    List<PopUp> PopUpQueue = new List<PopUp>();

    #region Singleton
    public static PopUpHandler instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one PopUpHandler exists!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion Singleton

    bool bPlaying = false;
    float timer = 0.0f;

    private void Start()
    {
        transitions = GetComponent<Animator>();
        textToChange = GetComponentInChildren<Text>();
        EventHandler.instance.endEstablishingShot += BasePopups;
        //QueuePopUp("This is test one", KeyCode.T);
        //QueuePopUp("This is test two", 5.0f);
    }
    // Update is called once per frame
    void Update()
    {
        if (PopUpQueue.Count > 0)
        {
            //Play next prompt in Queue (make sure has gone to start)
            if (!bPlaying && transitions.GetCurrentAnimatorStateInfo(0).IsName("Start"))
            {
                textToChange.text = PopUpQueue[0].PopUpText;
                transitions.SetTrigger("FadeIn");
                timer = 0.0f;
                bPlaying = true;
            }
            else //
            {

                timer += Time.deltaTime / PopUpQueue[0].TimeCheck;

                if (Input.GetKeyDown(PopUpQueue[0].KeyCheck) || timer > 1.0f)
                {
                    PopUpQueue.RemoveAt(0);
                    //play end anim
                    transitions.SetTrigger("FadeOut");
                    bPlaying = false;
                }
                
            }
        }
    }

    public void QueuePopUp(string _text, KeyCode _key)
    {
        PopUp newPopUp = new PopUp();
        newPopUp.PopUpText = _text;
        newPopUp.KeyCheck = _key;

        PopUpQueue.Add(newPopUp);
    }

    public void QueuePopUp(string _text, float _time)
    {
        PopUp newPopUp = new PopUp();
        newPopUp.PopUpText = _text;
        newPopUp.TimeCheck = _time;

        PopUpQueue.Add(newPopUp);
    }

    bool bCalled = false;
    public void BasePopups()
    {
        if (!bCalled)
        {
            bCalled = true;
            StartCoroutine(WaitForTime());
        }
        
    }

    private IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(2.5f);
        PopUpHandler.instance.QueuePopUp("Press A and D to steer the whale", KeyCode.A);
        PopUpHandler.instance.QueuePopUp("Hold Left Click to look around", KeyCode.Mouse0);
    }
}
