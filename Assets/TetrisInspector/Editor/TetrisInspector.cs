using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TetrisInspector : EditorWindow
{

    [MenuItem("UselessTools/TetrisInspector")]
    static void OpenWindow()
    {
        var window = GetWindow<TetrisInspector>();
    }

    TetrisGameCore _tetrisGameCore;
    TetrisGameCore tetrisGameCore
    {
        get
        {
            if (_tetrisGameCore == null)
                _tetrisGameCore = new TetrisGameCore();
            return _tetrisGameCore;
        }
    }

    double lastGameViewTime;
    private void Update()
    {
        Repaint();
    }

    float passiveUpdateTime;
    private void OnGUI()
    {
        KeyBoardInputDetect();
    }

    void KeyBoardInputDetect()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown)
        {
            switch (Event.current.keyCode)
            {
                case KeyCode.UpArrow:
                    tetrisGameCore.OnInput(TetrisGameCore.TetrisInput.Rotate);
                    break;
                case KeyCode.DownArrow:
                    tetrisGameCore.OnInput(TetrisGameCore.TetrisInput.PushDown);
                    break;
                case KeyCode.RightArrow:
                    tetrisGameCore.OnInput(TetrisGameCore.TetrisInput.MoveRight);
                    break;
                case KeyCode.LeftArrow:
                    tetrisGameCore.OnInput(TetrisGameCore.TetrisInput.MoveLeft);
                    break;
            }
        }
    }

    double lastFrameTime;
    double _deltaTime;
    double deltaTime
    {
        get
        {
            double currentTime = EditorApplication.timeSinceStartup;

            if (currentTime == lastFrameTime)
            {
                return _deltaTime;
            }
            else
            {
                _deltaTime = currentTime - lastFrameTime;
                lastFrameTime = currentTime;
            }
            return deltaTime;
        }
    }
}