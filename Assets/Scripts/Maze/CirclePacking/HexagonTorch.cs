using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonTorch : WallBase, IMazeWall
{
    public bool InOverlappingMode { get { return true; } }
    [SerializeField] private Transform top, topRight, botRight, bottom, botLeft, topLeft;
    [SerializeField] private ObjectsSelectorSetting setting;

    private Dictionary<int, TorchData> torches;
    private List<int> avaliableIndexes;
    private TorchData lastTorch;
    private bool ToBeEnable = false;
    private void Awake()
    {
        torches = new Dictionary<int, TorchData>();
        avaliableIndexes = new List<int>();
        lastTorch = new TorchData(null, 0);
        ToBeEnable = setting.IsEnable();

        AddNewTorchData(0, new TorchData(top, (int)HexagonFormation.top));
        AddNewTorchData(1, new TorchData(topRight, (int)HexagonFormation.topRight));
        AddNewTorchData(2, new TorchData(botRight, (int)HexagonFormation.botRight));
        AddNewTorchData(3, new TorchData(bottom, (int)HexagonFormation.bottom));
        AddNewTorchData(4, new TorchData(botLeft, (int)HexagonFormation.botLeft));
        AddNewTorchData(5, new TorchData(topLeft, (int)HexagonFormation.topLeft));
    }
    public override IEnumerator ClearAllWalls()
    {
        lastTorch = new TorchData(null, 0);
        SetWallsEnable(0);
        yield return null;
    }
    public void SetWallsEnable(int _overlappingFormaption)
    {
        if (_overlappingFormaption < 1)
        {
            for (int i = 0; i < torches.Count; i++)
                torches[i].transform.gameObject.SetActive(false);
        }
        else
        {
            avaliableIndexes.Clear();

            if (ToBeEnable)//only even id to show
            {
                if (((int)HexagonFormation.top & _overlappingFormaption) > 0) avaliableIndexes.Add(0);
                if (((int)HexagonFormation.topRight & _overlappingFormaption) > 0) avaliableIndexes.Add(1);
                if (((int)HexagonFormation.botRight & _overlappingFormaption) > 0) avaliableIndexes.Add(2);
                if (((int)HexagonFormation.bottom & _overlappingFormaption) > 0) avaliableIndexes.Add(3);
                if (((int)HexagonFormation.botLeft & _overlappingFormaption) > 0) avaliableIndexes.Add(4);
                if (((int)HexagonFormation.topLeft & _overlappingFormaption) > 0) avaliableIndexes.Add(5);

                if (avaliableIndexes.Count > 0)
                {
                    if ((lastTorch.transform != null && (lastTorch.position & _overlappingFormaption) == 0) || lastTorch.transform == null)
                    {
                        var randomIndex = Utils.RandomRange(0, avaliableIndexes.Count);
                        var selectedIndex = avaliableIndexes[randomIndex];

                        if (lastTorch.transform != null)
                            lastTorch.transform.gameObject.SetActive(false);

                        torches[selectedIndex].transform.gameObject.SetActive(true);
                        lastTorch = torches[selectedIndex];
                        //Debug.Log(torches[selectedIndex].transform.name + " Start  overlappingFormaption: " + _overlappingFormaption + "   <color=orange> " + Bitwise.GetCombinationNames(new HexagonFormation(), _overlappingFormaption) + "</color>  " + torches[selectedIndex].position);
                    }
                }
            }
            //Debug.Log("--------------------------------------------------------------- lastTorch.position <color=red>End <b>^</b>   " + ToBeEnable + "</color>");
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