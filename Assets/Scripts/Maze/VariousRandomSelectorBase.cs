using System.Collections.Generic;
using UnityEngine;

public class VariousRandomSelectorBase : MonoBehaviour
{
    [SerializeField] protected List<IMazeElement> mazeElements;
    [SerializeField] private ObjectsSelectorSetting setting;

    private List<int> objectIndexes;
    private void Awake()
    {
        mazeElements = new List<IMazeElement>(GetComponentsInChildren<IMazeElement>(true));
        InitIndexes();
    }
    private void OnEnable()
    {
        ResetSelector();
        int currIndex = Utils.RandomRange(0, objectIndexes.Count);
        if (mazeElements == null)
            return;
        for (int i = 0; i < mazeElements.Count; i++)
        {
            if (i == objectIndexes[currIndex])
            {
                mazeElements[i].gameObject.SetActive(true);
                break;
            }
        }
    }
    private void OnDisable() => ResetSelector();

    protected void InitIndexes()
    {
        objectIndexes = new List<int>();

        for (int i = 0; i < (mazeElements.Count + setting.FrequentlyUsedTimes - 1); i++)
        {
            int halfTimes = Mathf.CeilToInt(setting.FrequentlyUsedTimes / 2);
            int index;
            if (i < halfTimes)
                index = 0;
            else if (i >= halfTimes && i <= setting.FrequentlyUsedTimes)
                index = 1;
            else
                index = i - setting.FrequentlyUsedTimes + 1;
            objectIndexes.Add(index);
        }
    }
    private void ResetSelector()
    {
        if (mazeElements == null)
            return;
        for (int i = 0; i < mazeElements.Count; i++)
        {
            if (mazeElements[i].gameObject.activeSelf)
                mazeElements[i].gameObject.SetActive(false);
        }
    }
}