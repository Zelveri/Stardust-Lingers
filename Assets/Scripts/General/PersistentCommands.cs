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

        // command music
        // usage:
        // music stop
        // music play <filename>
        // music fade_out [fade_duration=3]
        // music fade_in <filename> [fade_duration=3]
        dialogueRunner.AddCommandHandler("music", music.PlayMusic);
        // sound (play/stop) <name> [loop] [fade]
        dialogueRunner.AddCommandHandler("sound", sound.Sound);
        dialogueRunner.AddCommandHandler("scene", LoadScene);
    }

    void LoadScene(string[] pars)
    {
        GameManager.sceneController.LoadScene(pars[0]);
    }
}
