using MathCalculation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonCreatureSet : BaseMazeObject, IMazeObjects, IOverlapableFormation
{
    [SerializeField] private Transform mazeObjectPrefab;
    [SerializeField] private ObjectsSelectorSetting setting;
    private Dictionary<int, MazeObjectData> torchDatas = new();
    private List<int> avaliableIndexes;
    private MazeObjectData lastTorchData;
    private float radius = 0;

    private void Awake()
    {
        avaliableIndexes = new List<int>();

        AddNewTorchData(0, new MazeObjectData(270f, (int)HexagonFormation.top));
        AddNewTorchData(1, new MazeObjectData(330f, (int)HexagonFormation.topRight));
        AddNewTorchData(2, new MazeObjectData(30f, (int)HexagonFormation.botRight));
        AddNewTorchData(3, new MazeObjectData(90f, (int)HexagonFormation.bottom));
        AddNewTorchData(4, new MazeObjectData(150f, (int)HexagonFormation.botLeft));
        AddNewTorchData(5, new MazeObjectData(210f, (int)HexagonFormation.topLeft));

        radius = Vector3.Distance(mazeObjectPrefab.transform.localPosition, Vector3.zero);
    }
    private bool isEnable = true;
    public void SetEnble(bool _isEnable) => isEnable = _isEnable;
    public override IEnumerator ClearAllObjs()
    {
        lastTorchData = null;
        SetMazeFormation(0);
        yield return null;
    }
    public void SetMazeFormation(int _overlappingFormation)
    {
        if (_overlappingFormation < 1)
        {
            mazeObjectPrefab.transform.gameObject.SetActive(false);
            return;
        }

        if (!isEnable)
            return;
        avaliableIndexes.Clear();

        if (lastTorchData != null)
            mazeObjectPrefab.gameObject.SetActive(false);

        if (!setting.IsEnable())
            return;

        //only even id to show
        if (((int)HexagonFormation.top & _overlappingFormation) > 0) avaliableIndexes.Add(0);
        if (((int)HexagonFormation.topRight & _overlappingFormation) > 0) avaliableIndexes.Add(1);
        if (((int)HexagonFormation.botRight & _overlappingFormation) > 0) avaliableIndexes.Add(2);
        if (((int)HexagonFormation.bottom & _overlappingFormation) > 0) avaliableIndexes.Add(3);
        if (((int)HexagonFormation.botLeft & _overlappingFormation) > 0) avaliableIndexes.Add(4);
        if (((int)HexagonFormation.topLeft & _overlappingFormation) > 0) avaliableIndexes.Add(5);

        if (avaliableIndexes.Count < 1)
            return;

        if ((lastTorchData != null && (lastTorchData.Position & _overlappingFormation) == 0) || lastTorchData == null)
        {
            var randomIndex = Utils.RandomRange(0, avaliableIndexes.Count);
            var selectedIndex = avaliableIndexes[randomIndex];
            var newPosition = AdvanceMath.PointInCircumference(radius, torchDatas[selectedIndex].Angle, Vector2.zero);
            mazeObjectPrefab.transform.localPosition = new Vector3(newPosition.y, mazeObjectPrefab.localPosition.y, newPosition.x);
            mazeObjectPrefab.gameObject.SetActive(true);
            mazeObjectPrefab.transform.rotation = Quaternion.AngleAxis(torchDatas[selectedIndex].Angle, Vector3.up);
            lastTorchData = torchDatas[selectedIndex];
        }
    }
    private void AddNewTorchData(int _index, MazeObjectData _torchData) => torchDatas.Add(_index, _torchData);

}
