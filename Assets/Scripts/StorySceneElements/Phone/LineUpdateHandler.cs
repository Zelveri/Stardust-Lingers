﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System.Linq;
using UnityEngine.UI;
using System;

public class LineUpdateHandler : MonoBehaviour
{
    List<BubbleBehaviour> bubbles;
    BubbleBehaviour curBubble;
    DataController dataController;

    DialogueRunner dialogueRunner;
    public GameObject meBubblesTemplate;
    public GameObject themBubblesTemplate;
    public GameObject phoneScreenPanel;
    public TransitionHandler transitionHandler;

    string activeSide = "me";

    private void Awake()
    {
        dataController = GameManager.dataController;
        dialogueRunner = GameManager.dialogueRunner;
        dialogueRunner.RemoveCommandHandler("nametag");
        dialogueRunner.AddCommandHandler("nametag", SwitchSides);
        dialogueRunner.AddCommandHandler("photo", ShowImage);
        dialogueRunner.AddCommandHandler("transition", Transition);
        bubbles = new List<BubbleBehaviour>();
    }

    private void OnDestroy()
    {
        dialogueRunner.RemoveCommandHandler("nametag");
        dialogueRunner.RemoveCommandHandler("photo");
        dialogueRunner.RemoveCommandHandler("transition");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBubblesPosition();
    }

    public void UpdateLine(string text)
    {
        // interface dialogue and bubble, update text
        curBubble.UpdateText(text);
    }

    // update the position of all bubbles
    void UpdateBubblesPosition()
    {
        if (curBubble != null)
        {
            // get height change of active bubble since last update
            float height = curBubble.GetLastHeightDelta();
            // move all other bubbles up by that amount
            bubbles?.ForEach((bubble) => bubble?.MoveUp(height));
        }
    }

    public void DialogueStart()
    {

    }

    public void LineStart()
    {
        // store previous bubble
        if (curBubble != null) bubbles.Add(curBubble);
        // create new bubble
        curBubble = CreateNewBubble();
    }

    BubbleBehaviour CreateNewBubble()
    {
        // create new text bubble
        BubbleBehaviour bb;
        // select if bubble is for phone user (me, Mira) or chat partner (them, Trevis)
        if (activeSide == "me")
        {
            // instatiate prefab for me bubble and assign phone screen as parent
            bb = Instantiate(meBubblesTemplate, phoneScreenPanel.transform).GetComponent<BubbleBehaviour>();
        }
        else
        {
            bb = Instantiate(themBubblesTemplate, phoneScreenPanel.transform).GetComponent<BubbleBehaviour>();
        }
        return bb;
    }

    public void LineEnd()
    {
        //bubbles.Add(curBubble);
    }

    public void SwitchSides(string[] pars)
    {
        // change the typing side
        if (pars[0] == "Mira")
        {
            activeSide = "me";
        }
        else
        {
            activeSide = "them";
        }
    }

    public void ShowImage(string[] pars)
    {
        // show image instead of text
        LineStart();
        curBubble.ShowImage(pars[0]);
    }

    void Transition(string[] pars, System.Action onComplete)
    {
        transitionHandler.Transition(pars, onComplete);
    }

    // for saving bubbles, not fully impemented

    public List<BubbleData> GetSerializableBubbles()
    {
        List<BubbleData> saveBubbles = new List<BubbleData>();
        foreach(var bubble in bubbles)
        {
            saveBubbles.Add(new BubbleData(bubble));
        }
        saveBubbles.Add(new BubbleData(curBubble));

        return saveBubbles;
    }

    public void LoadBubbles(List<BubbleData> data)
    {
        // clears all existing bubbles and loads the one from file
        bubbles.ForEach(b => Destroy(b.gameObject));
        foreach(var b in data)
        {
            BubbleBehaviour bb;
            if (b.isMeBubble)
            {
                bb = Instantiate(meBubblesTemplate).GetComponent<BubbleBehaviour>();
            }
            else
            {
                bb = Instantiate(themBubblesTemplate).GetComponent<BubbleBehaviour>();
            }
            bb.SetYPos(b.posy);
            if (b.contentIsImagePath)
            {
                bb.ShowImage(b.content);
            }
            else
            {
                bb.UpdateText(b.content);
            }
            bubbles.Add(bb);
        }
    }
}
