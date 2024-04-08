using System;
using System.Collections;

namespace Battleship.Boats;

public class Boat {
    private int hp;
    private ArrayList coords; 
    
    public Boat(int size, ArrayList coords) {
        this.hp = size;
        this.coords = coords;
    }

    public int GetHP() {
        return hp;
    }

    public bool IsDead() {
        return hp == 0;
    }

    public bool AttemptHit(string coord) {
        foreach(string s in coords) {
            if (s.Equals(coord)) {
                hp--;
                coords.Remove(s);
                return true;
            }
        }
        return false;
    }
}