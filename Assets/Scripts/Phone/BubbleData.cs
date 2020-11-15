using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BubbleData
{
    public float posy;
    public string content;
    public bool contentIsImagePath = false;
    public bool isMeBubble = true;

    public BubbleData(BubbleBehaviour bubble)
    {
        posy = bubble.GetYPos();
        content = bubble.GetContent();
        contentIsImagePath = bubble.ContentIsImagePath();
        isMeBubble = bubble.IsMeBubble();
    }
}
