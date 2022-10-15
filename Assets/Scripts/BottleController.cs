using UnityEngine;

public class BottleController : MonoBehaviour
{
    [SerializeField]
    private GrabableObject _grabable;

    [SerializeField]
    private Rigidbody _rigidbody;
    
    private void Start()
    {
        _grabable.Grabbed += Grabbed;
        _grabable.Dropped += Dropped;
    }

    private void OnDestroy()
    {
        _grabable.Grabbed -= Grabbed;
        _grabable.Dropped -= Dropped;
    }

    private void Dropped()
    {
        _rigidbody.isKinematic = false;
    }


    private void Grabbed()
    {
        _rigidbody.isKinematic = true;
    }
}