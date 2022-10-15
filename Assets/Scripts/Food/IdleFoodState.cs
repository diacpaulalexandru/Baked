using System.Collections;
using SimpleFSM;
using UnityEngine;

public class IdleFoodState : BaseState
{
    public float WaitMin = 3;
    public float WaitMax = 7;

    public BaseState NextState;

    public override void OnEnter()
    {
        base.OnEnter();
        StartCoroutine(WaitAndRoam());
    }

    public override void OnExit()
    {
        base.OnExit();
        StopAllCoroutines();
    }

    private IEnumerator WaitAndRoam()
    {
        yield return new WaitForSeconds(Random.Range(WaitMin, WaitMax));
        GoToState(NextState);
    }
}