using Microsoft.Xna.Framework;
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
        return rect.Contains(rotatedPoint);
    }

    public void MoveTo(Vector2 position) {
        rect.Location = position.ToPoint();
    }
    
    public void Rescale(float scale, Vector2 newViewportBounds, Vector2 oldViewportBounds) {
        rect.Size = (rect.Size.ToVector2() * scale).ToPoint();
        rect.Location = ((rect.Location.ToVector2() - oldViewportBounds) * scale + newViewportBounds).ToPoint();
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