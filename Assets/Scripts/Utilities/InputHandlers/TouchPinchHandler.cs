using UnityEngine;
/// <summary>This includes both pinch to zoom-in/out and touch panning feature</summary>
public class TouchPinchHandler : BaseInputHandler, ITouchHandler
{
    private Vector2 lastTouch1Pos, lastTouch2Pos, lastCentreTouchPos, currCentreTouchPos;
    private Vector3 lastInputWorldPoint;
    private Touch touchOne, touchTwo;
    private float zoomAmount, pinchAmount, newOrthographicSize, lastTouchesDistance, currentTouchesDistance;
    private const float pinchSpeed = 0.1f;
    private const float zoomMin = 0.5f;
    private const float zoomMax = 16f;

    public void HandleInput()
    {
        if (Input.touchCount != 2)
            return;

        touchOne = Input.GetTouch(0);
        touchTwo = Input.GetTouch(1);

        if ((touchOne.phase == TouchPhase.Stationary && touchTwo.phase == TouchPhase.Stationary) || touchOne.phase == TouchPhase.Began || touchTwo.phase == TouchPhase.Began)
        {
            lastTouch1Pos = touchOne.position;
            lastTouch2Pos = touchTwo.position;
            lastCentreTouchPos = GetPinchCentrePoint(lastTouch1Pos, lastTouch2Pos);
            lastInputWorldPoint = currCamera.ScreenToWorldPoint(lastCentreTouchPos);
        }
        else if (touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved)
        {
            lastTouchesDistance = Vector2.Distance(lastTouch1Pos, lastTouch2Pos);
            currentTouchesDistance = Vector2.Distance(touchOne.position, touchTwo.position);
            pinchAmount = lastTouchesDistance - currentTouchesDistance;

            zoomAmount = pinchAmount * pinchSpeed;
            newOrthographicSize = currCamera.orthographicSize + zoomAmount;
            newOrthographicSize = Mathf.Clamp(newOrthographicSize, zoomMin, zoomMax);
            currCamera.orthographicSize = Mathf.Lerp(currCamera.orthographicSize, newOrthographicSize, Time.deltaTime);

            currCentreTouchPos = GetPinchCentrePoint(touchOne.position, touchTwo.position);
            MoveCamera(lastInputWorldPoint, currCentreTouchPos);
        }
    }

    private Vector2 GetPinchCentrePoint(Vector2 _pointOne, Vector2 _pointTwo) => (_pointOne + _pointTwo) / 2f;
}
