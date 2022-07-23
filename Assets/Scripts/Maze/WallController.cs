using UnityEngine;

public class WallController : MonoBehaviour
{
    internal PillarPositioner pillarPositioner;
    internal WallSizeAdjuster wallSizeAdjuster;

    private void Awake()
    {
        pillarPositioner = GetComponentInChildren<PillarPositioner>();
        wallSizeAdjuster = GetComponentInChildren<WallSizeAdjuster>();
    }
}
