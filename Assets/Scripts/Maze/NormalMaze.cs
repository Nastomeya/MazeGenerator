using System.Collections;
using UnityEngine;

public class NormalMaze : BaseMaze
{
    //public override string GetFileNameAndPath()
    //{
    //    return "No Done Yet!!!!";
    //}

    protected override IEnumerator BacktrackingPath()
    {
        keepTracking = true;
        int nextIndex = -1;

        while (currentTile.Turns >= 0)
        {
            nextIndex = currentTile.GetNeighbourIndex(straightPathController);//STEP 1// return back to -1 if it gets nothing in return
            if (nextIndex >= 0)
            {
                if (!tiles[nextIndex].isVisited)
                {
                    stack.Push(currentTile);//STEP 2
                    var formationId = currentTile.UpdateRemovalObjectFormation(tiles[nextIndex]);//STEP 3
                    straightPathController.IdenticalRandomCheck(formationId);
                    currentTile = tiles[nextIndex];//STEP 4
                    break;
                }
                else
                    nextIndex = -1;
            }
            if (nextIndex < 0)
                currentTile.RemoveCurrObjectFromList();
        }

        keepTracking = false;
        yield return new WaitWhile(() => keepTracking);

        if (nextIndex >= 0)
        {
            SetVisited(currentTile);
            straightPathController.SetNextIndex(nextIndex);
            BacktrackPath();
        }
        else if (stack.Count > 0)
        {
            var nextTile = stack.Pop();
            currentTile.BacktrackRemovalObjFormation(nextTile);
            currentTile = nextTile;

            if (trackedTileNumber > totalTileNum)//Stop tracking if it is the last tile.
                yield break;
            BacktrackPath();
        }
    }

    protected override void CreateANewTile(ref int index, int y, int x)
    {
        Debug.LogWarning("No Done Yet!!!!");
    }
}
