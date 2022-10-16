using UnityEngine;

public class HandMotor : MonoBehaviour
{
    public float Speed = 2;

    private Vector2 _dir;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 direction)
    {
        _dir = direction;
    }

    private void Update()
    {
        //var coll = GetComponent<Collider>();
        var dir = new Vector3(_dir.x, 0, _dir.y);

        var maxDistance = Speed * Time.deltaTime;

        _rigidbody.velocity = dir * maxDistance;

        var target = Quaternion.Euler(_dir.y * 5, 0, _dir.x * -5);
        transform.rotation = Quaternion.Lerp(transform.rotation, target, 10 * Time.deltaTime);
    }
}