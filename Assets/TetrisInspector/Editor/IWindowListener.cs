using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.EditorCoroutines.Editor;

public interface IWindowListener
{
    void Init();

    string GetGameName();

    string GetInstruction();

    string GetVersionCode();

    string GetCreatorName();

    int GetHighScore();

    void OnInput(KeyCode input);

    EditorCoroutine StartGameCoreRunner();
}
