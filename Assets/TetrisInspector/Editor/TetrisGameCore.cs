using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGameCore: IWindowListener
{
    Coordinate gameSize = new Coordinate(7, 15);
    int[,] gameBoard = new int[1,1];

    public string GetCreatorName()
    {
        return "Rocky Hong";
    }

    public string GetGameName()
    {
        return "Tetris";
    }

    const string PP_HighScore = "HighScoreForArcadeTetris";
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(PP_HighScore, 0);
    }

    public string GetVersionCode()
    {
        return "0.0.1";
    }

    public int[,] GetViewArray()
    {
        return gameBoard;
    }

    public void Init()
    {
        gameBoard = new int[gameSize.x, gameSize.y];
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
        //Debug.Log("Update");
    }
}
