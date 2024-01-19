using System.Collections;
using UnityEngine;
public class SquareWall : WallBase, IMazeObjects
{
    public bool IsOverlappingMode { get { return false; } }
    [SerializeField] private Transform top, right, bottom, left;
    public override IEnumerator ClearAllObjs()
    {
        ResetPrillars();
        SetWallsEnable(0);
        yield return null;
    }
    public void SetWallsEnable(int _wallFormation)
    {
        var isEnable = _wallFormation == 0 ? false : true;

        top.gameObject.SetActive(((int)SquareFormation.top & _wallFormation) > 0);
        right.gameObject.SetActive(((int)SquareFormation.right & _wallFormation) > 0);
        bottom.gameObject.SetActive(((int)SquareFormation.bottom & _wallFormation) > 0);
        left.gameObject.SetActive(((int)SquareFormation.left & _wallFormation) > 0);
        SetPrillarsActive(isEnable);
    }
}