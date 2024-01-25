using UnityEngine;
public class StateRandomSelector : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var animationClips = animator.runtimeAnimatorController.animationClips;
        var randomIndex = Utils.RandomRange(0, animationClips.Length - 1);
        animator.SetTrigger(animationClips[randomIndex].name);
    }
}
