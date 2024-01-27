using UnityEngine;

public class SnakeHairStateController : LastWontPickNAlwaysFirstRandomSelector
{
    private float elapsedTime = 0f;
    private readonly float speed = 5f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        elapsedTime = 0;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(1, 1);
        base.OnStateExit(animator, stateInfo, layerIndex);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        elapsedTime += Time.deltaTime;
        // Calculate the t value for Mathf.PingPong using the current timer and duration
        float t = Mathf.PingPong(elapsedTime / stateInfo.length, 1f);

        // Adjust t to reverse the direction for the second half of the cycle
        if (elapsedTime > stateInfo.length / 2f)
            t = 1f - t;

        // Use t to interpolate between maxValue and minValue
        float interpolatedValue = Mathf.Lerp(1, 0, t * speed);
        animator.SetLayerWeight(1, interpolatedValue);
    }
}
