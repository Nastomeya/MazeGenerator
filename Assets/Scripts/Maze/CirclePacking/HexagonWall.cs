using System.Collections;
using UnityEngine;

public class HexagonWall : BaseWall, IMazeObjects
{
    [SerializeField] private Transform top, topRight, botRight, bottom, botLeft, topLeft;
    public override IEnumerator ClearAllObjs()
    {
        ResetPrillars();
        SetMazeFormation();
        yield return null;

    }
    private bool isEnable = true;
    public void SetEnble(bool _isEnable)
    {
        isEnable = _isEnable;
    }
    public void SetMazeFormation(int _wallFormation = 0)
    {
        if (!isEnable)
            return;

        top.gameObject.SetActive(((int)HexagonFormation.top & _wallFormation) > 0);
        topRight.gameObject.SetActive(((int)HexagonFormation.topRight & _wallFormation) > 0);
        botRight.gameObject.SetActive(((int)HexagonFormation.botRight & _wallFormation) > 0);
        bottom.gameObject.SetActive(((int)HexagonFormation.bottom & _wallFormation) > 0);
        botLeft.gameObject.SetActive(((int)HexagonFormation.botLeft & _wallFormation) > 0);
        topLeft.gameObject.SetActive(((int)HexagonFormation.topLeft & _wallFormation) > 0);

        SetPrillarsActive(_wallFormation != 0);
    }
}
