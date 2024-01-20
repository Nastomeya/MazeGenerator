using DataExtensions;
using MathCalculation;
using System.Collections.Generic;
using UnityEngine;

public class HexagonNeighbours : BaseNeighbours
{
    private HexagonFormation wallFormation;
    internal HexagonNeighbours(int _index, int _width, int _numOfTotalTile, List<TileWithMazeObjectSets> _tiles) : base(_index, _width, _numOfTotalTile, _tiles)
    {
        wallFormation = new HexagonFormation();
        totalFormationValue = GetTotalFormationValue(wallFormation);
    }
    internal override int GetMazeFormation() => (int)wallFormation;
    internal override void SetMazeFormation(int _wallFormation) => wallFormation = (HexagonFormation)_wallFormation;

    internal override void InitFormation() => wallFormation = HexagonFormation.top;
    internal override int[] GetElements() => Bitwise.GetElements(wallFormation);
    internal override void Increment()
    {
        wallFormation = (HexagonFormation)((int)(wallFormation + 1) % totalFormationValue);
        if (wallFormation == 0)
            wallFormation = (HexagonFormation)totalFormationValue;
    }
    internal override int ByWhichNeighbour(Transform curr, Transform next)
    {
        var _index = -1;
        var direction = (curr.position - next.position).normalized;
        float degree = AdvanceMath.GetDegree(direction);

        if (degree == 0f)
            _index = 1;//top
        else if (degree == 60f)
            _index = 2;//topRight
        else if (degree == 120f)
            _index = 4;//bottomRight
        else if (degree == 180f)
            _index = 8;//bottom
        else if (degree == 240f)
            _index = 16;//bottomLeft
        else if (degree == 270f)
            _index = 32;//topLeft
        return _index;
    }
    internal override int GetNeighbourIndex(int _nextWallFormation)
    {
        int nextIndex = -1;

        if (_nextWallFormation == 1)//TOP
            nextIndex = GetTopIndex();
        else if (_nextWallFormation == 2)//topRIGHT
            nextIndex = GetTopRightIndex();
        else if (_nextWallFormation == 4)//botRIGHT
            nextIndex = GetBotRightIndex();
        else if (_nextWallFormation == 8)//BOTTOM
            nextIndex = GetBottomIndex();
        else if (_nextWallFormation == 16)//botLEFT
            nextIndex = GetBotLeftIndex();
        else if (_nextWallFormation == 32)//topLEFT
            nextIndex = GetTopLeftIndex();

        return nextIndex;
    }
    internal override int OverlappingObjectsReduction(int currWallFormation)
    {
        int topIndex = GetTopIndex();
        int topRightIndex = GetTopRightIndex();
        int botRightIndex = GetBotRightIndex();
        int bottomIndex = GetBottomIndex();
        int botLeftIndex = GetBotLeftIndex();
        int topLeftIndex = GetTopLeftIndex();

        if (topIndex >= 0 && tiles[topIndex].isVisited)
            currWallFormation = CheckFormationExistence(topIndex, currWallFormation, (int)HexagonFormation.top, (int)HexagonFormation.bottom);
        if (topRightIndex >= 0 && tiles[topRightIndex].isVisited)
            currWallFormation = CheckFormationExistence(topRightIndex, currWallFormation, (int)HexagonFormation.topRight, (int)HexagonFormation.botLeft);
        if (botRightIndex >= 0 && tiles[botRightIndex].isVisited)
            currWallFormation = CheckFormationExistence(botRightIndex, currWallFormation, (int)HexagonFormation.botRight, (int)HexagonFormation.topLeft);
        if (bottomIndex >= 0 && tiles[bottomIndex].isVisited)
            currWallFormation = CheckFormationExistence(bottomIndex, currWallFormation, (int)HexagonFormation.bottom, (int)HexagonFormation.top);
        if (botLeftIndex >= 0 && tiles[botLeftIndex].isVisited)
            currWallFormation = CheckFormationExistence(botLeftIndex, currWallFormation, (int)HexagonFormation.botLeft, (int)HexagonFormation.topRight);
        if (topLeftIndex >= 0 && tiles[topLeftIndex].isVisited)
            currWallFormation = CheckFormationExistence(topLeftIndex, currWallFormation, (int)HexagonFormation.topLeft, (int)HexagonFormation.botRight);

        return currWallFormation;
    }

