using System.Collections;
using System.Collections.Generic;
using SimpleFSM;
using Unity.Mathematics;
using UnityEngine;

public class ChopController : MonoBehaviour
{
    public float ChopDuration;
    public bool Chopping { get; set; }

    public ChopEventListener ChopEventListener;

    [SerializeField]
    private Animator _animator;

    public string ChopAnimName = "Chop";

    public Transform ChopOverlapPoint;

    public Vector3 ChopRotation;

    public Vector3 Extents = Vector3.one;

    private void Start()
    {
        ChopEventListener.Chop += OnChop;
    }

    private void OnDestroy()
    {
        ChopEventListener.Chop -= OnChop;
    }

    private void OnDrawGizmos()
    {
        if (ChopOverlapPoint == null)
            return;
        Gizmos.matrix = Matrix4x4.Rotate(quaternion.Euler(ChopRotation));
        Gizmos.DrawCube(ChopOverlapPoint.position, Extents);
    }
    
    private GrabableObject GetUnderKnife()
    {
        var overlaps = Physics.OverlapBox(ChopOverlapPoint.position, Extents, quaternion.Euler(ChopRotation));

        foreach (var overlap in overlaps)
        {
            var grabableObject = overlap.GetComponent<GrabableObject>();
            if (grabableObject == null)
                continue;

            return grabableObject;
        }

        return null;
    }
    
    private void OnChop()
    {
        var underKnife = GetUnderKnife();
        
        if(underKnife==null)
            return;
        
        if (underKnife.TryGetComponent(out StateMachine stateMachine) && 
            stateMachine.GameState is PrepareState prepareState)
        {
            prepareState.Cut();
        }
    }

    public void Chop()
    {
        if (Chopping)
            return;
        StartCoroutine(ChopCoroutine());
    }

    private IEnumerator ChopCoroutine()
    {
        Chopping = true;
        _animator.SetBool(ChopAnimName, true);
        yield return new WaitForSeconds(ChopDuration);
        _animator.SetBool(ChopAnimName, false);
        Chopping = false;
    }
}