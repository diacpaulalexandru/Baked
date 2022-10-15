using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class AIMovementBehaviour : MonoBehaviour
{
    [SerializeField]
    private PhysicsMotor _motor;

    [SerializeField]
    private NavMeshObstacle _obstacle;

    [SerializeField]
    private float _forwardDist = 1;

    [SerializeField]
    private float _recalculateDistance = 0.5f;

    [SerializeField]
    private float _beginMovingDistance = 5f;

    [SerializeField]
    private float _keepDistance = 2f;

    [SerializeField]
    private float _timeToCountAsStuck = 1; //after this time passes we are counted as stuck

    private Vector3 _lastTarget;

    private NavMeshPath _path;

    private Vector3[] _allCorners = new Vector3[64];

    private int _cornersCount;

    private bool _reachedDestination = false;

    private Transform _transformToFollow;

    private Vector3 _myLastPosition;

    private float _notMovingTime;

    [ShowInInspector]
    [HideInEditorMode]
    public Transform TransformToFollow
    {
        get => _transformToFollow;
        set
        {
            if (_transformToFollow != value)
            {
                _reachedDestination = false;
                _transformToFollow = value;
            }

            if (_obstacle != null)
                _obstacle.enabled = _transformToFollow == null;

            if (_transformToFollow == null)
                _motor.SetInput(Vector3.zero);
        }
    }

    [ShowInInspector]
    [HideInEditorMode]
    public bool MoveToExactTarget { get; set; }

    public bool ReachedDestination => _reachedDestination;

    public bool IsStuck => _notMovingTime > _timeToCountAsStuck;

    private void Start()
    {
        _path = new NavMeshPath();
    }

    private void Update()
    {
        var hitPoint = _motor.HitPoint;

        if (TransformToFollow == null ||
            hitPoint.HasValue == false)
        {
            _path.ClearCorners();
            return;
        }

        var targetPosition = TransformToFollow.position;
        var localPosition = hitPoint.Value;

        _motor.SetInput(Vector2.zero);

        UpdateNotMovingTime(localPosition);

        var targetDistance = (_reachedDestination == false || MoveToExactTarget)
            ? _recalculateDistance
            : _beginMovingDistance;

        var targetDist = MoveToExactTarget == false ? _keepDistance : 0.1f;

        if (_path.status == NavMeshPathStatus.PathInvalid || IsStuck ||
            Vector3.Distance(targetPosition, _lastTarget) > targetDistance || IMoved(targetPosition, targetDist))
        {
            _lastTarget = targetPosition;

            if (NavMesh.CalculatePath(localPosition, _lastTarget, NavMesh.AllAreas, _path))
            {
                _reachedDestination = false;
                _cornersCount = _path.GetCornersNonAlloc(_allCorners);
            }
            else
            {
                if (NavMesh.SamplePosition(_lastTarget, out var hit, 10, NavMesh.AllAreas))
                {
                    if (NavMesh.CalculatePath(localPosition, hit.position, NavMesh.AllAreas, _path))
                    {
                        _reachedDestination = false;
                        _cornersCount = _path.GetCornersNonAlloc(_allCorners);
                    }
                    else
                    {
                        Debug.Log("This should not happen");
                    }
                }
                else
                {
                    _motor.SetInput(Vector3.zero);
                    return;
                }
            }
        }

        TryFollowPath(localPosition, targetDist);
    }

    private void TryFollowPath(Vector3 localPosition, float targetDist)
    {
        if (_path.status != NavMeshPathStatus.PathComplete &&
            _path.status != NavMeshPathStatus.PathPartial)
            return;

        var forward = LineUtils.GetForward(_allCorners, _cornersCount, localPosition, _forwardDist,
            out var remainingDistance);

        if (remainingDistance < targetDist)
        {
            _reachedDestination = true;
            return;
        }

        var direction = forward - localPosition;
        direction.y = 0;
        var clamp = Vector3.ClampMagnitude(direction, 1);

        _motor.SetInput(clamp);
    }

    private bool IMoved(Vector3 targetPosition, float targetDist)
    {
        return _reachedDestination &&
               Vector3.Distance(transform.position, targetPosition) > targetDist;
    }

    private void UpdateNotMovingTime(Vector3 localPosition)
    {
        if (_reachedDestination == false)
        {
            if (Vector3.SqrMagnitude(_myLastPosition - localPosition) < 0.1f)
            {
                _notMovingTime += Time.deltaTime;
            }
            else
            {
                _notMovingTime = 0;
                _myLastPosition = localPosition;
            }

            return;
        }

        _myLastPosition = localPosition;
        _notMovingTime = 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (_allCorners == null)
            return;

        for (var index = 0; index < _cornersCount - 1; index++)
        {
            var pathCorner = _allCorners[index];
            Gizmos.DrawLine(pathCorner, _allCorners[index + 1]);
        }
    }
}