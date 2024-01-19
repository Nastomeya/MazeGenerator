using DataExtensions;
using UnityEngine;

public class TileWithMazeObjectSets : MazeObjsController
{
    internal int currObjectFormation;
    internal int overlappingFormation;
    private int removedObjFormation;
    private int visitedCount = 0;
    public void EanbleMazeObjectSetIfExist(int _id, bool _isEnable)
    {
        int objectSetIndex;
        var objHasBeenFound = FindMazeObjectWithID(_id);
        if (objHasBeenFound == null)
        {
            if (!_isEnable)
                return;
            else
            {
                objHasBeenFound = maze.GetMazeObjectSetlFromSetting(_id);//if not found, get it from all;                
                AddNewMazeObjectType(objHasBeenFound);

                objectSetIndex = Lists.FindIndex(MazeObjects, objHasBeenFound);
                MazeObjects[objectSetIndex].transform.localPosition = Vector3.zero;
            }
        }
        else
            objectSetIndex = Lists.FindIndex(MazeObjects, objHasBeenFound);

        var needToBeShown = ShownOnlyBeenVisitedOrFormationBeenSet(_isEnable);

        int _objFormation = (!needToBeShown) ? 0 : currObjectFormation;
        MazeObjects[objectSetIndex].SetHolderEnable(needToBeShown);
        MazeObjects[objectSetIndex].SetEnable(_objFormation);
    }
    internal void RemoveCurrObjectFromList() => neighbours.RemoveCurrMazeObject();
    internal void BacktrackRemovalObjFormation(TileWithMazeObjectSets nextTile)
    {
        var _neighbourObj = neighbours.ByWhichNeighbour(transform, nextTile.transform);
        UpdateRemovalObjectFormation(nextTile, _neighbourObj);
    }
    internal int UpdateRemovalObjectFormation(TileWithMazeObjectSets nextTile, int _nextObjFormationID = -1)
    {
        _nextObjFormationID = (_nextObjFormationID >= 0) ? _nextObjFormationID : nextObjFormationID;
        removedObjFormation = _nextObjFormationID;
        CreateObjects(MazeObjects);
        UpdateObjectFormation(nextTile);
        return _nextObjFormationID;
    }
    internal override void ResetTile(bool isNumDisplayed = false)
    {
        currObjectFormation = neighbours.totalFormationValue;
        overlappingFormation = neighbours.totalFormationValue;
        removedObjFormation = 0;
        visitedCount = 0;
        foreach (var objectType in MazeObjects)
        {
            if (objectType.transform.gameObject.activeSelf)
                StartCoroutine(objectType.ClearAllObjs());
        }
        base.ResetTile(isNumDisplayed);
    }
    private void UpdateObjectFormation(TileWithMazeObjectSets next)//safe at moment
    {
        //Debug.Log("--------------------------------------------------------------- <color=yellow>Start <b>v</b>  " + index + "</color>");
        //next
        if (!next.isVisited)
        {
            var nextRemoveObjFormation = neighbours.GetNextRemovalObjectValue(removedObjFormation);
            next.currObjectFormation = Bitwise.Remove(next.currObjectFormation, nextRemoveObjFormation);
            next.overlappingFormation = Bitwise.Remove(next.currObjectFormation, nextRemoveObjFormation);
        }

        //curr
        var lastObjFormation = currObjectFormation;
        currObjectFormation = neighbours.OverlappingObjectsReduction(currObjectFormation);

        if (!next.isVisited)
        {
            currObjectFormation = Bitwise.Remove(currObjectFormation, removedObjFormation);
            overlappingFormation = Bitwise.Remove(overlappingFormation, removedObjFormation);
            //Debug.Log(" Has Not Been Visited:  <color=orange> ********************************************************************</color>");
        }

        //Debug.Log(visitedCount + " <color=yellow>lastObjectFormation:  " + lastObjFormation + "</color>  currObjectFormation: " + currObjectFormation + "   <color=orange>   " + Bitwise.GetCombinationNames(new HexagonFormation(), currObjectFormation) + "</color>    overlappingFormaption: " + overlappingFormation + " <color=green>" + Bitwise.GetCombinationNames(new HexagonFormation(), overlappingFormation) + "</color>");
        if (!currObjectFormation.Equals(lastObjFormation) || visitedCount < 1)//1st time visited
            ChangeObjectFormation(currObjectFormation, overlappingFormation);
        visitedCount++;
    }
    private bool ShownOnlyBeenVisitedOrFormationBeenSet(bool _isEnable)
    {
        if (!isVisited || currObjectFormation == neighbours.totalFormationValue)//initial set is same as totalFormation
            _isEnable = false;
        return _isEnable;
    }
}
