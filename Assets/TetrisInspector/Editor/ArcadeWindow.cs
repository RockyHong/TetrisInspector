using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ArcadeWindow : EditorWindow
{
    static ArcadeWindow window;

    static ArcadeWindow GetAracdeWindow()
    {
        return GetWindow<ArcadeWindow>();
    }
    
    [MenuItem("UselessTools/Arcade Room/Tetris")]
    static void OpenTetrisWindow()
    {
        window = GetAracdeWindow();
        window.SetWindowListener(new TetrisGameCore());
    }

    IWindowListener iWindowListener;

    public void SetWindowListener(IWindowListener iWindowListener)
    {
        this.iWindowListener = iWindowListener;
    }

    double lastGameViewTime;
    private void Update()
    {
        Repaint(); 
    }

    float passiveUpdateTime;
    private void OnGUI()
    {
        if (iWindowListener == null)
        {
            DrawSeletGame();
            return;
        }

        iWindowListener.OnViewUpdate();
        KeyBoardInputDetect();
        DrawGameView();
    }

    void DrawSeletGame()
    {
        if (GUILayout.Button("Tetris"))
        {
            OpenTetrisWindow();
        }
    }

    void DrawGameView()
    {
        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

        EditorGUILayout.LabelField(iWindowListener.GetGameName(), style, GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField(iWindowListener.GetVersion(), style, GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField(iWindowListener.GetCreatorName(), style, GUILayout.ExpandWidth(true));
    }

    void KeyBoardInputDetect()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown)
            iWindowListener.OnInput(Event.current.keyCode);
    }

    void ScrollingTitle()
    {
        string gameName = iWindowListener.GetGameName();
        window.titleContent = new GUIContent(gameName, "s");
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