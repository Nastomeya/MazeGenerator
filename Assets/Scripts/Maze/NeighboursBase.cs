using DataExtensions;
using System.Collections.Generic;
using UnityEngine;

public abstract class NeighboursBase
{
    internal int totalFormationValue;

    protected readonly int index, width, numOfTotalTile;
    protected readonly List<TileWithWalls> tiles;
    protected List<int> avaliableWalls;
    protected int wallIndex;

    public NeighboursBase(int _index, int _width, int _numOfTotalTile, List<TileWithWalls> _tiles)
    {
        index = _index;
        width = _width;
        tiles = _tiles;
        numOfTotalTile = _numOfTotalTile;
    }
    internal abstract void InitFormation();
    internal abstract void WallIncrement();
    internal abstract int ByWhichNeighbour(Transform curr, Transform next);
    internal abstract int GetNeighbourIndex(int _nextWallFormation);
    internal abstract int OverlappingWallsReduction(int currWallFormation);
    internal abstract int GetNextRemovalWallValue(int _removeWallFormationValue);
    internal abstract int GetWallFormation();
    internal abstract void SetWallFormation(int _wallFormation);
    internal abstract int[] GetElements();
    protected int GetTotalFormationValue<T>(T _wallFormation) => Bitwise.GetSumOfEnumValues(_wallFormation);
    internal int GetAvaliableWalls() => avaliableWalls.Count - 1;
    internal void RemoveCurrWall() => avaliableWalls.RemoveAt(wallIndex);
    internal int GetRandomWall(StraightPathController backTrackingData)
    {
        int turns = avaliableWalls.Count - 1;
        wallIndex = Utils.RandomRange(0, turns);
        var currNeighbour = avaliableWalls[wallIndex];

        while (backTrackingData.IsOutOfStraightPathRule(currNeighbour, Utils.RandomRange(1, 4)) && avaliableWalls.Count > 1)
        {
            wallIndex = (wallIndex + 1) % (turns + 1);
            currNeighbour = avaliableWalls[wallIndex];
        }
        return currNeighbour;
    }
    protected int CheckFormationExistence(int nextIndex, int _currWallFormation, int removalWallFormation, int nextWallFormation)
    {
        var neighbourHadIt = Bitwise.HasOneOfTheElement(tiles[nextIndex].currWallFormation, nextWallFormation);

        if (neighbourHadIt)
            _currWallFormation = Bitwise.Remove(_currWallFormation, removalWallFormation);

        return _currWallFormation;
    }
    internal void Reset()
    {
        avaliableWalls = DataRenewer.Renew(avaliableWalls);

        var allEnums = GetElements();
        for (int i = 0; i < allEnums.Length; i++)
            avaliableWalls.Add(allEnums[i]);
    }
}
