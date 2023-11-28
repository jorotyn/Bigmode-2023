using UnityEngine;

public static class InputManager
{
    public static Vector2 CurrentDirectionalInput() =>
        new Vector2(Input.GetAxis("Horizontal"),
                    Input.GetAxis("Vertical")).normalized;
}
