using UnityEngine;

public class MouseZoomHandler : BaseInputHandler, IMouseHandler
{
    private const float zoomScale = 5.0f;
    private const float zoomMin = 0.5f;
    private const float zoomMax = 16f;

    private Vector3 lastInputWorldPoint;

    private float MouseScroll => Input.GetAxis("Mouse ScrollWheel");

    public void HandleInput()
    {
        if (MouseScroll == 0)
        {
            lastInputWorldPoint = currCamera.ScreenToWorldPoint(Input.mousePosition);
            return;
        }

        currCamera.orthographicSize = Mathf.Clamp(currCamera.orthographicSize - MouseScroll * zoomScale, zoomMin, zoomMax);
        MoveCamera(lastInputWorldPoint, Input.mousePosition);
    }
}