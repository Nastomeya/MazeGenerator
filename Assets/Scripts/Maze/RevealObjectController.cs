using UnityEngine;
using System;

public class RevealObjectController : MonoBehaviour
{
    public static bool HasInstance { get; private set; } = false;
    public static RevealObjectController Instance = null;
    internal event Action<bool> OnTextDisplayOnOff = delegate { };
    internal event Action<bool> OnMarkDisplayOnOff = delegate { };
    internal bool isTextOnOff = true;
    internal bool isMarkOnOff = true;
    private void Awake()
    {
        Instance = this;
        HasInstance = true;
    }
    public void ShowTextOnOff()
    {
        if (Instance != null)
        {
            isTextOnOff = !isTextOnOff;
            OnTextDisplayOnOff?.Invoke(isTextOnOff);
        }
    }
    public void ShowMarkOnOff()
    {
        if (Instance != null)
        {
            isMarkOnOff = !isMarkOnOff;
            OnMarkDisplayOnOff?.Invoke(isMarkOnOff);
        }
    }
    public void OnDestroy()
    {
        HasInstance = false;
    }
}
