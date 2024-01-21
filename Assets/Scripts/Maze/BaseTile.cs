using UnityEngine;

public abstract class BaseTile : MonoBehaviour
{
    [SerializeField] internal TrackText trackText;
    internal int Turns { get { return neighbours.GetAvaliableWalls(); } }

    internal BaseNeighbours neighbours;
    internal int index;
    internal bool isVisited = false;

    protected BaseMaze maze;
    protected int nextObjFormationID;

    private StoneFloor stoneFloor;

    internal void SetMaze(BaseMaze _maze) => maze = _maze;
    internal void Init(int i, BaseNeighbours _neighbours)//internal void Init(int _width, int _totalTileNum, List<TileWithWalls> _tiles, int i)
    {
        stoneFloor = trackText.GetComponent<StoneFloor>();
        index = i;
        neighbours = _neighbours;// new SquareNeighbours(i, _width, _totalTileNum, _tiles);
        ResetTile();
    }
    internal int GetNeighbourIndex(StraightPathController straightPathController)//STEP 1
    {
        nextObjFormationID = neighbours.GetRandomWall(straightPathController);//loop inside
        int nextIndex = neighbours.GetNeighbourIndex(nextObjFormationID);
        return nextIndex;
    }
    internal void SetCurrentTileVisited(int trackMarkCounter = -1)
    {
        if (trackMarkCounter >= 0)
            trackText.SetNumber(trackMarkCounter);

        trackText.SetTextOnOff(RevealObjectController.Instance.isTextOnOff);
        trackText.SetMarkerOnOff(true);
        trackText.Init();
        isVisited = true;
        stoneFloor.VabrateFloor();
    }
    internal virtual void ResetTile(bool isIndexOnDisplay = false)
    {
        var currIndex = isIndexOnDisplay ? index : -1;
        trackText.SetTextOnOff(false);
        trackText.SetNumber(currIndex);
        trackText.End();
        trackText.SetMarkerOnOff(false);
        neighbours.Reset();
        isVisited = false;
    }
}