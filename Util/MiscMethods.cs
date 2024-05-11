using System;

namespace Battleship.Util;

using Vector2 = Microsoft.Xna.Framework.Vector2;

public static class MiscMethods {
    public static Vector2 RotatePoint(Vector2 point, Vector2 origin, float radians) {
        point.X -= origin.X;
        point.Y -= origin.Y;
        Vector2 rotatedPoint = new Vector2();
        rotatedPoint.X = (float)(point.X * Math.Cos(radians) - point.Y * Math.Sin(radians)) + origin.X;
        rotatedPoint.Y = (float)(point.Y * Math.Cos(radians) + point.X * Math.Sin(radians)) + origin.Y;
        return rotatedPoint;
    }
}