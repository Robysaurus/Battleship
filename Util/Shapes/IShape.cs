using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Battleship.Util.Shapes;

public interface IShape {
    public bool Contains(Vector2 point);
    public bool Contains(Point point);
    public bool Contains(float x, float y);

    public void MoveTo(float x, float y);
    public void MoveTo(Vector2 position);
    public void MoveTo(Point position);

    public void Rescale(float scale);
    public void Rescale(Matrix scaleMatrix, Viewport viewport);
    void SetRotation(float rads);
    
    public Vector2 GetPosition();
    public Vector2 GetOrigin();
}