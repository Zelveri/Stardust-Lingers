using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralDialogueInterface : MonoBehaviour,IDialogueInterface
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCommand(string command)
    {
 
    }

    public void OnDialogueComplete()
    {
 
    }

    public void OnDialogueEnd()
    {
 
    }

    public void OnDialogueStart()
    {
 
    }

    public void OnLineEnd()
    {
        GameManager.dataController.LineEnd();
    }

    public void OnLineFinishDisplaying()
    {
 
    }

    public void OnLineStart()
    {
        GameManager.dataController.LineStart();
    }

    public void OnLineUpdate(string line)
    {
        GameManager.dataController.AddLineToTracker(line);
    }

    public void OnNodeComplete(string node)
    {
 
    }

    public void OnNodeStart(string node)
    {
 
    }

    public void OnOptionsEnd()
    {

    }

    public void OnOptionsStart()
    {
 
    }
}
