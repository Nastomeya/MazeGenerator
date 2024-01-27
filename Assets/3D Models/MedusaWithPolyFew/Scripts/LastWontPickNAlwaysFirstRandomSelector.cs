using UnityEngine;

public class LastWontPickNAlwaysFirstRandomSelector : StateRandomSelector
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var animationClips = animator.runtimeAnimatorController.animationClips;
        var randomNumber = Utils.RandomRange(0, 1600);
        var randomIndex = (randomNumber < 1200) ? 0 : randomNumber % (animationClips.Length - 1);
        animator.SetTrigger(animationClips[randomIndex].name);
    }
}