using DataExtensions;
using UnityEngine;

public class TileWithWalls : WallsController
{
    internal int currWallFormation;
    internal int overlappingFormation;
    private int removeWallFormation;
    private int visitedCount = 0;
    public void EanbleWallIfExist(int _id, bool _isEnable)
    {
        int wallIndex;
        var wallHasFound = FindWallWithID(_id);
        if (wallHasFound == null)
        {
            if (!_isEnable)
                return;
            else
            {
                wallHasFound = maze.GetWallFromAll(_id);//if not found, get it from all;                
                AddNewWallType(wallHasFound);

                wallIndex = Lists.FindIndex(Walls, wallHasFound);
                Walls[wallIndex].transform.localPosition = Vector3.zero;
            }
        }
        else
            wallIndex = Lists.FindIndex(Walls, wallHasFound);

        var needToBeShown = ShownOnlyBeenVisitedOrFormationBeenSet(_isEnable);

        int _wallFormation = (!needToBeShown) ? 0 : currWallFormation;
        Walls[wallIndex].SetWallHolderEnable(needToBeShown);
        Walls[wallIndex].SetWallsEnable(_wallFormation);
    }
    internal void RemoveCurrWallFromList() => neighbours.RemoveCurrWall();
    internal void BacktrackRemovalWallFormation(TileWithWalls nextTile)
    {
        var _neighbourWall = neighbours.ByWhichNeighbour(transform, nextTile.transform);
        UpdateRemovalWallFormation(nextTile, _neighbourWall);
    }
    internal int UpdateRemovalWallFormation(TileWithWalls nextTile, int _nextWallFormationID = -1)
    {
        _nextWallFormationID = (_nextWallFormationID >= 0) ? _nextWallFormationID : nextWallFormationID;
        removeWallFormation = _nextWallFormationID;
        CreateWalls(Walls);
        UpdateWallFormation(nextTile);
        return _nextWallFormationID;
    }
    internal override void ResetTile(bool isNumDisplayed = false)
    {
        currWallFormation = neighbours.totalFormationValue;
        overlappingFormation = neighbours.totalFormationValue;
        removeWallFormation = 0;
        visitedCount = 0;
        foreach (var wallType in Walls)
        {
            if (wallType.transform.gameObject.activeSelf)
                StartCoroutine(wallType.ClearAllWalls());
        }
        base.ResetTile(isNumDisplayed);
    }
    private void UpdateWallFormation(TileWithWalls next)//safe at moment
    {
        //Debug.Log("--------------------------------------------------------------- <color=yellow>Start <b>v</b>  " + index + "</color>");
        //next
        if (!next.isVisited)
        {
            var nextRemoveWallFormation = neighbours.GetNextRemovalWallValue(removeWallFormation);
            next.currWallFormation = Bitwise.Remove(next.currWallFormation, nextRemoveWallFormation);
            next.overlappingFormation = Bitwise.Remove(next.currWallFormation, nextRemoveWallFormation);
        }

        //curr
        var lastWallFormation = currWallFormation;
        currWallFormation = neighbours.OverlappingWallsReduction(currWallFormation);

        if (!next.isVisited)
        {
            currWallFormation = Bitwise.Remove(currWallFormation, removeWallFormation);
            overlappingFormation = Bitwise.Remove(overlappingFormation, removeWallFormation);
            //Debug.Log(" Has Not Been Visited:  <color=orange> ********************************************************************</color>");
        }

        //Debug.Log(visitedCount + " <color=yellow>lastWallFormation:  " + lastWallFormation + "</color>  currWallFormation: " + currWallFormation + "   <color=orange>   " + Bitwise.GetCombinationNames(new HexagonFormation(), currWallFormation) + "</color>    overlappingFormaption: " + overlappingFormation + " <color=green>" + Bitwise.GetCombinationNames(new HexagonFormation(), overlappingFormation) + "</color>");
        if (!currWallFormation.Equals(lastWallFormation) || visitedCount < 1)//1st time visited
            ChangeWallFormation(currWallFormation, overlappingFormation);
        visitedCount++;
    }
    private bool ShownOnlyBeenVisitedOrFormationBeenSet(bool _isEnable)
    {
        if (!isVisited || currWallFormation == neighbours.totalFormationValue)//initial set is same as totalFormation
            _isEnable = false;
        return _isEnable;
    }
}
