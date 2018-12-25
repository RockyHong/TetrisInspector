using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.EditorCoroutines.Editor;

public interface IWindowListener
{
    string GetGameName();

    string GetInstruction();

    string GetVersionCode();

    string GetCreatorName();

    int GetHighScore();

    void OnInput(KeyCode input);

    EditorCoroutine StartGameCoreRunner();
}
