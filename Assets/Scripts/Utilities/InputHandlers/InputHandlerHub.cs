using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>Reference: <see href="https://www.youtube.com/watch?v=qEpmOqLHWcA"/> 
/// <br/>Reference: <see href="https://www.youtube.com/watch?v=K_aAnBn5khA"/>
/// <br/>Reference: <see href="https://www.youtube.com/watch?v=5LEVj3PLufE"/>/// </summary>
public class InputHandlerHub : MonoBehaviour
{
    public TextMeshProUGUI touchInfoText;//remove this later
    [SerializeField] private Camera currCamera;

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
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.touchCount > 0)
        {
            if (TouchHandlers == null)
                return;

            for (int i = 0; i < TouchHandlers.Count; i++)
            {
                TouchHandlers[i].HandleInput();
            }
        }
        else if (Input.mousePresent)
        {
            if (MouseHandlers == null)
                return;

            for (int i = 0; i < MouseHandlers.Count; i++)
            {
                MouseHandlers[i].HandleInput();
            }
        }
    }
}
