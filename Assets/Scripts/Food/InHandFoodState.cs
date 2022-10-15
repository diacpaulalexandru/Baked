using SimpleFSM;
using UnityEngine;

public class InHandFoodState : BaseState
{
    public GrabableObject Grabable;

    public Rigidbody Rigidbody;

    private void Start()
    {
        Grabable.Grabbed += Grabbed;
        Grabable.Dropped += Dropped;
    }

    private void OnDestroy()
    {
        Grabable.Grabbed += Grabbed;
        Grabable.Dropped += Dropped;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Rigidbody.isKinematic = true;
        Rigidbody.velocity = Vector3.zero;
        foreach (var lookAtCamera in GetComponentsInChildren<LookAtCamera>())
        {
            lookAtCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
            lookAtCamera.enabled = false;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        Rigidbody.isKinematic = false;

        foreach (var lookAtCamera in GetComponentsInChildren<LookAtCamera>())
        {
            lookAtCamera.enabled = true;
        }
    }

    private void Dropped()
    {
        GoToState<IdleFoodState>();
    }

    private void Grabbed()
    {
        GoToState<InHandFoodState>();
    }
}