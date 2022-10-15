using System.Linq;
using SimpleFSM;
using UnityEngine;

public class RunFoodState : BaseState, IStateMachineUpdateListener
{
    public float RunChance = 0.2f;


    public AIMovementBehaviour AIMovement;

    public BaseState NextState;

    private Transform[] _targets;

    private float _speed;

    private void Start()
    {
        _targets = GameObject.FindGameObjectsWithTag("Target").Select(o => o.transform).ToArray();
    }

    private Transform GetTarget()
    {
        var dist = float.MinValue;
        Transform tr = null;

        foreach (var target in _targets)
        {
            var di = Vector3.Distance(target.position, transform.position);
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
        var target = GetTarget();
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