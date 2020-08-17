using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ResourceFeedBack : MonoBehaviour
{
    public GameObject PopupPrefab;
    public Sprite Supplies;
    public Sprite Provs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
        //if (Input.GetKeyDown("p"))
        //{
        //    PopupText("+1", test);
        //}
    }

    public void SupplyPopUp(string _text)
    {
        PopupText(_text, Supplies);
    }

    public void ProvPopUp(string _text)
    {
        PopupText(_text, Provs);
    }

    void PopupText(string _text, Sprite _symbol)
    {
        GameObject temp = Instantiate(PopupPrefab, transform);

        temp.GetComponent<RectTransform>().localPosition = Vector3.zero;

        TextMeshPro tmp = temp.GetComponent<TextMeshPro>();
        tmp.text = _text;

        SpriteRenderer sr = temp.GetComponentInChildren<SpriteRenderer>();
        sr.sprite = _symbol;

        Animator anims = temp.GetComponent<Animator>();

        Destroy(temp, anims.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }
}
