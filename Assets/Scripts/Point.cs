using System;
using UnityEngine;

[Serializable]
public struct Point
{
    [SerializeField] public int X;
    [SerializeField] public int Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static bool operator ==(Point point1, Point point2)
    {
        if (point1.X == point2.X && point1.Y == point2.Y)
            return true;
        return false;
    }

    public static bool operator !=(Point point1, Point point2)
    {
        if (point1.X != point2.X || point1.Y != point2.Y)
            return true;
        return false;
    }
    
    public static Point operator - (Point point1, Point point2)
    {
       return new Point(point1.X - point2.X, point1.Y - point2.Y);
    }
    
    public static Point operator * (Point point1, Point point2)
    {
        return new Point(point1.X * point2.X, point1.Y * point2.Y);
    }
    
    public static Point operator * (Point point1, int number)
    {
        return new Point(point1.X * number, point1.Y * number);
    }
    
    public static Point operator + (Point point1, Point point2)
    {
        return new Point(point1.X + point2.X, point1.Y + point2.Y);
    }
}
