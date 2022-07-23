using MathCalculation;
using System.Collections.Generic;
using UnityEngine;

public class WallPositioner : MonoBehaviour
{
    [SerializeField] private WallController wallPrefab;
    [SerializeField, Range(0.35f, 1f)] private float radius = 0.862f;
    [SerializeField] private float degree, times;
    [SerializeField] private TileShapes tileShapes;

    private readonly float initialDegree = 90f;
    private int numOfSides;
    private float eachSideDegree;

    public bool isGenerateAll = false;
    internal HashSet<WallController> allWalls;
    private void Start()
    {
        GetUpdatedData();
        allWalls = new HashSet<WallController>();
    }
    private void GetUpdatedData()
    {
        //        UpdateNumberOfSide();
        eachSideDegree = 360f / numOfSides;
        degree = initialDegree + eachSideDegree;
    }
    private void UpdateNumberOfSide() => numOfSides = tileShapes == TileShapes.Hexagon ? 6 : 4;

    public void PlaceWalls()
    {
        GenerateAllWalls();
        if (allWalls.Count > 0)
        {
            GetUpdatedData();
            ResetAllWalls();
            var currWalls = allWalls.GetEnumerator();
            int index = 0;
            while (currWalls.MoveNext() && index < numOfSides)
            {
                degree = (degree - eachSideDegree) % 360f;
                var origin = new Vector2(transform.position.x, transform.position.z);
                Vector2 newPosi = AdvanceMath.PointInCircumference(radius, degree, origin);

                currWalls.Current.transform.position = new Vector3(newPosi.x, currWalls.Current.transform.position.y, newPosi.y);
                currWalls.Current.transform.LookAt(transform);//newWall.transform.rotation = Quaternion.AngleAxis(degree, Vector3.up);//

                currWalls.Current.transform.gameObject.SetActive(true);

                if (tileShapes == TileShapes.Hexagon)
                    currWalls.Current.wallSizeAdjuster.SetWallWidth(radius, eachSideDegree);
                else
                {
                    //currWalls.Current.wallSizeAdjuster.SetWallWidth(radius * 2);
                    var powerRadius = Mathf.Pow(radius, 2f);
                    var newRadius = Mathf.Sqrt(powerRadius + powerRadius);
                    currWalls.Current.wallSizeAdjuster.SetWallWidth(newRadius, eachSideDegree);
                }
                currWalls.Current?.pillarPositioner?.SetPosition(radius, degree + eachSideDegree / 2f, origin);

                index++;
            }
        }
    }
    private void GenerateAllWalls()
    {
        UpdateNumberOfSide();
        if (allWalls.Count < numOfSides)
        {
            int needMoreNum = numOfSides - allWalls.Count;
            for (int i = 0; i < needMoreNum; i++)
            {
                var newWall = GenerateAWall();
                allWalls.Add(newWall);
            }
        }
    }
    private WallController GenerateAWall()
    {
        var newWall = Instantiate(wallPrefab);
        newWall.gameObject.SetActive(false);
        return newWall;
    }
    private void ResetAllWalls()
    {
        var currWalls = allWalls.GetEnumerator();
        while (currWalls.MoveNext())
            currWalls.Current.gameObject.SetActive(false);
    }
}
