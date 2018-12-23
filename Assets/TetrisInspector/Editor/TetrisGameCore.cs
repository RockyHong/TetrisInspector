using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

public class TetrisGameCore: IWindowListener
{
    Coordinate gameSize = new Coordinate(10, 20);
    int[,] gameBoard = new int[1,1];

    public string GetGameName()
    {
        return " TETRIS  TETRIS ";
    }

    public string GetVersionCode()
    {
        return "0.0.1";
    }

    public string GetCreatorName()
    {
        return "Rocky Hong";
    }

    const string PP_HighScore = "HighScoreForArcadeTetris";
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(PP_HighScore, 0);
    }

    public string GetInstruction()
    {
        return "Arrow keys to control.";
    }

    public EditorCoroutine StartGameCoreRunner()
    {
        EditorCoroutineUtility.StartCoroutine(TimeDetector(), this);
        return EditorCoroutineUtility.StartCoroutine(GameCoreRunner(), this);
    }

    public void Init()
    {
        gameBoard = new int[gameSize.x, gameSize.y];
    }

    IEnumerator GameCoreRunner()
    {
        //Infinite Game Cycle
        while (true)
        {
            Init();
            viewWindow.SetViewArray(gameBoard);
            //Spawn Player Shape
            Coordinate[] currentShape = TetrisShapes.GetShapeCoordinates(TetrisShapes.Shape.Random);
            Coordinate shapePos = Coordinate.Zero;
            yield return null;
        }
    }

    public void OnInput(KeyCode input)
    {
        switch (Event.current.keyCode)
        {
            case KeyCode.UpArrow:
                //Rotate
                break;
            case KeyCode.DownArrow:
                //PushDown
                break;
            case KeyCode.RightArrow:
                //MoveRight
                break;
            case KeyCode.LeftArrow:
                //MoveLeft
                break;
        }
    }

    #region DeltaTimeDetector
    double lastFrameTime;
    double deltaTime;
    IEnumerator TimeDetector()
    {
        while (true)
        {
            double currentTime = EditorApplication.timeSinceStartup;
            deltaTime = currentTime - lastFrameTime;
            lastFrameTime = currentTime;
            yield return null;
        }
    }
    #endregion

    ArcadeWindow viewWindow { get { return ArcadeWindow.instance; } }
}
