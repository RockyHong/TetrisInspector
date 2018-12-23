using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGameCore: IWindowListener
{
    Coordinate gameSize = new Coordinate(10, 20);
    int[,] gameBoard = new int[1,1];

    public string GetGameName()
    {
        return "Arcade Room * ";
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

    public int[,] GetViewArray()
    {
        return gameBoard;
    }

    bool isInit = false;
    public void Init()
    {
        gameBoard = new int[gameSize.x, gameSize.y];
        isInit = true;
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

    public void OnViewUpdate()
    {
        if (!isInit)
            Init();

        viewWindow.SetViewArray(gameBoard);
    }

    public string GetInstruction()
    {
        return "Arrow keys to control.";
    }

    ArcadeWindow viewWindow { get { return ArcadeWindow.instance; } }
}
