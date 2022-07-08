using UnityEngine;
using System;

public class RevealObjectController : MonoBehaviour
{
    public static RevealObjectController Instance = null;
    internal event Action<bool> OnDisplayOnOff = delegate { };
    private bool isOnOff = true;
    private void Awake() => Instance = this;
    public void ShowItOnOff()
    {
        if (Instance != null)
        {
            isOnOff = !isOnOff;
            OnDisplayOnOff?.Invoke(isOnOff);
        }
    }
}
