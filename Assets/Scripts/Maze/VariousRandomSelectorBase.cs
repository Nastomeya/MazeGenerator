using System.Collections.Generic;
using UnityEngine;

public class VariousRandomSelectorBase : MonoBehaviour
{
    [SerializeField] protected Transform[] objects;
    [SerializeField] private ObjectsSelectorSetting setting;

    private List<int> objectIndexes;
    private void Awake() => InitIndexes();
    private void OnEnable()
    {
        ResetSelector();
        int currIndex = Utils.RandomRange(0, objectIndexes.Count);
        for (int i = 0; i < objects.Length; i++)
        {
            if (i == objectIndexes[currIndex])
            {
                objects[i].gameObject.SetActive(true);
                break;
            }
        }
    }
    private void OnDisable() => ResetSelector();

    protected void InitIndexes()
    {
        objectIndexes = new List<int>();
        //for (int i = 0; i < objects.Length; i++)
        //objectIndexes.Add(index);

        for (int i = 0; i < (objects.Length + setting.FrequentlyUsedTimes - 1); i++)
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
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].gameObject.activeSelf)
                objects[i].gameObject.SetActive(false);
        }
    }
}