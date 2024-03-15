using UnityEngine;

public class TouchOrbitCameraHandler : BaseInputHandler, ITouchHandler
{
    private Ray ray;
    private Vector3 target;
    private Touch lastMovedTouch;
    private readonly float rotationSpeed = 5f;
    private readonly float movementThreshold = 7.5f;

    //private Vector2 lastTouchPosition;
    //public void HandleInput()//works
    //{
    //    // Check for touch input
    //    if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
    //    {
    //        // Get the current touch position
    //        Vector2 currentTouchPosition = Input.GetTouch(0).position;

    //        // Calculate the delta position
    //        Vector2 deltaPosition = currentTouchPosition - lastTouchPosition;



    //        // Rotate the camera around the target based on touch input
    //        transform.RotateAround(target, Vector3.up, deltaPosition.x * rotationSpeed);
    //        transform.RotateAround(target, transform.right, -deltaPosition.y * rotationSpeed);

    //        // Update the last touch position
    //        lastTouchPosition = currentTouchPosition;
    //    }
    //    else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
    //    {
    //        // Store the initial touch position
    //        lastTouchPosition = Input.GetTouch(0).position;

    //        ray = currCamera.ScreenPointToRay(lastTouchPosition);

    //        if (!Physics.Raycast(ray, out RaycastHit hit))
    //            return;
    //        // Check if the clicked object has a collider
    //        if (hit.collider == null)
    //            return;
    //        target = hit.point;
    //    }
    //}

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
        float deltaMagnitude1 = touch1.deltaPosition.magnitude;
        float deltaMagnitude2 = touch2.deltaPosition.magnitude;

        bool touch1IsMoving = deltaMagnitude1 > movementThreshold && touch1.phase == TouchPhase.Moved;
        bool touch2IsMoving = deltaMagnitude2 > movementThreshold && touch2.phase == TouchPhase.Moved;

        if (touch1IsMoving && !touch2IsMoving)
            return (touch1, TouchPhase.Moved);
        else if (touch2IsMoving && !touch1IsMoving)
            return (touch2, TouchPhase.Moved);
        else
            return (null, TouchPhase.Stationary);
    }
}