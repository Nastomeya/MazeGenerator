using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Reference: <see href="https://www.youtube.com/watch?v=qEpmOqLHWcA"/> 
/// <br/>Reference: <see href="https://www.youtube.com/watch?v=K_aAnBn5khA"/>
/// <br/>Reference: <see href="https://www.youtube.com/watch?v=5LEVj3PLufE"/> </summary>
public class InputHandlerHub : MonoBehaviour
{
    internal event Action<InputState> InputModeNotifier = delegate { };
    [SerializeField] private Camera currCamera;
    private bool clickNotifyOnce = false, touchNotifyOnce = false, mouseInputActivity = false;
    private List<IMouseHandler> MouseHandlers;
    private List<ITouchHandler> TouchHandlers;

    private void Awake()
    {
        MouseHandlers = new(GetComponents<IMouseHandler>());
        TouchHandlers = new(GetComponents<ITouchHandler>());

        for (int i = 0; i < TouchHandlers.Count; i++)
        {
            TouchHandlers[i].Init(currCamera);
        }
        for (int i = 0; i < MouseHandlers.Count; i++)
        {
            MouseHandlers[i].Init(currCamera);
        }
    }

    private void LateUpdate()
    {
        if (DetectingTouchInput())
            return;
        else
            MouseClicksMode();
    }
    private bool DetectingTouchInput()
    {
        if (TouchHandlers == null)
            return false;

        if (Input.touchCount == 0)
            return false;

        if (!touchNotifyOnce)
        {
            InputModeNotifier?.Invoke(InputState.TouchPress);
            touchNotifyOnce = true;
            clickNotifyOnce = false;
        }

        if (!touchNotifyOnce)
            return false;

        for (int i = 0; i < TouchHandlers.Count; i++)
        {
            TouchHandlers[i].HandleInput();
        }
        return true;
    }
    private void MouseClicksMode()
    {
        if (MouseHandlers == null)
            return;

        mouseInputActivity = Input.GetAxis("Mouse ScrollWheel") != 0 || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2);

        if (mouseInputActivity && !clickNotifyOnce)
        {
            InputModeNotifier?.Invoke(InputState.MouseClick);
            clickNotifyOnce = true;
            touchNotifyOnce = false;
        }

        if (!clickNotifyOnce)
            return;

        for (int i = 0; i < MouseHandlers.Count; i++)
        {
            MouseHandlers[i].HandleInput();
        }
    }
}
