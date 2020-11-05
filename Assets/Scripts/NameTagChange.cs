using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class NameTagChange : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public TMPro.TextMeshProUGUI nameTag;
    // Start is called before the first frame update
    void Awake()
    {
        dialogueRunner.AddCommandHandler("nametag", ChangeNameTag);
    }

    public void ChangeNameTag(string[] parameters)
    {
        nameTag.text = parameters[0];
    }
}
