using UnityEngine;
public class StateRandomSelector : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var animationClips = animator.runtimeAnimatorController.animationClips;
        var randomNumber = Utils.RandomRange(0, 1500);
        var randomIndex = randomNumber % animationClips.Length;
        animator.SetTrigger(animationClips[randomIndex].name);
    }
}
