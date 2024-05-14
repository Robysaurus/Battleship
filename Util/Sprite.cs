using System;
using System.Collections.Generic;
using Battleship.Util.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Battleship.Util;

public class Sprite {
    private readonly Texture2D texture;
    private Vector2 pos;
    private readonly Vector2 origin;
    private readonly float scale;
    private float rot;
    private int rotationNum = 0;
    private readonly bool isRotatable;
    private readonly IShape shape;
    private bool isSelected = false;
    private readonly Action<List<long>> action;
    private readonly List<long> args;
    private readonly bool shouldFollowMouse;
    private Color color = Color.White;
    private bool shouldDraw;

    public Sprite(Texture2D texture, Vector2 pos, Vector2 origin, float scale, float rot, bool isRotatable, IShape shape, Action<List<long>> action, List<long> args, bool shouldFollowMouse, bool shouldDraw) {
        this.texture = texture;
        this.pos = pos;
        this.origin = origin;
        this.scale = scale;
        this.rot = rot;
        this.isRotatable = isRotatable;
        this.shape = shape;
        this.action = action;
        this.args = args;
        this.shouldFollowMouse = shouldFollowMouse;
        this.shouldDraw = shouldDraw;
    }

    public bool WasClicked(Vector2 mousePos) {
        return shape!=null && Contains(mousePos);
    }

    public Texture2D GetTexture() {
        return texture;
    }

    public Vector2 GetPosition() {
        return pos;
    }

    public double GetRotation() {
        return rot;
    }

    public Vector2 GetOrigin() {
        return origin;
    }

    public float GetScale() {
        return scale;
    }

    public void ExecuteAction() {
        action(args);
    }

    public IShape GetShape() {
        return shape;
    }

    public void SetColor(Color color) {
        this.color = color;
    }

    public Color GetColor() {
        return color;
    }

    public bool HasAction() {
        return action != null;
    }

    public bool Contains(Vector2 point) {
        return shape.Contains(point);
    }

    public void SetRotation(float rads) {
        shape.SetRotation(rads);
        rot = rads;
        rotationNum++;
        rotationNum = rotationNum == 6 ? 0 : rotationNum;
    }

    public int GetRotationNum() {
        return rotationNum;
    }

    public bool IsSelected() {
        return isSelected;
    }

    public void UpdateSelected(bool selected) { 
        SetColor(selected ? Color.Gold : Color.White);
        isSelected = selected;
    }

    public bool IsRotatable() {
        return isRotatable;
    }

    public bool ShouldFollowMouse() {
        return shouldFollowMouse;
    }

    public void MoveTo(Vector2 position) {
        pos = position;
        shape.MoveTo(new Vector2(pos.X-origin.X, pos.Y-origin.Y) * BattleshipGame.aspect + BattleshipGame.viewportBounds);
    }

    public bool ShouldDraw() {
        return shouldDraw;
    }

    public void SetVisibility(bool shouldDraw) {
        this.shouldDraw = shouldDraw;
    }

    public void SnapTo(Vector2 position) {
        pos = position;
    }
}