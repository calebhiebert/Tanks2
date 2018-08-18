using UnityEngine;
using UnityEditor;

public class PlayerInput
{
    public float X { get; set; }
    public float Y { get; set; }

    public PlayerInput(float x, float y)
    {
        this.X = x;
        this.Y = y;
    }
}