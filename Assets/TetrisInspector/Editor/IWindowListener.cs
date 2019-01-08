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

    void OnKeyboardInput(KeyCode input);

    void OnMouseClick(int x, int y);

    EditorCoroutine StartGameCoreRunner();
}
