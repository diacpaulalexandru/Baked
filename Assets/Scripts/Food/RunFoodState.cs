using System.Linq;
using SimpleFSM;
using UnityEngine;

public class RunFoodState : BaseState, IStateMachineUpdateListener
{
    public float RunChance = 0.2f;
    
    public AIMovementBehaviour AIMovement;

    public BaseState NextState;
    
    private float _speed;
    
    public static Transform GetTarget(Transform me)
    {
        var targets = GameObject.FindGameObjectsWithTag("Target").Select(o => o.transform).ToArray();
            
        var dist = float.MinValue;
        Transform tr = null;

        foreach (var target in targets)
        {
            var di = Vector3.Distance(target.position, me.position);
            if (di > dist)
            {
                dist = di;
                tr = target;
            }
        }

        return tr;
    }

    public void UpdateState()
    {
        if (AIMovement.ReachedDestination ||
            AIMovement.IsStuck)
            GoToState(NextState);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        var target = GetTarget(transform);
        var physicsMotor = GetComponent<PhysicsMotor>();
        _speed = physicsMotor.MaxSpeed;
        physicsMotor.MaxSpeed *= 2;
        AIMovement.TransformToFollow = target;
    }


    public override void OnExit()
    {
        base.OnExit();
        var physicsMotor = GetComponent<PhysicsMotor>();
        physicsMotor.MaxSpeed = _speed;
        AIMovement.TransformToFollow = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RunTrigger") == false)
            return;

        if (Random.value > RunChance)
            return;

        if (StateMachine.GameState is InHandFoodState)
            return;
        if (StateMachine.GameState == this)
            return;

        GoToState(this);
    }
}