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

    public DialogueRunner dialogueRunner;
    public GameObject meBubblesTemplate;
    public GameObject themBubblesTemplate;
    public GameObject phoneScreenPanel;

    string activeSide = "me";

    //public string nametag
    //{
    //    set { 
    //        if(value.Contains("nametag")) SwitchSides(); 
    //    }
    //}

    private void Awake()
    {
        dataController = DataController.dataController;
        dialogueRunner.AddCommandHandler("nametag", SwitchSides);
        dialogueRunner.AddCommandHandler("photo", ShowImage);
        bubbles = new List<BubbleBehaviour>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBubblesPosition();
    }

    public void UpdateLine(string text)
    {
        curBubble.UpdateText(text);
        //UpdateBubblesPosition();
    }

    void UpdateBubblesPosition()
    {
        float height = curBubble.GetLastHeightDelta();
        //if (height > 0)
        //{
            bubbles?.ForEach(bubble => bubble.MoveUp(height));
        //}
    }

    public void DialogueStart()
    {

    }

    public void LineStart()
    {
        if (curBubble != null) bubbles.Add(curBubble);
        // create new bubble
        curBubble = CreateNewBubble();
        //UpdateBubblesPosition();
    }

    BubbleBehaviour CreateNewBubble()
    {
        BubbleBehaviour bb;
        if (activeSide == "me")
        {
            bb = Instantiate(meBubblesTemplate).GetComponent<BubbleBehaviour>();

        }
        else
        {
            bb = Instantiate(themBubblesTemplate).GetComponent<BubbleBehaviour>();
        }
        bb.transform.SetParent(phoneScreenPanel.transform);
        return bb;
    }

    public void LineEnd()
    {
        //bubbles.Add(curBubble);
    }

    public void SwitchSides(string[] pars)
    {
        if (pars[0] == "Mira")
        {
            activeSide = "me";
        }
        else
        {
            activeSide = "them";
        }
       // return activeSide;
    }

    public void ShowImage(string[] pars)
    {

        // create new bubble
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
