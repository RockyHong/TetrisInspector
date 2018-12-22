using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TetrisInspector : EditorWindow {

    [MenuItem("UselessTools/TetrisInspector")]
    static void OpenWindow()
    {
        TetrisInspector tetrisInspector = GetWindow<TetrisInspector>();
    }

    private void OnGUI()
    {
        
    }
}
