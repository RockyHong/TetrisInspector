using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGameCore
{
    public void OnInput(TetrisInput input)
    {
        switch (input)
        {
            case TetrisInput.MoveLeft:
                break;
            case TetrisInput.MoveRight:
                break;
            case TetrisInput.Rotate:
                break;
            case TetrisInput.PushDown:
                break;
        }
    }

    public enum TetrisInput { PushDown, MoveLeft, MoveRight, Rotate }
}
