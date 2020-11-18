using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ISceneSettings
{
    public Canvas dialogueCanvas;
    public List<Button> optionButtons;

    public float textSpeed;
    public string startNode;
    public YarnProgram[] yarnScripts;


}
