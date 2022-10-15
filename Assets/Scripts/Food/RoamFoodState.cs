using System;
using System.Linq;
using SimpleFSM;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoamFoodState : BaseState, IStateMachineUpdateListener
{
    private Transform[] _targets;

    public AIMovementBehaviour AIMovement;

    public BaseState NextState;

    private void Start()
    {
        _targets = GameObject.FindGameObjectsWithTag("Target").Select(o => o.transform).ToArray();
    }

    private Transform GetTarget()
    {
        var randomTransform = _targets[Random.Range(0, _targets.Length)];
        return randomTransform;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        var target = GetTarget();
        AIMovement.TransformToFollow = target;
    }


    public override void OnExit()
    {
        base.OnExit();
        AIMovement.TransformToFollow = null;
    }

    public void UpdateState()
    {
        if (AIMovement.ReachedDestination ||
            AIMovement.IsStuck)
            GoToState(NextState);
    }
}