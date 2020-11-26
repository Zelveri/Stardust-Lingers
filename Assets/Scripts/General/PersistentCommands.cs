using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentCommands : MonoBehaviour
{
    public MusicPlayerBehaviour music;
    public SoundEffectsBehaviour sound;
    MyDialogueRunner dialogueRunner;
    private void Awake()
    {
        dialogueRunner = GameManager.dialogueRunner;
        dialogueRunner.AddCommandHandler("music", music.PlayMusic);
        dialogueRunner.AddCommandHandler("sound", sound.Sound);
        dialogueRunner.AddCommandHandler("scene", LoadScene);
    }

    void LoadScene(string[] pars)
    {
        GameManager.sceneController.LoadScene(pars[0]);
    }
}
