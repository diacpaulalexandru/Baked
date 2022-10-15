using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KeepRigidBodyStraight : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public float UprightSpringStrength = 1500;

    public float UprightSpringDamper = 100;

    private void Awake()
    {
        TryGetComponent(out _rigidbody);
    }

    private void FixedUpdate()
    {
        KeepUpright();
    }

    private void KeepUpright()
    {
        var rot = transform.rotation;
        var quat = Quaternion.Euler(0, 0, 0);

        var target = MathUtils.ShortestRotation(quat, rot);

        target.ToAngleAxis(out var angle, out var axis);
        axis.Normalize();

        var radians = angle * Mathf.Deg2Rad;

        _rigidbody.AddTorque((axis * (radians * UprightSpringStrength)) -
                             (_rigidbody.angularVelocity * UprightSpringDamper));
    }
}