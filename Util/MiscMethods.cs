using System;
using Microsoft.Xna.Framework;

namespace Battleship.Util;

using Vector2 = Vector2;

public static class MiscMethods {
    public static bool CheckAllTilesOfP1Boat(Sprite s, string[] simulatedPosition) {
        string type = s.GetBoatType();
        int rows = type == "t" ? 3 : Int32.Parse(type);
        string[][] simulatedOccupiedTiles = new string[rows][];
        if (type == "1" || type == "2" || type == "3" || type == "4") {
            for (int i = 0; i < Int32.Parse(type); i++) {
                int xSign, ySign;
                if (s.GetRotationNum() == 1 || s.GetRotationNum() == 4) {
                    xSign = 0;
                    ySign = 2 * (Math.Sin(Math.PI - s.GetRotation()) > 0 ? 1 : -1);
                } else {
                    xSign = Math.Cos(Math.PI - s.GetRotation()) > 0 ? 1 : -1;
                    ySign = Math.Sin(Math.PI - s.GetRotation()) > 0 ? 1 : -1;
                }
                string[] coords = CoordsToClosestPoint(TranslatePosToCoords(simulatedPosition[0][0], Int32.Parse(simulatedPosition[1]), BattleshipGame.aspect) + i * new Vector2(22*-xSign, 12*ySign) * BattleshipGame.aspect + BattleshipGame.viewportBounds);
                simulatedOccupiedTiles[i] = coords;
                if (!IsValidP1Tile(coords[0][0], Int32.Parse(coords[1]), type)) {
                    return false;
                }
            }
        }else if (type == "t") {
            
        }else if (type == "5") {
            
        }
        s.UpdateOccupiedTiles(simulatedOccupiedTiles);
        return true;
    }
    public static bool IsValidP1Tile(char col, int row, string type) {
        if (col == '0' || row == 0) {
            return false;
        }
        if (BattleshipGame.p1Board[ColLetterToNumber(col) - 1][row - 1] != type[0] && BattleshipGame.p1Board[ColLetterToNumber(col) - 1][row - 1] != '-') {
            return false;
        }
        switch (col) {
            case 'A' when row <= 4:
            case 'B' when row <= 4:
            case 'C' when row <= 5:
            case 'D' when row <= 6:
            case 'E' when row <= 6:
            case 'F' when row <= 6:
            case 'G' when row <= 7:
            case 'H' when row <= 8:
            case 'I' when row <= 7:
            case 'J' when row <= 6:
            case 'K' when row <= 6:
            case 'L' when row <= 6:
            case 'M' when row <= 5:
            case 'N' when row <= 4:
            case 'O' when row <= 4:
                return true;
            default:
                return false;
        }
    }
    
    public static bool IsValidP2Tile(char col, int row) {
        switch (col) {
            case 'A' when row >= 5 && row <= 8:
            case 'B' when row >= 5 && row <= 9:
            case 'C' when row >= 6 && row <= 10:
            case 'D' when row >= 7 && row <= 11:
            case 'E' when row >= 7 && row <= 12:
            case 'F' when row >= 7 && row <= 13:
            case 'G' when row >= 8 && row <= 14:
            case 'H' when row >= 9 && row <= 14:
            case 'I' when row >= 8 && row <= 14:
            case 'J' when row >= 7 && row <= 13:
            case 'K' when row >= 7 && row <= 12:
            case 'L' when row >= 7 && row <= 11:
            case 'M' when row >= 6 && row <= 10:
            case 'N' when row >= 5 && row <= 9:
            case 'O' when row >= 5 && row <= 8:
                return true;
            default:
                return false;
        }
    }
    
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

        int col = ColLetterToNumber(column);

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

    public static string[] CoordsToClosestPoint(Vector2 coords) {
        for (int i = 0; i < 15; i++) {
            for (int z = 0; z < BattleshipGame.tiles[i].Length; z++) {
                if (BattleshipGame.tiles[i][z].Contains(coords)) {
                    i++;
                    z++;
                    char col = ColNumberToLetter(i);
                    return new string[]{ ""+col, ""+z };
                }
            }
        }
        return new string[]{ "0", "0" };
    }

    public static int ColLetterToNumber(char col) {
        int column = col switch{
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
        return column;
    }
    
    public static char ColNumberToLetter(int col) {
        char column = col switch{
            1 => 'A',
            2 => 'B',
            3 => 'C',
            4 => 'D',
            5 => 'E',
            6 => 'F',
            7 => 'G',
            8 => 'H',
            9 => 'I',
            10 => 'J',
            11 => 'K',
            12 => 'L',
            13 => 'M',
            14 => 'N',
            15 => 'O',
            _ => 'Z'
        };
        return column;
    }
}