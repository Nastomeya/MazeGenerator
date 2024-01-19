using System.Collections;
using UnityEngine;

public abstract class WallBase : MonoBehaviour
{
    [SerializeField] private WallsDetector[] wallsDetectors;
    public int id { get; private set; }
    public abstract IEnumerator ClearAllObjs();
    public void SetID(int _newID) => id = _newID;
    public void SetWallHolderEnable(bool _isEnable) => gameObject.SetActive(_isEnable);

    internal void ResetPrillars()
    {
        for (int i = 0; i < wallsDetectors.Length; i++)
        {
            wallsDetectors[i].gameObject.SetActive(false);
        }
    }

    protected void SetPrillarsActive(bool isActive)
    {
        if (!isActive)
            return;
        for (int i = 0; i < wallsDetectors.Length; i++)
        {
            wallsDetectors[i].gameObject.SetActive(true);
        }
    }
}