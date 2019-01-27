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

    public class ButtonWrapper {
        string btn;

        public ButtonWrapper(string btn) {
            this.btn = btn;
        }

        public bool Pressing => input.GetButton(btn);
        public bool Down => input.GetButtonDown(btn);
        public bool Up => input.GetButtonUp(btn);
    }

    public static readonly ButtonWrapper
        Boost = new ButtonWrapper("Boost"),
        Shoot = new ButtonWrapper("Shoot"),
        MainMenu = new ButtonWrapper("Menu"),
        Start = new ButtonWrapper("Start");

    private static Player input {
        get {
            return ReInput.players.GetPlayer(0);
        }
    }

}
