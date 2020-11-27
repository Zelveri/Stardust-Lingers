using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState : MonoBehaviour
{
    bool _isrunning = false;

    public bool IsRunning
    {
        get { return _isrunning; }
        set { _isrunning = value; }
    }

    public void AnimationStart()
    {
        _isrunning = true;
    }

    public void AnimationComplete()
    {
        _isrunning = false;
    }
}
