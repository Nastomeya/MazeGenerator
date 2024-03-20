using UnityEngine;

public class TouchSingleOrbitCameraHandler : BaseInputHandler, ITouchHandler
{
    private Vector3 target;
    private Vector2 lastTouchPosition, currentTouchPosition, deltaPosition;
    private Ray ray;
    [SerializeField] private float rotationSpeed = 20f;

    public void HandleInput()
    {
        if (Input.touchCount != 1)
            return;

        // Check for touch input
        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            currentTouchPosition = Input.GetTouch(0).position;
            deltaPosition = currentTouchPosition - lastTouchPosition;

            transform.RotateAround(target, Vector3.up, deltaPosition.x * rotationSpeed * Time.deltaTime);
            transform.RotateAround(target, transform.right, -deltaPosition.y * rotationSpeed * Time.deltaTime);

            // Update the last touch position
            lastTouchPosition = currentTouchPosition;
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Store the initial touch position
            lastTouchPosition = Input.GetTouch(0).position;

            ray = currCamera.ScreenPointToRay(lastTouchPosition);

            if (!Physics.Raycast(ray, out RaycastHit hit))
                return;
            // Check if the clicked object has a collider
            if (hit.collider == null)
                return;
            target = hit.point;
        }
    }
}