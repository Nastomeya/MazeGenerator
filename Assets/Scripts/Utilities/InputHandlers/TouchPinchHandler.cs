//using TMPro;
using UnityEngine;

/// <summary>This includes both pinch to zoom-in/out and touch panning feature</summary>
public class TouchPinchHandler : BaseTouchHandler, ITouchHandler
{
    //public TextMeshProUGUI touchInfoText;//remove this later

    private Vector2 lastCentreTouchPos, currCentreTouchPos;
    private Vector3 lastInputWorldPoint;
    private float currZoomAmount;
    private const float pinchSpeed = 0.9f;
    private const float zoomMin = 0.5f;
    private const float zoomMax = 16f;

    public void HandleInput()
    {
        if (Input.touchCount != 2)
            return;

        currTouchOne = Input.GetTouch(0);
        currTouchTwo = Input.GetTouch(1);

        if (currTouchOne.phase == TouchPhase.Began || currTouchTwo.phase == TouchPhase.Began)
        {
            lastTouch1Pos = currTouchOne.position;
            lastTouch2Pos = currTouchTwo.position;

            lastCentreTouchPos = GetPinchCentrePoint(lastTouch1Pos, lastTouch2Pos);
            lastInputWorldPoint = currCamera.ScreenToWorldPoint(lastCentreTouchPos);
        }
        else if (currTouchOne.phase == TouchPhase.Moved && currTouchTwo.phase == TouchPhase.Moved)
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
        var areOppositedDir = AreMovingInOppositeDirection(Input.GetTouch(0), Input.GetTouch(1));
        if (!areOppositedDir)
            return 0;

        var touchOneMoveAmount = currTouchOne.deltaPosition.magnitude;
        var touchTwoMoveAmount = currTouchTwo.deltaPosition.magnitude;
        var touchMoveAmount = touchOneMoveAmount > touchTwoMoveAmount ? touchOneMoveAmount : touchTwoMoveAmount;

        //Pinch Method No.1
        var inwardingIndex = GetPinchInwardingIndex(currTouchOne, currTouchTwo);
        return touchMoveAmount * inwardingIndex;

        //Pinch Method No.2
        //var isInwarding = IsPinchInwarding();
        //return touchMoveAmount * (isInwarding ? 1 : -1);
    }
}
