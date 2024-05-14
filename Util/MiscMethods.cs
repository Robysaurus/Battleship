using System;

namespace Battleship.Util;

using Vector2 = Microsoft.Xna.Framework.Vector2;

public static class MiscMethods {
    public static Vector2 RotatePoint(Vector2 point, Vector2 origin, float radians) {
        point.X -= origin.X;
        point.Y -= origin.Y;
        Vector2 rotatedPoint = new Vector2{
            X = (float)(point.X * Math.Cos(radians) - point.Y * Math.Sin(radians)) + origin.X,
            Y = (float)(point.Y * Math.Cos(radians) + point.X * Math.Sin(radians)) + origin.Y
        };
        return rotatedPoint;
    }

    public static Vector2 TranslatePosToCoords(char column, int row, float scale) {
        float x = column switch{
            'A' => 25.5f,
            'B' => 47.5f,
            'C' => 69.5f,
            'D' => 91.5f,
            'E' => 113.5f,
            'F' => 135.5f,
            'G' => 157.5f,
            'H' => 179.5f,
            'I' => 201.5f,
            'J' => 223.5f,
            'K' => 245.5f,
            'L' => 267.5f,
            'M' => 289.5f,
            'N' => 311.5f,
            'O' => 333.5f,
            _ => 0
        };

        int col = column switch{
            'A' => 1,
            'B' => 2,
            'C' => 3,
            'D' => 4,
            'E' => 5,
            'F' => 6,
            'G' => 7,
            'H' => 8,
            'I' => 7,
            'J' => 6,
            'K' => 5,
            'L' => 4,
            'M' => 3,
            'N' => 2,
            'O' => 1,
            _ => 0
        };

        float y = row switch{
            1 => 118.5f,
            2 => 142.5f,
            3 => 166.5f,
            4 => 190.5f,
            5 => 214.5f,
            6 => 238.5f,
            7 => 262.5f,
            8 => 286.5f,
            9 => 298.5f,
            10 => 310.5f,
            11 => 322.5f,
            12 => 334.5f,
            13 => 346.5f,
            14 => 358.5f,
            _ => 0
        };
        y -= (row >= 8 ? col + 7 - row : col - 1) * 12;
        
        return new Vector2(x, y) * scale;
    }

    // public static char[] CoordsToClosestPoint(Vector2 coords) {
    //     
    // }
}