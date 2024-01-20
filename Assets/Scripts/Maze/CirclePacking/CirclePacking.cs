using UnityEngine;

public class CirclePacking : BaseCirclePacking
{
    [SerializeField] private Vector2 boundarySize;

    protected override void Awake()
    {
        base.Awake();
        boundarySize = setting.BoundarySize;//new Vector2(wallThickness, wallThickness);
    }
    protected override Vector3 GetRandomPosition(int _wallFormation)
    {
        var RandomXPosition = transform.position.x + Utils.RandomRange(-boundarySize.x / 2f, boundarySize.x / 2f);
        var RandomYPosition = transform.position.z + Utils.RandomRange(-boundarySize.y / 2f, boundarySize.y / 2f);
        SetRandomObjectIndex();
        return new Vector3(RandomXPosition, objectPrefabs[objectIndex].transform.position.y, RandomYPosition);
    }
    protected override void SetNumberOfObject(int _wallFormation) => currentNumOfObject = setting.NumberOfObjects;// numberOfObjRequired;
}
