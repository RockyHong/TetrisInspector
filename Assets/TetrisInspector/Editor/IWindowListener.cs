using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWindowListener
{
    string GetGameName();

    string GetVersion();

    string GetCreatorName();

    int[,] GetViewArray();

    void OnViewUpdate();

    void OnInput(KeyCode input);
}
