using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableData/Maze/CirclePackingSetting", fileName = "CirclePackingSetting")]
public class CirclePackingSetting : ScriptableObject
{
    [SerializeField] private int numberOfObjRequired = 25;
    [Header("")]
    [SerializeField] private float wallThickness = 0.25f;
    [SerializeField] private float innerWallFormationRadius = 0.36f;
    [Header("")]
    [SerializeField] private float minObjectSize = 0.03f;
    [SerializeField] private float maxObjectSize = 0.1f;
    [Header("")]
    [SerializeField] private bool isRandomRotation = false;
    internal float WallThickness { get { return wallThickness; } }
    internal float FullRange { get { return innerWallFormationRadius + wallThickness; } }
    internal float HalfRange { get { return innerWallFormationRadius; } }
    internal int NumberOfObjects { get { return numberOfObjRequired; } }
    internal float GetRandomSize() => Utils.RandomRange(minObjectSize, maxObjectSize);
    internal Vector2 BoundarySize { get { return new Vector2(wallThickness, wallThickness); } }
    /// <summary>This is only available for Hexagon and Square Circle Packing Only</summary>
    internal bool IsRandomRotation { get { return isRandomRotation; } }

}
