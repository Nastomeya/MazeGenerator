using UnityEngine;

public class MousePanHandler : BaseInputHandler, IMouseHandler
{
    private bool IsMiddleMouseBtnDown => Input.GetMouseButtonDown(2);
    private bool IsMiddleMouseButton => Input.GetMouseButton(2);
    private float MouseXAxis => Input.GetAxis("Mouse X");
    private float MouseYAxis => Input.GetAxis("Mouse Y");
    private Vector3 lastInputWorldPoint;

    public void HandleInput()
    {
        if (!IsMiddleMouseButton)
            return;

        if (IsMiddleMouseBtnDown) //Check for Pan
            lastInputWorldPoint = currCamera.ScreenToWorldPoint(Input.mousePosition);

        if (MouseYAxis != 0 || MouseXAxis != 0)
            MoveCamera(lastInputWorldPoint, Input.mousePosition);
    }
}
