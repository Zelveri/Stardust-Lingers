using System.Collections;
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

    string activeSide = "me";

    private void Awake()
    {
        dataController = GameManager.dataController;
        dialogueRunner = GameManager.dialogueRunner;
        dialogueRunner.RemoveCommandHandler("nametag");
        dialogueRunner.AddCommandHandler("nametag", SwitchSides);
        dialogueRunner.AddCommandHandler("photo", ShowImage);
        bubbles = new List<BubbleBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBubblesPosition();
    }

    public void UpdateLine(string text)
    {
        curBubble.UpdateText(text);
    }

    void UpdateBubblesPosition()
    {
        // get height change of active bubble since last update
        float height = curBubble.GetLastHeightDelta();
        // move all other bubbles up by that amount
        bubbles?.ForEach(bubble => bubble.MoveUp(height));
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
            bb = Instantiate(meBubblesTemplate).GetComponent<BubbleBehaviour>();

        }
        else
        {
            bb = Instantiate(themBubblesTemplate).GetComponent<BubbleBehaviour>();
        }
        // seta phone screen as partent for masking
        bb.transform.SetParent(phoneScreenPanel.transform);
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
