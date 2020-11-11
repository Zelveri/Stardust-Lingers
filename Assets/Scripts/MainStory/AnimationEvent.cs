using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    // Start is called before the first frame update
    bool _isfading = false;
    CanvasGroup canvasGroup;
    Animator animator;
    bool _ishidden = false;
    public bool isHidden
    {
        get { return _ishidden; }
        set { _ishidden = value; }
    }

    public bool isRunning
    {
        get { return _isfading; }
    }

    private void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        animator = gameObject.GetComponent<Animator>();
    }

    public void FadeClear()
    {
        var currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (!currentState.IsName("Dialogue_Fade_Clear"))
        {
            _isfading = true;
            _ishidden = true;
            animator.SetTrigger("Fade_Clear");
//       while (isFading) { };
        }
    }

    public void FadeOpaque()
    {
        var currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (!currentState.IsName("Dialogue_Fade_Opaque"))
        {
            _isfading = true;
            _ishidden = false;
            animator.SetTrigger("Fade_Opaque"); 
           // while (isFading) { };
        }
    }

    public void AnimationStart()
    {
        _isfading = true;
    }

    public void AnimationEnd()
    {
        _isfading = false;
    }


}
