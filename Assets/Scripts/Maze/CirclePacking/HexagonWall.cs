using System.Collections;
using UnityEngine;

public class HexagonWall : WallBase, IMazeObjects
{
    public bool IsOverlappingMode { get { return false; } }

    [SerializeField] private Transform top, topRight, botRight, bottom, botLeft, topLeft;
    public override IEnumerator ClearAllObjs()
    {
        ResetPrillars();
        SetEnable();
        yield return null;

    }
    public void SetEnable(int _wallFormation = 0)
    {
        var isEnable = _wallFormation == 0 ? false : true;

        top.gameObject.SetActive(((int)HexagonFormation.top & _wallFormation) > 0);
        topRight.gameObject.SetActive(((int)HexagonFormation.topRight & _wallFormation) > 0);
        botRight.gameObject.SetActive(((int)HexagonFormation.botRight & _wallFormation) > 0);
        bottom.gameObject.SetActive(((int)HexagonFormation.bottom & _wallFormation) > 0);
        botLeft.gameObject.SetActive(((int)HexagonFormation.botLeft & _wallFormation) > 0);
        topLeft.gameObject.SetActive(((int)HexagonFormation.topLeft & _wallFormation) > 0);

        SetPrillarsActive(isEnable);
    }
}
