using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Battleship.Util.Shapes;

public class RotatedRect : IShape {
    private Rectangle rect;
    private float rotation;

    public RotatedRect(Rectangle rect, float rotation) {
        this.rect = rect;
        this.rotation = rotation;
    }

    public bool Contains(Vector2 point) {
        Vector2 rotatedPoint = MiscMethods.RotatePoint(point, GetOrigin(), -rotation);
        /*Console.WriteLine("mouse:"+point);
        Console.WriteLine("rotated:"+rotatedPoint);
        Console.WriteLine("rect:"+rect.Location+"  "+rect.Size);*/
        return rect.Contains(rotatedPoint);
    }

    public bool Contains(Point point) {
        return Contains(new Vector2(point.X, point.Y));
    }

    public bool Contains(float x, float y) {
        return Contains(new Vector2(x, y));
    }

    public void MoveTo(float x, float y) {
        rect.X = (int)x;
        rect.Y = (int)y;
    }

    public void MoveTo(Vector2 position) {
        rect.X = (int)position.X;
        rect.Y = (int)position.Y;
    }

    public void MoveTo(Point position) {
        rect.X = position.X;
        rect.Y = position.Y;
    }

    public void Rescale(float scale) {
        rect.X = (int)(rect.X*scale);
        rect.Y = (int)(rect.Y*scale);
    }

    public void Rescale(Matrix scaleMatrix, Viewport viewport) {
        
    }

    public void SetRotation(float rads) {
        rotation = rads;
    }
    
    public Vector2 GetPosition() {
        return new Vector2(rect.X, rect.Y);
    }

    public Vector2 GetOrigin() {
        return new Vector2(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);
    }
}