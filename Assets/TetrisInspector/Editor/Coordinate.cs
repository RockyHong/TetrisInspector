using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Coordinate
{
    public int x;
    public int y;

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Coordinate operator +(Coordinate a, Coordinate b)
    {
        return new Coordinate(a.x + b.x, a.y + b.y);
    }

    public static Coordinate[] operator +(Coordinate[] a, Coordinate b)
    {
        for (int i = 0; i < a.Length; i++)
            a[i] += b;

        return a;
    }

    public static Coordinate operator -(Coordinate a, Coordinate b)
    {
        return new Coordinate(a.x - b.x, a.y - b.y);
    }

    public static Coordinate[] operator -(Coordinate[] a, Coordinate b)
    {
        for (int i = 0; i < a.Length; i++)
            a[i] -= b;

        return a;
    }

    public static Coordinate operator *(Coordinate a, int b)
    {
        return new Coordinate(a.x * b, a.y * b);
    }

    public static bool operator ==(Coordinate a, Coordinate b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(Coordinate a, Coordinate b)
    {
        return a.x != b.x || a.y != b.y;
    }

    public static Coordinate Zero { get { return new Coordinate(0, 0); } }
    public static Coordinate Right { get { return new Coordinate(1, 0); } }
    public static Coordinate Left { get { return new Coordinate(-1, 0); } }
    public static Coordinate Up { get { return new Coordinate(0, 1); } }
    public static Coordinate Down { get { return new Coordinate(0, -1); } }

    public static Coordinate GetCenterInCoordinates(Coordinate[] coordinates)
    {
        Coordinate center = Zero;
        foreach (Coordinate c in coordinates)
        {
            center += c;
        }
        center.x = Mathf.RoundToInt(center.x / coordinates.Length);
        center.y = Mathf.RoundToInt(center.y / coordinates.Length);
        return center;
    }

    public static Coordinate RotateCoordinateByPivot(Coordinate coordinate, Coordinate pivot, RotateDirection rotateDiraction)
    {
        Coordinate relativeCoordinate = coordinate - pivot;

        float degree = 0;
        switch (rotateDiraction) {
            case RotateDirection.None:
                degree = 0;
                break;
            case RotateDirection.Oppsite:
                degree = 180;
                break;
            case RotateDirection.Right:
                degree = -90;
                break;
            case RotateDirection.Left:
                degree = 90;
                break;
        }

        int cos = Mathf.RoundToInt(Mathf.Cos(degree * Mathf.Deg2Rad));
        int sin = Mathf.RoundToInt(Mathf.Sin(degree * Mathf.Deg2Rad));

        int x = cos * relativeCoordinate.x - sin * relativeCoordinate.y;
        int y = sin * relativeCoordinate.x + cos * relativeCoordinate.y;

        Coordinate rotatedCoordinate = new Coordinate(x, y);
        Coordinate result = pivot + rotatedCoordinate;
        return result;
    }

    public static Coordinate[] RotateCoordinatesByPivot(Coordinate[] coordinates, Coordinate pivot, RotateDirection rotateDiraction)
    {
        for (int i = 0; i < coordinates.Length; i++)
            coordinates[i] = RotateCoordinateByPivot(coordinates[i], pivot, rotateDiraction);

        return coordinates;
    }

    public static Coordinate FromVector2(Vector2 vector)
    {
        return new Coordinate(
            Mathf.RoundToInt(vector.x),
            Mathf.RoundToInt(vector.y));
    }

    public enum RotateDirection { None, Right, Left, Oppsite }
}
