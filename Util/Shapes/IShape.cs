using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Battleship.Util.Shapes;

public interface IShape {
    public bool Contains(Vector2 point);
    public void MoveTo(Vector2 position);
    public void Rescale(float scale, Vector2 newViewportBounds, Vector2 oldViewportBounds);
    void SetRotation(float rads);

    public Vector2 GetPosition();
}