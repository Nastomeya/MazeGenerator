using UnityEngine;

public class Touch2FingersOrbitCameraHandler : BaseTouchHandler, ITouchHandler
{
    private Ray ray;
    private Vector3 target;
    private Touch lastMovedTouch;
    private readonly float rotationSpeed = 5f;

    protected override float MovementThreshold { get; } = 7.5f;
    public void HandleInput()
    {
        if (Input.touchCount != 2)
            return;

        var currTouchData = GetTouchData();
        if (currTouchData.Item1 == null)
            return;

        lastMovedTouch = (Touch)currTouchData.Item1;

        // Check for touch input
        if (currTouchData.Item2 == TouchPhase.Moved)
        {
            Vector2 deltaPosition = lastMovedTouch.deltaPosition;

            transform.RotateAround(target, Vector3.up, -deltaPosition.x * rotationSpeed * Time.deltaTime);
            transform.RotateAround(target, transform.right, deltaPosition.y * rotationSpeed * Time.deltaTime);
        }
        else if (currTouchData.Item2 == TouchPhase.Stationary)
        {
            ray = currCamera.ScreenPointToRay(lastMovedTouch.deltaPosition);

            if (!Physics.Raycast(ray, out RaycastHit hit))
                return;
            // Check if the clicked object has a collider
            if (hit.collider == null)
                return;

            target = hit.point;
        }
    }

    private (Touch?, TouchPhase) GetTouchData()
    {
        Touch touch1 = Input.GetTouch(0);
        Touch touch2 = Input.GetTouch(1);

        // Calculate the magnitudes of deltaPosition vectors for each touch
        var touch1State = GetHumanTouchPhase(touch1);
        var touch2State = GetHumanTouchPhase(touch2);

        bool touch1IsMoving = touch1State == TouchPhase.Moved;
        bool touch2IsMoving = touch2State == TouchPhase.Moved;

        if (touch1IsMoving && !touch2IsMoving)
            return (touch1, TouchPhase.Moved);
        else if (touch2IsMoving && !touch1IsMoving)
            return (touch2, TouchPhase.Moved);
        else
            return (null, TouchPhase.Stationary);
    }
}
