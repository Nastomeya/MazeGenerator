using UnityEngine;

public static class Vector3Extensions
{
    //easy to read, clean code
    public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
    {
        float newX = x ?? original.x; //x.HasValue ? x.Value : original.x;
        float newY = y ?? original.y; //y.HasValue ? y.Value : original.y;
        float newZ = z ?? original.z; //z.HasValue ? z.Value : original.z;

        return new Vector3(newX, newY, newZ);
    }
    public static bool IsBehind(this Vector3 queried, Vector3 forward)
    {
        return (queried.z - forward.z) < 0f;
    }
    public static float DistanceFrom(this Vector3 queried, Vector3 target)
    {
        float diff_x = queried.x - target.x;
        float diff_z = queried.z - target.z;
        return Mathf.Sqrt(diff_x * diff_x + diff_z * diff_z);
    }
}
