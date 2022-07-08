using UnityEngine;

public class StoneFloor : MonoBehaviour
{
    [SerializeField] private FloorRotation floorRotation;
    private float degreeRange;
    private float[] edges;
    private Vector3 randomAllowed;
    private void Awake()
    {
        degreeRange = floorRotation.DegreeRange;
        edges = floorRotation.Edges;
        randomAllowed = floorRotation.AllowedRandomAxis;
    }
    internal void VabrateFloor()
    {
        float xDegree = GetRandomDegree((int)randomAllowed.x, transform.rotation.eulerAngles.x);
        float yDegree = GetRandomDegree((int)randomAllowed.y, transform.rotation.eulerAngles.y, true);
        float zDegree = GetRandomDegree((int)randomAllowed.z, transform.rotation.eulerAngles.z);
        gameObject.transform.rotation = Quaternion.Euler(xDegree, yDegree, zDegree);
    }
    private float GetRandomDegree(int _randomAllowed, float _originalDegree, bool isOnYAxis = false)
    {
        float degree, addition = 0;
        if (_randomAllowed == 1)
        {
            if (isOnYAxis)
            {
                var randomIndex = Utils.RandomRange(0, edges.Length);
                addition = edges[randomIndex];
            }

            degree = Utils.RandomRange(-degreeRange, degreeRange) + addition;
        }
        else
            degree = _originalDegree;
        return degree;
    }
}
