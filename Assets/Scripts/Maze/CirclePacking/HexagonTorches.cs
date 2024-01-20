using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonTorches : BaseMazeObject, IMazeObjects, IOverlapable
{
    [SerializeField] private Transform top, topRight, botRight, bottom, botLeft, topLeft;
    [SerializeField] private ObjectsSelectorSetting setting;

    private Dictionary<int, TorchData> torches = new();
    private List<int> avaliableIndexes;
    private TorchData lastTorch;
    private void Awake()
    {
        avaliableIndexes = new List<int>();
        lastTorch = new TorchData(null, 0);

        AddNewTorchData(0, new TorchData(top, (int)HexagonFormation.top));
        AddNewTorchData(1, new TorchData(topRight, (int)HexagonFormation.topRight));
        AddNewTorchData(2, new TorchData(botRight, (int)HexagonFormation.botRight));
        AddNewTorchData(3, new TorchData(bottom, (int)HexagonFormation.bottom));
        AddNewTorchData(4, new TorchData(botLeft, (int)HexagonFormation.botLeft));
        AddNewTorchData(5, new TorchData(topLeft, (int)HexagonFormation.topLeft));
    }
    private bool isEnable = true;
    public void SetEnble(bool _isEnable)
    {
        isEnable = _isEnable;
    }

    public override IEnumerator ClearAllObjs()
    {
        lastTorch = new TorchData(null, 0);
        SetMazeFormation(0);
        yield return null;
    }
    public void SetMazeFormation(int _overlappingFormation)
    {
        if (_overlappingFormation < 1)
        {
            for (int i = 0; i < torches.Count; i++)
            {
                torches[i].transform.gameObject.SetActive(false);
            }
            return;
        }

        if (!isEnable)
            return;

        avaliableIndexes.Clear();

        if (lastTorch.transform != null)
            lastTorch.transform.gameObject.SetActive(false);

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
        
        if ((lastTorch.transform != null && (lastTorch.position & _overlappingFormation) == 0) || lastTorch.transform == null)
        {
            var randomIndex = Utils.RandomRange(0, avaliableIndexes.Count);
            var selectedIndex = avaliableIndexes[randomIndex];

            torches[selectedIndex].transform.gameObject.SetActive(true);
            lastTorch = torches[selectedIndex];
        }
    }

    private void AddNewTorchData(int _index, TorchData _torchData) => torches.Add(_index, _torchData);
    public struct TorchData
    {
        public TorchData(Transform _transform, int _position)
        {
            transform = _transform;
            position = _position;
        }
        public Transform transform;
        public int position;
    }
}
