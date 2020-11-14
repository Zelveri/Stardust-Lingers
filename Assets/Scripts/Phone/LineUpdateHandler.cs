using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LineUpdateHandler : MonoBehaviour
{
    public List<BubbleBehaviour> meBubbles;
    public BubbleBehaviour[] themBubbles;

    public string text
    {
        set { UpdateLine(value); }
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
        meBubbles.LastOrDefault().UpdateText(text);
    }

    public void DialogueStart()
    {

    }

    public void LineStart()
    {
        // create new bubble
    }

    public void LineEnd()
    {
        
    }

    public void SwitchSides(string side)
    {
        // who is typing right now?
    }
}
