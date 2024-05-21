using System;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Battleship.Util.InputHandling;

public static class MouseInputHandler {
    public static void HandleLeftMouseClick(Vector2 mousePos) {
        foreach (Sprite s in BattleshipGame.sprites) {
            if (s.WasClicked(mousePos)) {
                string[] point = MiscMethods.CoordsToClosestPoint(mousePos);
                if (s.IsSelected() && point[0]!="0" && point[1]!="0") {
                    s.SnapTipTo(MiscMethods.TranslatePosToCoords(point[0][0], int.Parse(point[1]), BattleshipGame.aspect));
                    s.UpdateSelected(false);
                } else if(!s.IsSelected()){
                    s.UpdateSelected(true);
                }
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
                i = i == 6 ? 0 : i;
                s.SetRotation(BattleshipGame.rotations[i]);
            }
        }
    }
}