    internal override int GetNextRemovalObjectValue(int _removeWallFormationValue)
    {
        int topBottomPairValue = (int)(HexagonFormation.top | HexagonFormation.bottom);
        int topLeftBotRightPairValue = (int)(HexagonFormation.topLeft | HexagonFormation.botRight);
        int topRightBotLeftPairValue = (int)(HexagonFormation.topRight | HexagonFormation.botLeft);

        var isTopBottomPair = Bitwise.OnlyOppositePairCheck(_removeWallFormationValue, topBottomPairValue);
        var isTopLeftBotRightPair = Bitwise.OnlyOppositePairCheck(_removeWallFormationValue, topLeftBotRightPairValue);

        if (isTopBottomPair)
            return Bitwise.GetOppoisteElement(_removeWallFormationValue, topBottomPairValue);
        else if (isTopLeftBotRightPair)
            return Bitwise.GetOppoisteElement(_removeWallFormationValue, topLeftBotRightPairValue);
        else
            return Bitwise.GetOppoisteElement(_removeWallFormationValue, topRightBotLeftPairValue);
    }
    private int GetTopLeftIndex()
    {
        int nextIndex;
        if (IsIndexAnOddNum())
        {
            nextIndex = index - 1;
            if (IsOutsideOfLeftBoundary(nextIndex))
                nextIndex = -1;
        }
        else
        {
            nextIndex = index + width - 1;
            if (IsOutsideOfLeftBoundary(nextIndex))
                nextIndex = -1;
            else
            {
                if (IsOutsideOfTopBoundary(nextIndex))
                    nextIndex = -1;
            }
        }
        return nextIndex;
    }
    private int GetTopRightIndex()
    {
        int nextIndex;
        if (IsIndexAnOddNum())//odd
        {
            nextIndex = index + 1;
            if (IsOutsideOfRightBoundary(nextIndex))
                nextIndex = -1;
        }
        else//even
        {
            nextIndex = index + width + 1;
            if (IsOutsideOfRightBoundary(nextIndex))
                nextIndex = -1;
            else
            {
                if (IsOutsideOfTopBoundary(nextIndex))
                    nextIndex = -1;
            }
        }
        return nextIndex;
    }
    private int GetBotLeftIndex()
    {
        int nextIndex;
        if (IsIndexAnOddNum())//odd
        {
            nextIndex = index - width - 1;
            if (nextIndex < 0)
                nextIndex = -1;
        }
        else
        {
            nextIndex = index - 1;
            if (IsOutsideOfLeftBoundary(nextIndex))
                nextIndex = -1;
        }
        return nextIndex;
    }
    private int GetBotRightIndex()
    {
        int nextIndex;
        if (IsIndexAnOddNum())//odd
            nextIndex = index - width + 1;
        else//even
            nextIndex = index + 1;

        if (IsOutsideOfRightBoundary(nextIndex))//if (nextIndex <= 0)
            nextIndex = -1;
        return nextIndex;
    }
    private int GetBottomIndex()
    {
        int nextIndex = index - width;
        if (nextIndex < 0) nextIndex = -1;
        return nextIndex;
    }
    private int GetTopIndex()
    {
        int nextIndex = index + width;
        if (IsOutsideOfTopBoundary(nextIndex)) nextIndex = -1;
        return nextIndex;
    }
    private bool IsIndexAnOddNum() => (index % width % 2) == 1;
    private bool IsOutsideOfRightBoundary(int _nextIndex) => (_nextIndex % width) <= 0;
    private bool IsOutsideOfLeftBoundary(int _nextIndex) => (_nextIndex % width) == (width - 1);
    private bool IsOutsideOfTopBoundary(int _nextIndex) => _nextIndex >= numOfTotalTile;
}