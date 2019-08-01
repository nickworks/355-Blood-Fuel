using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputMode
{
    Gamepad,
    MouseKeyboard
}
public static class PlayerInput
{
    public static InputMode mode = InputMode.MouseKeyboard;
}
public class InputModeSwapper : MonoBehaviour {

	void Update () {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        //if (mx > 1 || my > 1 || mx < -1 || my < -1) PlayerInput.mode = InputMode.MouseKeyboard;
        float h = Input.GetAxis("Horizontal2");
        float v = Input.GetAxis("Vertical2");
        //if (Mathf.Abs(h) > .2f || Mathf.Abs(v) > .2f) PlayerInput.mode = InputMode.Gamepad;
    }
}
