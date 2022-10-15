using System;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PhysicsMotor : MonoBehaviour
{
    public float RideHeight = .2f;

    [SerializeField] private LayerMask _groundMask = int.MaxValue;

    public float CastDistance = .5f;

    [ReadOnly] [SerializeField] private Rigidbody _rigidbody;

    [ReadOnly] [SerializeField] private CapsuleCollider _capsule;

    public Vector3? HitPoint { get; private set; }
    
    
    public float MaxSpeed = 100;

    private void OnValidate()
    {
        if (TryGetComponent(out _rigidbody) == false)
            Debug.LogError("No rigidbody attached", this);

        if (TryGetComponent(out _capsule) == false)
            Debug.LogError("No CapsuleCollider attached", this);

        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void OnDrawGizmosSelected()
    {
        if (_capsule == null)
            return;

        var localPos = transform.position;
        var bottomPoint = localPos + _capsule.center;

        Gizmos.DrawLine(bottomPoint, bottomPoint - (transform.up * CastDistance));
    }

    public float SpringForce;
    public float SpringDamper;

    private Vector3 _unitGoal;

    public void Jump()
    {
        _wantsJump = true;
        _stopJumpTimer = JumpInputBuffering;
    }

    public void SetInput(Vector3 input)
    {
        _unitGoal = input;
    }

    private Vector3 _goalVel;

    public float Acceleration = 10;
    public float MaxAccelForce = 500;

    [SerializeField] private AnimationCurve _velocityDotCurve;
    
    private bool _wantsJump;

    private float _stopJumpTimer;

    public float JumpInputBuffering = 0.1f;

    public float GroundedBuffering = 0.2f;

    private float _groundedBuffer;

    private void FixedUpdate()
    {
        if (_floatTimeout > 0)
        {
            _floatTimeout -= Time.fixedDeltaTime;
            _wantsJump = false;
        }

        if (_groundedBuffer > 0)
        {
            _groundedBuffer -= Time.fixedDeltaTime;
        }

        var wantedJump = _wantsJump;

        if (wantedJump)
        {
            if (_stopJumpTimer > 0)
                _stopJumpTimer -= Time.fixedDeltaTime;
            else
                _wantsJump = false;
        }

        var velocity = _rigidbody.velocity;

        var hitCollider = CheckGrounded(out var hit);

        if (hitCollider)
        {
            HitPoint = hit.point;
            GroundedMovement(hit, velocity);
        }
        else
        {
            HitPoint = null;
            AirMovement(velocity);
        }

        if (hitCollider && hit.distance < RideHeight)
        {
            _groundedBuffer = GroundedBuffering;
        }

        if (_groundedBuffer > 0)
        {
            if (wantedJump)
            {
                _wantsJump = false;
                _floatTimeout = JumpFloatTimeout;

                //if (velocity.y < 0)
                {
                    velocity.y = 0;
                    _rigidbody.velocity = velocity;
                }

                _rigidbody.AddForce(new Vector3(0, JumpForce, 0), ForceMode.VelocityChange);
            }
        }

        if (hitCollider)
        {
            KeepAboveGround(velocity, hit);
        }
    }

    private float _floatTimeout;

    [SerializeField] public float JumpForce = 500;

    public float JumpFloatTimeout = .5f;

    private bool CheckGrounded(out RaycastHit hit)
    {
        var localPos = transform.position;
        var down = transform.TransformDirection(Vector3.down);
        var bottomPoint = localPos + _capsule.center;

        var ray = new Ray()
        {
            origin = bottomPoint,
            direction = down
        };

        return Physics.Raycast(ray, out hit, CastDistance, _groundMask.value);
    }

    public float RepulseForce = 100;

    private void KeepAboveGround(Vector3 velocity, RaycastHit hit)
    {
        var down = transform.TransformDirection(Vector3.down);

        var otherVel = Vector3.zero;

        if (hit.rigidbody != null)
        {
            otherVel = hit.rigidbody.velocity;
        }

        var rayDirVel = Vector3.Dot(down, velocity);
        var otherRayDirVel = Vector3.Dot(down, otherVel);

        var relativeVelocity = rayDirVel - otherRayDirVel;

        var x = hit.distance - RideHeight;

        var springForce = (x * SpringForce) - (relativeVelocity * SpringDamper);

        var force = down * springForce;

        if (_floatTimeout < float.Epsilon)
        {
            _rigidbody.AddForce(force);

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForceAtPosition(_rigidbody.mass * new Vector3(0, -RepulseForce, 0), hit.point);
            }
        }
    }

    public float AirDownForce = 100;

    private void GroundedMovement(RaycastHit hit, Vector3 velocity)
    {
        var velDot = Vector3.Dot(_unitGoal, _goalVel.normalized);

        var accel = Acceleration * _velocityDotCurve.Evaluate(velDot);

        var goalVel = _unitGoal * MaxSpeed;

        _goalVel = Vector3.MoveTowards(_goalVel, goalVel, accel * Time.fixedDeltaTime);

        _goalVel.y = velocity.y;

        Vector3 neededAccel = (_goalVel - velocity) / Time.fixedDeltaTime;

        var maxAccel = MaxAccelForce;

        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);

        neededAccel = Vector3.ProjectOnPlane(neededAccel, hit.normal);

        _rigidbody.AddForce(neededAccel * _rigidbody.mass);
    }


    public float MaxAirSpeed = 5;
    public float AirAcceleration = 40;
    public float MaxAirAccel = 40;

    private void AirMovement(Vector3 velocity)
    {
        _goalVel = Vector3.MoveTowards(_goalVel, _unitGoal * MaxAirSpeed, AirAcceleration * Time.fixedDeltaTime);

        Vector3 neededAccel = (_goalVel - velocity) / Time.fixedDeltaTime;

        neededAccel = Vector3.ClampMagnitude(neededAccel, MaxAirAccel);

        var force = neededAccel * _rigidbody.mass;
        force.y = -AirDownForce;
        _rigidbody.AddForce(force);
    }
    

    private void OnDestroy()
    {
    }
}