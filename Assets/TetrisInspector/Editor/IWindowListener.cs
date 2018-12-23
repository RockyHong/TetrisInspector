using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWindowListener
{
    void Init();

    string GetGameName();

    string GetInstruction();

    string GetVersionCode();

    string GetCreatorName();

    int GetHighScore();

    int[,] GetViewArray();

    void OnViewUpdate();

    void OnInput(KeyCode input);
}
