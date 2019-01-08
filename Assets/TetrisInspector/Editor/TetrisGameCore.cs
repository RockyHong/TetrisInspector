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
        EditorCoroutineUtility.StartCoroutine(ViewUpdater(), this);
        return EditorCoroutineUtility.StartCoroutine(GameCoreRunner(), this);
    }

    void Init()
    {
        gameBoard = new BlockState[gameSize.x, gameSize.y];
    }

    const float FallFrequency = 1f;
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

            double t = 0;
            while (playerShape != null)
            {
                t += deltaTime;
                if (t > FallFrequency)
                {
                    t -= FallFrequency;
                    t %= FallFrequency;
                    FallDownPlayer();
                }

                yield return null;
            }

            BlockState[,] newBlockStates;
            bool isAnyMatchedLines = CheckMatch(out newBlockStates);
            gameBoard = newBlockStates;

            if (isAnyMatchedLines)
            {
                yield return new EditorWaitForSeconds(1f);
                ClearLine();
            }
        }
    }

    void ClearLine()
    {
        BlockState[,] blockStates = gameBoard.Clone() as BlockState[,];
        for (int x = 0; x < blockStates.GetLength(0); x++)
        {
            int clearedBlockCount = 0;
            for (int y = 0; y < blockStates.GetLength(1); y++)
            {
                if (blockStates[x, y] == BlockState.ReadyToClear)
                {
                    blockStates[x, y] = BlockState.Empty;
                    clearedBlockCount++;
                }

                if (blockStates[x, y] == BlockState.Block && clearedBlockCount > 0)
                {
                    blockStates[x, y] = BlockState.Empty;
                    blockStates[x, y - clearedBlockCount] = BlockState.Block;
                }
            }
        }
        gameBoard = blockStates.Clone() as BlockState[,];
        ArcadeWindow.instance.ShakeWindow(ArcadeWindow.ShakeWindowStrengh.Small);
    }

    IEnumerator ViewUpdater()
    {
        while (true)
        {
            RefreshViewArray();
            yield return null;
        }
    }

    void RefreshViewArray()
    {
        BlockState[,] mergedGameBoard = gameBoard.Clone() as BlockState[,];

        if (playerShape != null)
        {
            foreach (Coordinate c in playerShape.shapeCoordinates)
            {
                if (c.x >= 0 && c.y >= 0 && c.x < mergedGameBoard.GetLength(0) && c.y < mergedGameBoard.GetLength(1))
                    mergedGameBoard[c.x, c.y] = BlockState.Block;
            }
        }

        var viewArray = BlockStatesArray2ViewArray(mergedGameBoard);
        viewWindow.SetViewArray(viewArray);
    }

    bool CheckMatch(out BlockState[,] blockStates)
    {
        BlockState[,] array = gameBoard.Clone() as BlockState[,];

        bool isAnyMatchedLines = false;
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
                isAnyMatchedLines = true;
                for (int x = 0; x < array.GetLength(0); x++)
                {
                    array[x, y] = BlockState.ReadyToClear;
                }
            }
        }

        blockStates = array;

        return isAnyMatchedLines;
    }

    ArcadeWindow.ArrayViewBlockState[,] BlockStatesArray2ViewArray(BlockState[,] blockStates)
    {
        int lengthX = blockStates.GetLength(0);
        int lengthY = blockStates.GetLength(1);

        if (lengthX <= 0 || lengthY <= 0)
            return new ArcadeWindow.ArrayViewBlockState[0, 0];

        ArcadeWindow.ArrayViewBlockState[,] result = new ArcadeWindow.ArrayViewBlockState[lengthX, lengthY];

        for (int x = 0; x < lengthX; x++)
        {
            for (int y = 0; y < lengthY; y++)
            {
                ArcadeWindow.ArrayViewBlockState viewBlockState = ArcadeWindow.ArrayViewBlockState.Empty;
                switch (blockStates[x, y])
                {
                    case BlockState.Empty:
                        viewBlockState = ArcadeWindow.ArrayViewBlockState.Empty;
                        break;
                    case BlockState.Block:
                        viewBlockState = ArcadeWindow.ArrayViewBlockState.Block;
                        break;
                    case BlockState.ReadyToClear:
                        viewBlockState = ArcadeWindow.ArrayViewBlockState.Flash;
                        break;
                }

                result[x, y] = viewBlockState;
            }
        }
        return result;
    }

    public void OnMouseClick(int x, int y)
    {
        switch (gameBoard[x, y])
        {
            case BlockState.Empty:
                gameBoard[x, y] = BlockState.Block;
                break;
            case BlockState.Block:
                gameBoard[x, y] = BlockState.Empty;
                break;
            case BlockState.ReadyToClear:
                break;
        }
    }

    public void OnKeyboardInput(KeyCode input)
    {
        if (playerShape == null)
            return;

        switch (Event.current.keyCode)
        {
            case KeyCode.UpArrow:
                RotatePlayer();
                break;
            case KeyCode.DownArrow:
                FallPlayerToBottom();
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
        ArcadeWindow.instance.ShakeWindow(ArcadeWindow.ShakeWindowStrengh.Hard);
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
