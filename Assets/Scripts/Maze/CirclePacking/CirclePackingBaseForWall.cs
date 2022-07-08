using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CirclePackingBaseForWall : WallBase
{
    [SerializeField] internal List<ObjectBase> objectPrefabs;
#if UNITY_EDITOR
    [SerializeField] private List<string> objectNames;//remove it later!
#endif

    [SerializeField] protected CirclePackingSetting setting;

    public bool InOverlappingMode { get { return false; } }
    internal Quaternion objectRotation;

    protected int currentNumOfObject, objectIndex, objectCount = 0;
    protected HashSet<ObjectBase> validObjects;

    protected virtual void Awake()
    {
        validObjects = new HashSet<ObjectBase>();
#if UNITY_EDITOR
        objectNames = new List<string>();
#endif
        objectRotation = Quaternion.identity;
    }
    internal HashSet<ObjectBase> GetPackingObjects() => validObjects;//remove it later!!
    public override IEnumerator ClearAllWalls()
    {
        if (validObjects != null)
        {
            var allValidObjects = validObjects.GetEnumerator();

            while (allValidObjects.MoveNext())
            {
                allValidObjects.Current.transform.SetPositionAndRotation(Vector3.zero.With(y: allValidObjects.Current.transform.position.y), Quaternion.identity);
                allValidObjects.Current.UnsubscribedToOnDisplayOnOff();
                allValidObjects.Current.ReturnToPool();
                allValidObjects.Current.gameObject.SetActive(false);
            }
            objectCount = 0;
            validObjects.Clear();
#if UNITY_EDITOR
            objectNames.Clear();
#endif
            yield return new WaitUntil(() => validObjects.Count == 0);
        }
    }
    protected abstract void SetNumberOfObject(int _wallFormation);
    protected abstract Vector3 GetRandomPosition(int _wallFormation);
    public virtual void SetWallsEnable(int _wallFormation)
    {
        if (validObjects != null)
        {
            var isEnable = _wallFormation == 0 ? false : true;

            var allValidObjects = validObjects.GetEnumerator();
            while (allValidObjects.MoveNext())
                allValidObjects.Current.gameObject.SetActive(isEnable);
        }
    }
    //if there are many kinds of them
    protected virtual void SetRandomObjectIndex() => objectIndex = (objectPrefabs.Count > 1) ? (Utils.RandomRange(0, objectPrefabs.Count)) : 0;//protected void SetRandomObjectIndex() => objectIndex = (objectPrefabs.Count > 1) ? Utils.RandomRange(0, objectPrefabs.Count - 1) : 0;
    protected void SetEachWallEnable(int _wallFormation)
    {
        if (gameObject.activeSelf)
            StartCoroutine(ClearAllWalls());

        SetNumberOfObject(_wallFormation);

        if (validObjects != null)
        {
            int i = 0;
            while (validObjects.Count < currentNumOfObject)
            {
                Vector3 dummyRandomPosition = GetRandomPosition(_wallFormation);
                float dummyRandomSize = setting.GetRandomSize();

                bool isOverLapping = OverLappingCheck(dummyRandomPosition, dummyRandomSize);
                if (!isOverLapping) ProduceObject(dummyRandomPosition, dummyRandomSize);

                if (OutOfLoopLimitedProtection(i, 250)) break;//5000
                i++;
            }
        }
    }
    private bool OverLappingCheck(Vector3 dummyRandomPosition, float dummyRandomSize)
    {
        bool isOverLapping = false;
        var allValidObjects = validObjects.GetEnumerator();
        int j = 0;
        while (allValidObjects.MoveNext())
        {
            SetRandomObjectIndex();
            dummyRandomPosition = dummyRandomPosition.With(y: objectPrefabs[objectIndex].transform.position.y);

            float distance = Vector3.Distance(dummyRandomPosition.With(y: 0), allValidObjects.Current.transform.position.With(y: 0));
            float minDistance = dummyRandomSize + allValidObjects.Current.GetSize();
            if (distance < minDistance)
            {
                isOverLapping = true;
                break;
            }

            if (OutOfLoopLimitedProtection(j, 125)) break;//2500
            j++;
        }

        return isOverLapping;
    }
    private void ProduceObject(Vector3 dummyRandomPosition, float dummyRandomSize)
    {
        var newObject = objectPrefabs[objectIndex].Get<ObjectBase>(dummyRandomPosition, objectRotation);
        newObject.SubscribedToOnDisplayOnOff();
        newObject.SetSize(dummyRandomSize);
        newObject.SetNumberOnObject(objectCount++);

        validObjects.Add(newObject);
#if UNITY_EDITOR
        objectNames.Add(newObject.name);//remove it later
#endif
    }
    private bool OutOfLoopLimitedProtection(int count, int limited)
    {
        bool isBreaked = false;
        if (count > limited)
        {
#if UNITY_EDITOR
            Debug.LogWarning("OUT OF ORDER!");
#endif
            isBreaked = true;
        }
        return isBreaked;
    }
}