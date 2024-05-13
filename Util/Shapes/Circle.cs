using System;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Battleship.Util.Shapes;

public class Circle : IShape {
    private float radius;
    private Vector2 center;

    public Circle(float radius, Vector2 center) {
        this.radius = radius;
        this.center = center;
    }

    public float GetRadius() {
        return radius;
    }

    public void MoveTo(float x, float y) {
        MoveTo(new Vector2(x, y));
    }

    public void MoveTo(Vector2 newPos) {
        center = newPos + new Vector2(radius);
    }

    public void MoveTo(Point position) {
        MoveTo(position.ToVector2());
    }

    public void Rescale(float scale, Vector2 newViewportBounds, Vector2 oldViewportBounds) {
        radius *= scale;
        center -= oldViewportBounds;
        center *= scale;
        center += newViewportBounds;
    }

    public Vector2 GetPosition() {
        return center;
    }

    public Vector2 GetOrigin() {
        return center;
    }

    public void SetRotation(float rads) {
        //Lol imagine rotating a circle
    }

    public bool Contains(Vector2 point) {
        return Math.Sqrt(Math.Pow(point.X - center.X, 2) + Math.Pow(point.Y - center.Y, 2)) <= radius;
    }
    
    public bool Contains(Point point) {
        return Math.Sqrt(Math.Pow(point.X - center.X, 2) + Math.Pow(point.Y - center.Y, 2)) <= radius;
    }
    
    public bool Contains(float x, float y) {
        return Math.Sqrt(Math.Pow(x - center.X, 2) + Math.Pow(y - center.Y, 2)) <= radius;
    }
}