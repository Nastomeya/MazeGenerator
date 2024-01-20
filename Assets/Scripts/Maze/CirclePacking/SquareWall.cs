using System.Collections;
using UnityEngine;
public class SquareWall : BaseWall, IMazeObjects
{
    [SerializeField] private Transform top, right, bottom, left;
    public override IEnumerator ClearAllObjs()
    {
        ResetPrillars();
        SetMazeFormation(0);
        yield return null;
    }
    private bool isEnable = true;
    public void SetEnble(bool _isEnable)
    {
        isEnable = _isEnable;
    }

    public void SetMazeFormation(int _objFormation)
    {
        if (!isEnable)
            return;

        top.gameObject.SetActive(((int)SquareFormation.top & _objFormation) > 0);
        right.gameObject.SetActive(((int)SquareFormation.right & _objFormation) > 0);
        bottom.gameObject.SetActive(((int)SquareFormation.bottom & _objFormation) > 0);
        left.gameObject.SetActive(((int)SquareFormation.left & _objFormation) > 0);
        SetPrillarsActive(_objFormation != 0);
    }
}