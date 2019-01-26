using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public static class GameInput {

    public static Vector2 MoveAxis {
        get {
            return input.GetAxis2D("Horiz", "Vert");
        }
    }

    public static bool Boost {
        get {
            return input.GetButton("Boost");
        }
    }

    public static bool BoostDown {
        get {
            return input.GetButtonDown("Boost");
        }
    }

    public static bool Shoot {
        get {
            return input.GetButton("Shoot");
        }
    }

    public static bool ShootDown {
        get {
            return input.GetButtonDown("Shoot");
        }
    }


    private static Player input {
        get {
            return ReInput.players.GetPlayer(1);
        }
    }

}
