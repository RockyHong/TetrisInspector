using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWindowListener
{
    string GetGameName();

    string GetVersionCode();

    string GetCreatorName();

    int GetHighScore();

    int[,] GetViewArray();

    void OnViewUpdate();

    void OnInput(KeyCode input);
}
