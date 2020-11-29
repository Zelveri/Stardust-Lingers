using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpriteCommandBehaviour : MonoBehaviour
{
    public GameObject characterCanvas;
    GameObject curChar;
    Image characterImage;
    AnimationState state;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpriteCommand(string[] parameters, System.Action onComplete)
    {
        if(parameters.Length < 2)
        {
            Debug.LogError("YarnCommands: \"sprite\" command did not receieve enough arguments");
            return;
        }
        // get gameobject of referred character and set active
        curChar = characterCanvas.transform.Find(parameters[0]).gameObject;
        // set visibility of gameobject
        if(parameters[1] == "None")
        {
            TransitionHandler.OnDark.AddListener(new UnityAction(() => curChar.SetActive(false)));
            onComplete?.Invoke();
            return;
        }
        // get components
        characterImage = curChar.GetComponent<Image>();
        state = curChar.GetComponent<AnimationState>();
        // load image and set sprite
        var path = "Artwork/Character/" + parameters[0] + "/" + parameters[1];
        Sprite character = Resources.Load<Sprite>(path);
        // do we need to schedule the change?
        if (TransitionHandler.newNode)
        {
            // only do live change if transition is not scheduled
            TransitionHandler.OnDark.AddListener(new UnityAction(
                delegate() {
                    characterImage.sprite = character;
                    curChar.SetActive(true);
                }
                )) ;
            onComplete?.Invoke();
        }
        else 
        {
            // do animation
            string trigger;
            // if command does not give animation type, do bounce
            if (parameters.Length >= 3)
            {
                trigger = parameters[2];
            }
            else
            {
                trigger = "Bounce";
            }
            characterImage.sprite = character;
            curChar.SetActive(true);
            StartCoroutine(DoAnimation(trigger, onComplete)); 
        }
    }

    IEnumerator DoAnimation(string trigger, System.Action onComplete)
    {
        state.AnimationStart();
        curChar.GetComponent<Animator>().SetTrigger(trigger);
        while (state.IsRunning)
        {
            yield return null;
        }
        onComplete?.Invoke();
    }
}
