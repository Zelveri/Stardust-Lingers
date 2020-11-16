using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueInterface
{
    // Dialogue runner events
    void OnNodeStart(string node);

    void OnNodeComplete(string node);

    void OnDialogueComplete();

    // dialogue UI events
    void OnDialogueStart();

    void OnDialogueEnd();

    void OnLineStart();

    void OnLineFinishDisplaying();

    void OnLineUpdate(string line);

    void OnLineEnd();

    void OnOptionsStart();

    void OnOptionsEnd();

    void OnCommand(string command);
}
