using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class DataController : MonoBehaviour
{
    static List<string> lines = new List<string>();
    static string currentNode;
    static string prevNode;
    static string curNametag;
    static string backdrop;
    static string curMusic;
    static string [] curSounds;
    // static bool lineIncomplete = true;

    public string CurNametag => curNametag;

    // Start is called before the first frame update
    public void Awake()
    {
    }

    public void LineStart()
    {
        //lineIncomplete = true;
    }

    public void LineEnd()
    {
       // lineIncomplete = false;
    }

    public void AddLineToTracker(TMPro.TextMeshProUGUI dialogueTextContainer)
    {
        lines.Add(dialogueTextContainer.text);
    }

    public void AddLineToTracker(string text)
    {
        lines.Add(text);
    }

    public void LoadState(SaveState save)
    {
        StartCoroutine(DoStateLoad(save));
    }

    IEnumerator DoStateLoad(SaveState save)
    {
        // prevent dialogue from starting until save load complete
        GameManager.sceneController.startDialogueOnLoad = false;
        yield return StartCoroutine(GameManager.sceneController.LoadScene(save.curScene));
        lines = new List<string>(save.lines);
        curNametag = save.curNameTag;
        currentNode = save.currentNode;
        prevNode = save.prevNode;
        foreach (var entry in save.variables)
        {
            GameManager.variableStorage.SetValue(entry.Key, entry.Value);
        }
        TransitionHandler.overrideTransitionFade = true;
        // if (lineIncomplete) GameManager.dialogueUI.MarkLineComplete();
        //StartCoroutine(backgroundChange.DoChangeFast(save.backdrop, null));
        yield return new WaitUntil(() => (bool)GameManager.dialogueUI);
        GameManager.musicPlayer.Play(save.curMusic);
        GameManager.dialogueRunner.StartDialogue(save.currentNode);
    }

    public void UpdateNametag(string text, string visible_as="")
    {
        if (visible_as == "") visible_as = text;
        curNametag = text;
        AddLineToTracker("\n" + visible_as + ":");
    }

    public void UpdateCurrentNode(string node)
    {
        currentNode = node;
        AddLineToTracker("-------------------\nScene " + currentNode +":");
    }

    public void UpdateFinishedNode(string node)
    {
        prevNode = node;
        AddLineToTracker("-------------------");
    }

    public void UpdateBackdrop(string b)
    {
        backdrop = b;

    }

    public void UpdateMusic(string music)
    {
        curMusic = music;
    }

    public void UpdateSounds(string[] sounds)
    {
        curSounds = sounds;
    }

    public static string[] Lines => lines.ToArray();


    public static string CurrentNode => currentNode;

    public static string PrevousNode => prevNode;


    public static string Nametag => curNametag;

    public static string Backdrop => backdrop;


    public static Dictionary<string, Yarn.Value> Variables
    {
        get
        {
            var dict = new Dictionary<string, Yarn.Value>();
            foreach (var entry in GameManager.variableStorage)
            {
                dict.Add(entry.Key, entry.Value);
            }
            return dict;
        }
    }

    public static string Music => curMusic;

    public static string[] Sounds => curSounds;
}
