using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisShapes
{
    public static Coordinate[] GetShapeCoordinates(Shape shape)
    {
        switch (shape)
        {
            case Shape.ZShape:
                return GetCoordinatesByIntArray(ZShape_IntArray);
            case Shape._ZShape:
                return GetCoordinatesByIntArray(_ZShape_IntArray);
            case Shape.LShape:
                return GetCoordinatesByIntArray(LShape_IntArray);
            case Shape._LShape:
                return GetCoordinatesByIntArray(_LShape_IntArray);
            case Shape.TShape:
                return GetCoordinatesByIntArray(TShape_IntArray);
            case Shape.IShape:
                return GetCoordinatesByIntArray(IShape_IntArray);
            case Shape.Square:
                return GetCoordinatesByIntArray(Square_IntArray);
            case Shape.Random:
                return GetShapeCoordinates((Shape)Random.Range(0, (int)Shape.Random));
        }

        throw new System.Exception("Can't find matched shape in enum");
    }

    public static Coordinate[] GetCoordinatesByIntArray(int[,] array)
    {
        List<Coordinate> coordinates = new List<Coordinate>();

        Coordinate center =
            new Coordinate(
                Mathf.RoundToInt(array.GetLength(0) / 2),
                Mathf.RoundToInt(array.GetLength(1) / 2));

        for (int y = 0; y < array.GetLength(1); y++)
            for (int x = 0; x < array.GetLength(0); x++)
                if (array[x,y] == 1)
                    coordinates.Add(new Coordinate(x, y) - center);

        return coordinates.ToArray();
    }

    #region Shapes' IntArray
    static int[,] ZShape_IntArray =
            new int[,] {
            { 1, 1, 0 },
            { 0, 1, 1 }};

    static int[,] _ZShape_IntArray =
        new int[,] {
            { 0, 1, 1 },
            { 1, 1, 0 }};

    static int[,] LShape_IntArray =
        new int[,] {
            { 1, 0 },
            { 1, 0 },
            { 1, 1 }};

    static int[,] _LShape_IntArray =
        new int[,] {
            { 0, 1 },
            { 0, 1 },
            { 1, 1 }};

    static int[,] TShape_IntArray =
    new int[,] {
            { 1, 1, 1 },
            { 0, 1, 0 }};

    static int[,] IShape_IntArray =
        new int[,] {
            { 1 },
            { 1 },
            { 1 },
            { 1 }};

    static int[,] Square_IntArray =
        new int[,] {
            { 1, 1 },
            { 1, 1 }};
#endregion

    public enum Shape { ZShape, _ZShape, LShape, _LShape, TShape, IShape, Square, Random }
}