using MathCalculation;
using UnityEngine;

public class HexagonMaze : MazeBase
{
    /// <summary>Please check the scale size of original prefab, which should be 0.34f. But it's now 0.68f, so the ratio should be 2</summary>    
    private readonly int tileSizeRatio = 2;//1

    //public override string GetFileNameAndPath() => GetFilePath() + "/hexagonMazeWallData.txt";
    protected override void CreateANewTile(ref int index, int y, int x)
    {
        var newTile = Instantiate(tileWithWallsPrefab);
        newTile.name += "_" + index;
        newTile.SetMaze(this);

        currNeighbours = new HexagonNeighbours(index, width, totalTileNum, tiles);
        newTile.InitObjectsIdentities(MazeObjectSetting.ToListOfMazeObjects());
        newTile.Init(index, currNeighbours);

        float yPosition = GetYPosition(index, y);
        float xPosition = GetXPosition(x);

        newTile.transform.position = new Vector3(xPosition, 0, yPosition);
        newTile.transform.SetParent(transform);

        tiles.Add(newTile);
        index++;
    }
    private float GetYPosition(int index, int y)
    {
        float rowAlignment = 0.216f;
        float yPosi = y - offset.y;
        var invertedValue = BasicMath.Convert2USequenceValue(y, height);
        yPosi -= (invertedValue * 0.13f);

        if (IsIndexAnOddNum(index))//it's an odd index
            yPosi -= rowAlignment;
        else
            yPosi += rowAlignment;

        yPosi *= tileSizeRatio;
        return yPosi;
    }
    private float GetXPosition(int x)
    {
        float xPosi = x - offset.x;
        var invertedValue = BasicMath.Convert2USequenceValue(x, width);
        xPosi -= (invertedValue * 0.243f);

        xPosi *= tileSizeRatio;
        return xPosi;
    }
    private bool IsIndexAnOddNum(int _index) => (_index % width % 2) == 1;
}