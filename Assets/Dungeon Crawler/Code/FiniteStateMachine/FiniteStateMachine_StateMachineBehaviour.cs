using N_Awakening.DungeonCrawler;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FiniteStateMachine_StateMachineBehaviour : StateMachineBehaviour
{
    public States state;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<FiniteStateMachine>().SetState(state);
    }
}
