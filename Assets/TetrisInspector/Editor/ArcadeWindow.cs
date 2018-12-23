using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class ArcadeWindow : EditorWindow
{
    static ArcadeWindow _instance;
    static ArcadeWindow instance
    {
        get
        {
            if (_instance == null)
                _instance = GetAracdeWindow();
            return _instance;
        }
    }

    static ArcadeWindow GetAracdeWindow()
    {
        return GetWindow<ArcadeWindow>();
    }
    
    [MenuItem("UselessTools/Arcade Room/Tetris")]
    static void OpenTetrisWindow()
    {
        instance.SetWindowListener(new TetrisGameCore());
    }

    IWindowListener iWindowListener;

    public void SetWindowListener(IWindowListener iWindowListener)
    {
        this.iWindowListener = iWindowListener;
    }

    private void Update()
    {
        Repaint(); 
    }

    private void OnGUI()
    {
        if (iWindowListener == null)
        {
            ScrollingTitle("- Arcade Room -");
            DrawSeletGame();
        }
        else
        {
            string gameTitle = string.Format("{0} ( {1} ) by {2}      ",
                iWindowListener.GetGameName(),
                iWindowListener.GetVersionCode(),
                iWindowListener.GetCreatorName());

            ScrollingTitle(gameTitle);
            iWindowListener.OnViewUpdate();
            KeyBoardInputDetect();
            DrawGameView();
        }
    }

    void DrawSeletGame()
    {
        if (GUILayout.Button("Tetris"))
        {
            OpenTetrisWindow();
        }
    }

    const double titleFlashFrequency = .5d;
    void DrawGameView()
    {
        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

        double t = EditorApplication.timeSinceStartup;
        if (t % titleFlashFrequency > titleFlashFrequency * .5f)
        {
            int highScore = iWindowListener.GetHighScore();
            string highScoreText = highScore == 0 ? "--" : highScore.ToString();
            EditorGUILayout.LabelField("High Score : " + highScoreText, style, GUILayout.ExpandWidth(true));
        }
        else
        {
            EditorGUILayout.LabelField(string.Empty, style, GUILayout.ExpandWidth(true));
        }
    }

    void KeyBoardInputDetect()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown)
            iWindowListener.OnInput(Event.current.keyCode);
    }

    void ScrollingTitle(string title)
    {
        double scrollSpeed = 12;
        double systemTime = EditorApplication.timeSinceStartup;
        int shiftIndex = (int)Math.Round(systemTime * scrollSpeed);
        shiftIndex %= title.Length;

        List<char> chars = new List<char>();
        chars.AddRange(title);

        for (int i = 0; i < shiftIndex; i++)
        {
            char c = chars[0];
            chars.RemoveAt(0);
            chars.Add(c);
        }

        string titleText = string.Empty;
        for (int j = 0; j < chars.Count; j++)
        {
            titleText += chars[j];
        }

        instance.titleContent = new GUIContent(titleText);
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

    public void ShakeWindow()
    {

    }
    enum ShakeWindowStrengh { Small, Medium, Hard }
}