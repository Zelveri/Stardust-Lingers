using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    // Start is called before the first frame update
    bool _isfading = false;
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
        animator = gameObject.GetComponent<Animator>();
    }

    public IEnumerator FadeClear(System.Action onComplete)
    {
        var currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (!currentState.IsName("Dialogue_Fade_Clear"))
        {
            _isfading = true;
            _ishidden = true;
            animator.SetTrigger("Fade_Clear");
            while (_isfading) 
            {
                yield return null;
            }
            onComplete?.Invoke();
        }
    }

    public IEnumerator FadeOpaque(System.Action onComplete)
    {
        var currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (!currentState.IsName("Dialogue_Fade_Opaque"))
        {
            _isfading = true;
            _ishidden = false;
            animator.SetTrigger("Fade_Opaque");
            while (_isfading)
            {
                yield return null;
            }
            onComplete?.Invoke();
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
