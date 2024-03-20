using MathCalculation;
using UnityEngine;

public abstract class BaseTouchHandler : BaseInputHandler
{
    protected float lastTouchesDistance, currTouchesDistance;
    protected Touch currTouchOne, currTouchTwo;
    protected Vector2 lastTouch1Pos, lastTouch2Pos;
    protected abstract float MovementThreshold { get; } //= 7.5f;

    protected TouchPhase GetHumanTouchPhase(Touch _targetTouch)
    {
        float deltaMagnitude = _targetTouch.deltaPosition.magnitude;
        bool touchIsMoving = deltaMagnitude > MovementThreshold && _targetTouch.phase == TouchPhase.Moved;
        if (touchIsMoving && _targetTouch.phase == TouchPhase.Moved)
            return TouchPhase.Moved;

        if (_targetTouch.phase == TouchPhase.Began)
            return TouchPhase.Began;
        return TouchPhase.Stationary;
    }
    protected Vector2 GetPinchCentrePoint(Vector2 _pointOne, Vector2 _pointTwo) => (_pointOne + _pointTwo) / 2f;

    protected float GetTouchSpeed(Touch _touch)
    {
        return _touch.deltaPosition.magnitude / _touch.deltaTime;
    }
    protected bool IsPinchCrossing()
    {
        var t1Direction = BasicMath.GetDirection(currTouchOne.position, lastTouch1Pos);
        var t2Direction = BasicMath.GetDirection(currTouchTwo.position, lastTouch2Pos);

        if (t1Direction == Vector2.zero || t2Direction == Vector2.zero)
            return false;

        if (Mathf.Sign(t1Direction.x) != Mathf.Sign(t2Direction.x) || Mathf.Sign(t1Direction.y) != Mathf.Sign(t2Direction.y))
            return true;
        return false;
    }
    protected bool IsPinchInwarding()//preform better in result
    {
        currTouchesDistance = (currTouchOne.position - currTouchTwo.position).magnitude;
        lastTouchesDistance = (lastTouch1Pos - lastTouch2Pos).magnitude;

        lastTouch1Pos = currTouchOne.position;
        lastTouch2Pos = currTouchTwo.position;

        return lastTouchesDistance > currTouchesDistance;
    }
    /// <summary> 'greater than 0' away, 'less than 0' toward, 'equals to 0' stay</summary>
    protected int GetPinchInwardingIndex(Touch _touchOne, Touch _touchTwo)
    {
        var displacement = _touchTwo.position - _touchOne.position;
        var velocity = _touchTwo.deltaPosition - _touchOne.deltaPosition;
        var movingDir = Vector2.Dot(velocity, displacement);

        var areOppositedDir = AreMovingInOppositeDirection(currTouchOne, currTouchTwo);
        if (areOppositedDir)
            return (int)Mathf.Sign(movingDir);
        else
            return 0;
    }

    private const float twoPointOppositeDirThreshold = 0.85f;
    protected bool AreMovingInOppositeDirection(Touch _touchOne, Touch _touchTwo)
    {
        // Update touch positions
        var touch1Position = _touchOne.position;
        var touch2Position = _touchTwo.position;

        // Calculate vectors representing motion of each touch
        Vector2 motion1 = _touchOne.deltaPosition / Time.deltaTime;
        Vector2 motion2 = _touchTwo.deltaPosition / Time.deltaTime;

        // Calculate vectors representing the relative positions of the touches
        Vector2 relativePosition1 = touch1Position - touch2Position;
        Vector2 relativePosition2 = touch2Position - touch1Position;

        var touch1Dir = Vector2.Dot(motion1.normalized, relativePosition1.normalized);
        var touch2Dir = Vector2.Dot(motion2.normalized, relativePosition2.normalized);

        return Mathf.Sign(touch1Dir) == Mathf.Sign(touch2Dir) && Mathf.Abs(touch1Dir) > twoPointOppositeDirThreshold && Mathf.Abs(touch2Dir) > twoPointOppositeDirThreshold;
    }
}
