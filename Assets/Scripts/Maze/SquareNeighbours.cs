using DataExtensions;
using MathCalculation;
using System.Collections.Generic;
using UnityEngine;

public class SquareNeighbours : NeighboursBase
{
    private SquareFormation wallFormation;
    internal SquareNeighbours(int _index, int _width, int _numOfTotalTile, List<TileWithMazeObjectSets> _tiles) : base(_index, _width, _numOfTotalTile, _tiles)
    {
        wallFormation = new SquareFormation();
        totalFormationValue = GetTotalFormationValue(wallFormation);
    }
    internal override int GetWallFormation() => (int)wallFormation;
    internal override void SetWallFormation(int _wallFormation) => wallFormation = (SquareFormation)_wallFormation;

    internal override void InitFormation() => wallFormation = SquareFormation.top;
    internal override int[] GetElements() => Bitwise.GetElements(wallFormation);
    internal override void WallIncrement()
    {
        wallFormation = (SquareFormation)((int)(wallFormation + 1) % totalFormationValue);//wallFormation++;//
        if (wallFormation == 0)
            wallFormation = (SquareFormation)totalFormationValue;
    }
    internal override int ByWhichNeighbour(Transform curr, Transform next)
    {
        var _index = -1;
        var direction = (curr.position - next.position).normalized;
        float degree = AdvanceMath.GetDegree(direction);
        if (degree == 0)
            _index = 1;//top
        else if (degree == 90)
            _index = 2;//right
        else if (degree == 180)
            _index = 4;//bottom
        else if (degree == 270)
            _index = 8;//left
        return _index;
    }
    internal override int GetNeighbourIndex(int _nextWallFormation)
    {
        int nextIndex = -1;

        if (_nextWallFormation == 1)//TOP
            nextIndex = GetTopIndex();
        else if (_nextWallFormation == 2)//RIGHT
            nextIndex = GetRightIndex();
        else if (_nextWallFormation == 4)//BOTTOM
            nextIndex = GetBottomIndex();
        else if (_nextWallFormation == 8)//LEFT
            nextIndex = GetLeftIndex();

        return nextIndex;
    }
    internal override int OverlappingObjectsReduction(int currWallFormation)
    {
        int topIndex = GetTopIndex();
        int rightIndex = GetRightIndex();
        int bottomIndex = GetBottomIndex();
        int leftIndex = GetLeftIndex();

        if (topIndex >= 0 && tiles[topIndex].isVisited)
            currWallFormation = CheckFormationExistence(topIndex, currWallFormation, (int)SquareFormation.top, (int)SquareFormation.bottom);
        if (rightIndex >= 0 && tiles[rightIndex].isVisited)
            currWallFormation = CheckFormationExistence(rightIndex, currWallFormation, (int)SquareFormation.right, (int)SquareFormation.left);
        if (bottomIndex >= 0 && tiles[bottomIndex].isVisited)
            currWallFormation = CheckFormationExistence(bottomIndex, currWallFormation, (int)SquareFormation.bottom, (int)SquareFormation.top);
        if (leftIndex >= 0 && tiles[leftIndex].isVisited)
            currWallFormation = CheckFormationExistence(leftIndex, currWallFormation, (int)SquareFormation.left, (int)SquareFormation.right);

        return currWallFormation;
    }
    internal override int GetNextRemovalObjectValue(int _removeWallFormationValue)
    {
        int topBottomPairValue = (int)(SquareFormation.top | SquareFormation.bottom);
        int rightLeftPairValue = (int)(SquareFormation.right | SquareFormation.left);

        var isTopBottomPair = Bitwise.OnlyOppositePairCheck(_removeWallFormationValue, topBottomPairValue);
        if (isTopBottomPair)
            return Bitwise.GetOppoisteElement(_removeWallFormationValue, topBottomPairValue);
        else
            return Bitwise.GetOppoisteElement(_removeWallFormationValue, rightLeftPairValue);
    }

    private int GetLeftIndex()
    {
        int nextIndex = index - 1;
        if (nextIndex % width == (width - 1)) nextIndex = -1;
        return nextIndex;
    }
    private int GetBottomIndex()
    {
        int nextIndex = index - width;
        if (nextIndex < 0) nextIndex = -1;
        return nextIndex;
    }
    private int GetRightIndex()
    {
        int nextIndex = index + 1;
        if ((nextIndex % width) == 0) nextIndex = -1;
        return nextIndex;
    }
    private int GetTopIndex()
    {
        int nextIndex = index + width;
        if (nextIndex > numOfTotalTile) nextIndex = -1;
        return nextIndex;
    }
}
