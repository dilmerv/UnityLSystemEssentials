using System;

[Serializable]
public class LSystemState
{
    public float size;
    public float angle;
    public float x;
    public float y;
    public float z;

    public LSystemState Clone() 
    { 
        return (LSystemState) this.MemberwiseClone(); 
    }

    public override string ToString()
    {
        return $"x: {x} y: {y} z: {z} size: {size} angle: {angle}";
    }
}