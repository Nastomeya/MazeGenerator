using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableData/Maze/FloorRotation", fileName = "FloorRotation")]
public class FloorRotation : ScriptableObject
{
    [SerializeField] private float degreeRange = 3f;
    [SerializeField] private float[] edges = new float[4] { 0, 90, 180, 270 };
    /// <summary> 1 allowed, 0 is not allowed </summary>
    [SerializeField] private Vector3 allowedRandomAxis = new Vector3(1, 1, 1);
    internal float DegreeRange { get { return degreeRange; } }
    internal float[] Edges { get { return edges; } }
    internal Vector3 AllowedRandomAxis { get { return allowedRandomAxis; } }
}