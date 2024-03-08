using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathCalculation;
public class HexagonTorch : BaseMazeObject, IMazeObjects, IOverlapableFormation
{
    [SerializeField] private Transform torchesPrefab;
    [SerializeField] private ObjectsSelectorSetting setting;

    private Dictionary<int, MazeObjectData> torchDatas = new();
    private List<int> avaliableIndexes;
    private MazeObjectData lastTorchData;
    private float radius = 0;

    private void Awake()
    {
        avaliableIndexes = new List<int>();

        AddNewTorchData(0, new MazeObjectData(0f, (int)HexagonFormation.top));
        AddNewTorchData(1, new MazeObjectData(60f, (int)HexagonFormation.topRight));
        AddNewTorchData(2, new MazeObjectData(120f, (int)HexagonFormation.botRight));
        AddNewTorchData(3, new MazeObjectData(180f, (int)HexagonFormation.bottom));
        AddNewTorchData(4, new MazeObjectData(240f, (int)HexagonFormation.botLeft));
        AddNewTorchData(5, new MazeObjectData(300f, (int)HexagonFormation.topLeft));

        radius = Vector3.Distance(torchesPrefab.transform.localPosition, Vector3.zero);
    }
    private bool isEnable = true;
    public void SetEnble(bool _isEnable)
    {
        isEnable = _isEnable;
        //torchesPrefab.transform.gameObject.SetActive(isEnable);
    }

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
            torchesPrefab.transform.gameObject.SetActive(false);
            return;
        }

        if (!isEnable)
            return;

        avaliableIndexes.Clear();

        if (lastTorchData != null)
            torchesPrefab.gameObject.SetActive(false);

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
            torchesPrefab.transform.localPosition = new Vector3(newPosition.y, torchesPrefab.localPosition.y, newPosition.x);
            torchesPrefab.gameObject.SetActive(true);
            torchesPrefab.transform.rotation = Quaternion.AngleAxis(torchDatas[selectedIndex].Angle, Vector3.up);
            lastTorchData = torchDatas[selectedIndex];
        }
    }

    private void AddNewTorchData(int _index, MazeObjectData _torchData) => torchDatas.Add(_index, _torchData);
}
