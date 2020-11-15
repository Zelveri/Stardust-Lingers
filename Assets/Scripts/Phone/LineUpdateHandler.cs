using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System.Linq;

public class LineUpdateHandler : MonoBehaviour
{
    List<BubbleBehaviour> meBubbles;
    List<BubbleBehaviour> themBubbles;
    BubbleBehaviour curBubble;

    public DialogueRunner dialogueRunner;
    public GameObject meBubblesTemplate;
    public GameObject themBubblesTemplate;
    public GameObject phoneScreenPanel;

    string activeSide = "me";

    public string text
    {
        set { UpdateLine(value); }
    }
    //public string nametag
    //{
    //    set { 
    //        if(value.Contains("nametag")) SwitchSides(); 
    //    }
    //}

    private void Awake()
    {
        dialogueRunner.AddCommandHandler("nametag", SwitchSides);
        meBubbles = new List<BubbleBehaviour>();
        themBubbles = new List<BubbleBehaviour>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLine(string text)
    {
        curBubble.UpdateText(text);
        UpdateBubblesPosition();
    }

    void UpdateBubblesPosition()
    {
        float height = curBubble.GetLastHeightDelta();
        meBubbles?.ForEach(bubble => bubble.MoveUp(height));
        themBubbles?.ForEach(bubble => bubble.MoveUp(height));
    }

    public void DialogueStart()
    {

    }

    public void LineStart()
    {
        // create new bubble
        curBubble = CreateNewBubble();
        UpdateBubblesPosition();
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
        curBubble.AdjustBubbleSize();
        if (activeSide == "me")
        {
            meBubbles.Add(curBubble);
        }
        else
        {
            themBubbles.Add(curBubble);
        }
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
}
