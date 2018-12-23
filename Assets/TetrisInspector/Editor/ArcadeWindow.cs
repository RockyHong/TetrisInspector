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
        CaculateDeltaTime();
        ShakeWindowStep();
        Repaint();
    }

    private void OnGUI()
    {
        #region ShakeTest
        for (int i = 0; i <= (int)ShakeWindowStrengh.Hard; i++)
        {
            ShakeWindowStrengh strengh = (ShakeWindowStrengh)i;
            if (GUILayout.Button(strengh.ToString()))
                ShakeWindow((ShakeWindowStrengh)i);
        }
        #endregion

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

    void CaculateDeltaTime()
    {
        double currentTime = EditorApplication.timeSinceStartup;

        if (currentTime == lastFrameTime)
        {
            deltaTime = 0;
        }
        else
        {
            deltaTime = currentTime - lastFrameTime;
            lastFrameTime = currentTime;
        }
    }

    #region ScrollingTitle
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
    #endregion

    #region Shake Screen
    float shake_duration = 1;
    float shake_progress = 1;
    float shake_magnetude;
    Vector2 shake_oPos;
    public void ShakeWindow(ShakeWindowStrengh strengh)
    {
        shake_oPos = GetWindowPosition();
        shake_progress = 0;
        switch (strengh)
        {
            case ShakeWindowStrengh.Small:
                shake_duration = .2f;
                shake_magnetude = 1f;
                break;
            case ShakeWindowStrengh.Medium:
                shake_duration = .25f;
                shake_magnetude = 2f;
                break;
            case ShakeWindowStrengh.Hard:
                shake_duration = .3f;
                shake_magnetude = 5f;
                break;
        }
    }

    void ShakeWindowStep()
    {
        if (shake_progress < 1 && shake_progress >= 0)
        {
            shake_progress += (float)deltaTime / shake_duration;
            shake_progress = Mathf.Clamp01(this.shake_progress);

            Vector2 currentOffset = new Vector2(
                UnityEngine.Random.Range(-1f, 1f) * shake_magnetude * (1 - shake_progress),
                UnityEngine.Random.Range(-1f, 1f) * shake_magnetude * (1 - shake_progress));

            SetWindowPosition(shake_oPos + currentOffset);
        }
        else if (shake_progress != -1)
        {
            ClearWindowPosition();
            shake_progress = -1;
        }
    }

    void ClearWindowPosition()
    {
        SetWindowPosition(shake_oPos);
    }

    Vector2 GetWindowPosition()
    {
        Vector2 pos = position.position;
        pos.y += 5; //Don't know why but need to fix this parameter
        return pos;
    }

    void SetWindowPosition(Vector2 pos)
    {
        pos.x = Mathf.Clamp(pos.x, 50, 5000);
        pos.y = Mathf.Clamp(pos.y, 50, 5000);
        position = new Rect(pos.x, pos.y, position.width, position.height);
    }

    public enum ShakeWindowStrengh { Small, Medium, Hard }

    double lastFrameTime;
    double deltaTime;
    #endregion

}
