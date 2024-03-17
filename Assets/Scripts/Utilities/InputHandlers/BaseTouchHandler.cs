using UnityEngine;

public abstract class BaseTouchHandler : BaseInputHandler
{
    protected abstract float MovementThreshold { get; } //= 7.5f;

    protected TouchPhase GetHumanTouchPhase(Touch _targetTouch)
    {
        float deltaMagnitude = _targetTouch.deltaPosition.magnitude;
        bool touch1IsMoving = deltaMagnitude > MovementThreshold && _targetTouch.phase == TouchPhase.Moved;
        if (touch1IsMoving && _targetTouch.phase == TouchPhase.Moved)
            return TouchPhase.Moved;
        else
            return TouchPhase.Stationary;
    }
    protected Vector2 GetPinchCentrePoint(Vector2 _pointOne, Vector2 _pointTwo) => (_pointOne + _pointTwo) / 2f;
}