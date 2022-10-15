using System.Collections;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    public string GrabAnimName = "Grab";

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private GrabEventListener _grabEventListener;

    [SerializeField]
    private Transform _grabPoint;

    [SerializeField]
    private float _grabRadius;

    [SerializeField]
    private Transform _grabTransform;

    private enum GrabState
    {
        Waiting,
        Grabbing,
        Holding
    }

    private GrabableObject _hold;

    private void Start()
    {
        _grabEventListener.Grab += DoGrab;
    }

    private void OnDestroy()
    {
        _grabEventListener.Grab -= DoGrab;
    }

    private GrabableObject GetUnderHand()
    {
        var overlaps = Physics.OverlapSphere(_grabPoint.position, _grabRadius);

        foreach (var overlap in overlaps)
        {
            var grabableObject = overlap.GetComponent<GrabableObject>();
            if (grabableObject == null)
                continue;

            return grabableObject;
        }

        return null;
    }

    private void DoGrab()
    {
        if (_grab == false)
            return;

        var underHand = GetUnderHand();
        if (underHand == null)
            return;

        _hold = underHand;
        _hold.transform.parent = _grabTransform;
        _hold.transform.localPosition = Vector3.zero;
        _hold.PickUp();
    }

    private bool _grab;

    private Coroutine _grabRoutine;

    public void Grab(bool grab)
    {
        _grab = grab;

        if (_grab == false)
        {
            if (_hold != null)
            {
                _hold.Drop();
                _hold.transform.parent = null;
                _hold = null;
            }

            if (_grabRoutine != null)
            {
                StopCoroutine(_grabRoutine);
                _grabRoutine = null;
            }
        }
        else
        {
            if (_hold != null)
                return;
            if (_grabRoutine != null)
                return;

            _animator.SetTrigger(GrabAnimName);

            _grabRoutine = StartCoroutine(GrabRoutine());
        }
    }

    private IEnumerator GrabRoutine()
    {
        yield return new WaitForSeconds(1);
        _grabRoutine = null;
    }

    private void OnDrawGizmos()
    {
        if (_grabPoint == null)
            return;
        Gizmos.DrawWireSphere(_grabPoint.position, _grabRadius);
    }

    // public IEnumerator GrabRoutine()
    // {
    //     _animator.SetTrigger(GrabAnimName);
    // }
}