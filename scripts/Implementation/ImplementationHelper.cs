using Godot;

namespace CamelliaFrontend.scripts.Implementation;

public static class ImplementationHelper
{
    public static Vector3 ToGodot(this System.Numerics.Vector3 v) => new Vector3(v.X, v.Y, v.Z);
    public static System.Numerics.Vector3 ToClr(this Vector3 v) => new System.Numerics.Vector3(v.X, v.Y, v.Z);
    
    public static Quaternion ToGodot(this System.Numerics.Quaternion q) => new Quaternion(q.X, q.Y, q.Z, q.W);
    public static System.Numerics.Quaternion ToClr(this Quaternion q) => new System.Numerics.Quaternion(q.X, q.Y, q.Z, q.W);
}