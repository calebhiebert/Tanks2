using UnityEngine;

public class PlayerInput
{
    public float X { get; set; }
    public float Y { get; set; }
    public float MouseX { get; set; }
    public float MouseY { get; set; }

    public PlayerInput() { }

    public PlayerInput(float x, float y)
    {
        X = x;
        Y = y;
    }

    public PlayerInput(float x, float y, float mx, float my)
    {
        X = x;
        Y = y;
        MouseX = mx;
        MouseY = my;
    }

    public PlayerInput(Vector2 xy, Vector2 mouse)
    {
        X = xy.x;
        Y = xy.y;
        MouseX = mouse.x;
        MouseY = mouse.y;
    }
}