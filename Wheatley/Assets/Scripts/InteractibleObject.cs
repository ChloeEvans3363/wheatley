using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractibleObject : MonoBehaviour
{
    public Tuple<int, int> location;
    public bool canPush = true;
    public string heldWord = "Dogs";

    private void Start()
    {
        TextMeshPro text = gameObject.GetComponentInChildren<TextMeshPro>();
        if(text != null )
            text.text = heldWord;
    }

    public void SetupData(Tuple<int, int> loc, bool pushable, string word)
    {
        location = loc;
        canPush = pushable;
        heldWord = word;

        TextMeshPro text = gameObject.GetComponentInChildren<TextMeshPro>();
        if (text != null)
            text.text = heldWord;
    }
}