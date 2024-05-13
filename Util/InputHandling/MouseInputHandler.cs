using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Battleship.Util.InputHandling;

public static class MouseInputHandler {
    public static void HandleLeftMouseClick(Vector2 mousePos) {
        foreach (Sprite s in BattleshipGame.sprites) {
            if (s.WasClicked(mousePos)) {
                s.UpdateSelected(!s.IsSelected());
                if (s.HasAction()) {
                    s.ExecuteAction();
                }
            } else {
                s.UpdateSelected(false);
            }
        }
    }

    public static void HandleRightMouseClick() {
        foreach (Sprite s in BattleshipGame.sprites) {
            if (s.IsSelected() && s.IsRotatable()) {
                int i = s.GetRotationNum() + 1;
                i = i == 4 ? 0 : i;
                s.SetRotation(BattleshipGame.rotations[i]);
            }
        }
    }
}