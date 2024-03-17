using UnityEngine;

/// <summary>This includes both pinch to zoom-in/out and touch panning feature</summary>
public class TouchPinchHandler : BaseTouchHandler, ITouchHandler
{
    private Vector2 lastTouch1Pos, lastTouch2Pos, lastCentreTouchPos, currCentreTouchPos;
    private Vector3 lastInputWorldPoint;
    private Touch currTouchOne, currTouchTwo;
    private float currZoomAmount, lastTouchesDistance, currTouchesDistance;
    private const float pinchSpeed = 0.5f, zoomMin = 0.5f, zoomMax = 16f;
    protected override float MovementThreshold { get; } = 3.5f;

    public void HandleInput()
    {
        if (Input.touchCount != 2)
            return;

        currTouchOne = Input.GetTouch(0);
        currTouchTwo = Input.GetTouch(1);

        // Calculate the magnitudes of deltaPosition vectors for each touch
        var touch1Phase = GetHumanTouchPhase(currTouchOne);
        var touch2Phase = GetHumanTouchPhase(currTouchTwo);


        if (currTouchOne.phase == TouchPhase.Began || currTouchTwo.phase == TouchPhase.Began)
        {
            lastTouch1Pos = currTouchOne.position;
            lastTouch2Pos = currTouchTwo.position;

            lastCentreTouchPos = GetPinchCentrePoint(lastTouch1Pos, lastTouch2Pos);
            lastInputWorldPoint = currCamera.ScreenToWorldPoint(lastCentreTouchPos);
        }
        else if (touch1Phase == TouchPhase.Moved || touch2Phase == TouchPhase.Moved)
        {
            var currDeltaPinchAmount = GetDeltaPinchAmount();
            currZoomAmount = currDeltaPinchAmount * pinchSpeed * Time.deltaTime;

            currCamera.orthographicSize = Mathf.Clamp(currCamera.orthographicSize + currZoomAmount, zoomMin, zoomMax);

            currCentreTouchPos = GetPinchCentrePoint(currTouchOne.position, currTouchTwo.position);
            MoveCamera(lastInputWorldPoint, currCentreTouchPos);
        }
    }

    private float GetDeltaPinchAmount()
    {
        var touchOneMoveAmount = currTouchOne.deltaPosition.magnitude;
        var touchTwoMoveAmount = currTouchTwo.deltaPosition.magnitude;
        var touchMoveAmount = touchOneMoveAmount > touchTwoMoveAmount ? touchOneMoveAmount : touchTwoMoveAmount;
        var isInwarding = IsPinchInwarding();

        return touchMoveAmount * (isInwarding ? 1 : -1);
    }
    private bool IsPinchInwarding()
    {
        currTouchesDistance = (currTouchOne.position - currTouchTwo.position).magnitude;
        lastTouchesDistance = (lastTouch1Pos - lastTouch2Pos).magnitude;

        lastTouch1Pos = currTouchOne.position;
        lastTouch2Pos = currTouchTwo.position;

        return lastTouchesDistance > currTouchesDistance;
    }
}
