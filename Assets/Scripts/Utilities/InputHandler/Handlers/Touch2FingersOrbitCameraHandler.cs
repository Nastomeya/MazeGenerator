using UnityEngine;
public class Touch2FingersOrbitCameraHandler : BaseTouchHandler, ITouchHandler
{
    private const float rotationSpeed = 14f;//Been adjusted 2024-03-22
    private Ray ray;
    private Vector3 target;
    private Touch currMovingTouch;

    public void HandleInput()
    {
        if (Input.touchCount != 2)
            return;
        var areOppositedDir = AreMovingInOppositeDirection(Input.GetTouch(0), Input.GetTouch(1));
        if (areOppositedDir)
            return;

        var currTouchData = GetTouchData();
        // Check for touch input
        if (currTouchData.Item2 == TouchPhase.Moved)
        {
            currMovingTouch = (Touch)currTouchData.Item1;
            Vector2 deltaPosition = currMovingTouch.deltaPosition;

            transform.RotateAround(target, Vector3.up, -deltaPosition.x * rotationSpeed * Time.deltaTime);
            transform.RotateAround(target, transform.right, deltaPosition.y * rotationSpeed * Time.deltaTime);
        }

        else if (currTouchData.Item2 == TouchPhase.Began)
        {
            ray = currCamera.ScreenPointToRay(lastStationaryTouch.position);

            if (!Physics.Raycast(ray, out RaycastHit hit))
                return;
            if (hit.collider == null)
                return;

            target = hit.point;
        }
    }
    private Touch lastStationaryTouch;
    private (Touch?, TouchPhase) GetTouchData()
    {
        currTouchOne = Input.GetTouch(0);
        currTouchTwo = Input.GetTouch(1);

        // Calculate the magnitudes of deltaPosition vectors for each touch
        var touch1State = GetHumanTouchPhase(currTouchOne);
        var touch2State = GetHumanTouchPhase(currTouchTwo);

        bool touch1IsMoving = touch1State == TouchPhase.Moved;
        bool touch2IsMoving = touch2State == TouchPhase.Moved;

        if (touch1IsMoving && !touch2IsMoving)
        {
            lastStationaryTouch = currTouchTwo;
            return (currTouchOne, TouchPhase.Moved);
        }
        else if (touch2IsMoving && !touch1IsMoving)
        {
            lastStationaryTouch = currTouchOne;
            return (currTouchTwo, TouchPhase.Moved);
        }

        else if (touch1State == TouchPhase.Began || touch2State == TouchPhase.Began)
        {
            if (touch1State == TouchPhase.Began && touch2State == TouchPhase.Stationary)
            {
                lastStationaryTouch = currTouchTwo;
                return (null, TouchPhase.Began);
            }
            else if (touch2State == TouchPhase.Began && touch1State == TouchPhase.Stationary)
            {
                lastStationaryTouch = currTouchOne;
                return (null, TouchPhase.Began);
            }
        }
        return (null, TouchPhase.Stationary);
    }
}
