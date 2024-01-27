using UnityEngine;

public class LastOneWontPickRandomSelector : StateRandomSelector
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var animationClips = animator.runtimeAnimatorController.animationClips;
        var randomNumber = Utils.RandomRange(0, 1500);
        var randomIndex = randomNumber % (animationClips.Length - 1);
        animator.SetTrigger(animationClips[randomIndex].name);
    }
}
