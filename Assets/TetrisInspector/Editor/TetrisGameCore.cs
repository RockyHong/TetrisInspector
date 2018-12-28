using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using System;

public class TetrisGameCore : IWindowListener
{
    #region Properties
    Coordinate gameSize = new Coordinate(10, 20);
    BlockState[,] gameBoard = new BlockState[1, 1];

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
    #endregion

    public EditorCoroutine StartGameCoreRunner()
    {
        EditorCoroutineUtility.StartCoroutine(TimeDetector(), this);
        return EditorCoroutineUtility.StartCoroutine(GameCoreRunner(), this);
    }

    void Init()
    {
        gameBoard = new BlockState[gameSize.x, gameSize.y];
        int[,] array = gameBoard.Clone() as int[,];

        viewWindow.SetViewArray(array);
    }

    bool isHasMatchedLine = false;
    PlayerShape playerShape;
    IEnumerator GameCoreRunner()
    {
        Init();
        //Infinite Game Cycle
        while (true)
        {
            //Spawn Player Shape
            playerShape = new PlayerShape();
            playerShape.shapeCoordinates = TetrisShapes.GetShapeCoordinates(TetrisShapes.Shape.Random);
            playerShape.shapeCoordinates += new Coordinate(Mathf.RoundToInt(gameSize.x / 2), gameSize.y - 3);
            while (playerShape != null)
            {
                    RefreshViewArray();
                    yield return null;
                }
            }
        }
    }

    void RefreshViewArray()
    {
        int[,] viewArray = gameBoard.Clone() as int[,];
        foreach (Coordinate c in playerShape.shapeCoordinates)
        {
            if (c.x >= 0 && c.y >= 0 && c.x < viewArray.GetLength(0) && c.y < viewArray.GetLength(1))
                viewArray[c.x, c.y] = 1;
        }

        viewWindow.SetViewArray(viewArray);
    }

    BlockState[,] CheckMatch()
    {
        BlockState[,] array = gameBoard.Clone() as BlockState[,];
        for (int y = 0; y < array.GetLength(1); y++)
        {
            bool isMatched = true;
            for (int x = 0; x < array.GetLength(0); x++)
            {
                if (array[x, y] == BlockState.Empty)
                    isMatched = false;
            }

            if (isMatched)
            {
                for (int x = 0; x < array.GetLength(0); x++)
                {
                    array[x, y] = BlockState.ReadyToClear;
                }
            }
        }

        return array;
    }

    public void OnInput(KeyCode input)
    {
        switch (Event.current.keyCode)
        {
            case KeyCode.UpArrow:
                RotatePlayer();
                break;
            case KeyCode.DownArrow:
                FallDownPlayer();
                break;
            case KeyCode.RightArrow:
                MovePlayer(1);
                break;
            case KeyCode.LeftArrow:
                MovePlayer(-1);
                break;
        }
    }

    #region PlayerBehaviour
    void MovePlayer(int offset)
    {
        Coordinate[] coordinates = playerShape.shapeCoordinates.Clone() as Coordinate[];
        coordinates += Coordinate.Right * offset;

        if (DetectIfCollideWithGameBoard(coordinates))
            playerShape.shapeCoordinates = coordinates;
        else
            viewWindow.ShakeWindow(ArcadeWindow.ShakeWindowStrengh.Small);
    }

    bool RotatePlayer()
    {
        Coordinate[] coordinates = playerShape.shapeCoordinates.Clone() as Coordinate[];
        Coordinate center = Coordinate.GetCenterInCoordinates(coordinates);
        coordinates = Coordinate.RotateCoordinatesByPivot(coordinates, center, Coordinate.RotateDirection.Right);

        bool isAvailable = DetectIfCollideWithGameBoard(coordinates);
        if (isAvailable)
            playerShape.shapeCoordinates = coordinates;
        else
            viewWindow.ShakeWindow(ArcadeWindow.ShakeWindowStrengh.Small);

        return isAvailable;
    }

    bool FallDownPlayer()
    {
        Coordinate[] coordinates = playerShape.shapeCoordinates.Clone() as Coordinate[];
        coordinates += Coordinate.Down;

        bool isAvailable = DetectIfCollideWithGameBoard(coordinates);
        if (isAvailable)
            playerShape.shapeCoordinates = coordinates;
        else
            OnLand();
        return isAvailable;
    }

    void FallPlayerToBottom()
    {
        while (FallDownPlayer()) ;
    }

    bool DetectIfCollideWithGameBoard(Coordinate[] coordinates)
    {
        foreach (Coordinate c in coordinates)
        {
            //Check is outside of gameboard
            if (c.x >= gameBoard.GetLength(0) || c.y >= gameBoard.GetLength(1) || c.x < 0 || c.y < 0)
                return false;

            //Check is overlap gameboard
            if (gameBoard[c.x, c.y] == BlockState.Block)
                return false;
        }
        return true;
    }

    void OnLand()
    {
        foreach (Coordinate c in playerShape.shapeCoordinates)
            gameBoard[c.x, c.y] = BlockState.Block;

        viewWindow.ShakeWindow(ArcadeWindow.ShakeWindowStrengh.Medium);

        playerShape = null;
    }

    class PlayerShape
    {
        public Coordinate[] shapeCoordinates;
    }
        
    #endregion

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

    enum BlockState { Empty, Block, ReadyToClear }
}
