using UnityEngine;

public abstract class BaseWall : BaseMazeObject
{
    [SerializeField] private WallsDetector[] wallsDetectors;
    public void ActivateFunction(bool _isEnable) => gameObject.SetActive(_isEnable);

    internal void ResetPrillars()
    {
        for (int i = 0; i < wallsDetectors.Length; i++)
        {
            wallsDetectors[i].gameObject.SetActive(false);
        }
    }

    protected void SetPrillarsActive(bool isActive = false)
    {
        if (!isActive)
            return;

        for (int i = 0; i < wallsDetectors.Length; i++)
        {
            wallsDetectors[i].gameObject.SetActive(isActive);
        }
    }
}
