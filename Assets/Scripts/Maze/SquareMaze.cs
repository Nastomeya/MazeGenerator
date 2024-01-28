using UnityEngine;

public class SquareMaze : BaseMaze
{
    protected override void CreateANewTile(ref int index, int y, int x)
    {
        var newTile = Instantiate(tileWithWallsPrefab);
        newTile.name += "_" + index;
        newTile.SetMaze(this);

        currNeighbours = new SquareNeighbours(index, Width, totalTileNum, tiles);
        newTile.InitObjectsIdentities(MazeObjectSetting.ToListOfMazeObjects());
        newTile.Init(index, currNeighbours);

        newTile.transform.position = new Vector3(x - offset.x, 0, y - offset.y);
        newTile.transform.SetParent(transform);

        tiles.Add(newTile);
        index++;
    }
}