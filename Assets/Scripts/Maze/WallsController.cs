using System.Collections.Generic;

public class WallsController : TileBase
{
    internal List<IMazeWall> Walls { get; private set; }
    internal void InitWallIdentities(List<IMazeWall> _walls)
    {
        Walls = new List<IMazeWall>();
        CreateWalls(_walls);
    }
    protected IMazeWall FindWallWithID(int _id)
    {
        IMazeWall newFoundWall = Walls.Find(x => x.id == _id);
        return newFoundWall;
    }
    protected void CreateWalls(List<IMazeWall> enabledWallList)
    {
        for (int i = 0; i < enabledWallList.Count; i++)
        {
            var wallHasFound = FindWallWithID(enabledWallList[i].id);
            if (wallHasFound == null)
                AddNewWallType(enabledWallList[i]);
            else
                wallHasFound.SetWallHolderEnable(maze.GetCurrWallTypes()[enabledWallList[i].id]);
        }
    }
    protected void AddNewWallType(IMazeWall _newWall)
    {
        var clone = CloneWall(_newWall);
        Walls.Add(clone);
    }
    private IMazeWall CloneWall(IMazeWall _newWall)
    {
        var newObject = Instantiate(_newWall.transform);
        newObject.SetParent(transform);

        var newWall = newObject.GetComponent<IMazeWall>();
        newWall.SetID(_newWall.id);
        StartCoroutine(newWall.ClearAllWalls());
        return newWall;
    }
    protected void ChangeWallFormation(int _wallFormation, int _overlappingFormaption)
    {
        for (int i = 0; i < Walls.Count; i++)
        {
            if (Walls[i].transform.gameObject.activeSelf)
                if (Walls[i].InOverlappingMode)
                    Walls[i].SetWallsEnable(_overlappingFormaption);
                else
                    Walls[i].SetWallsEnable(_wallFormation);
        }
    }
}