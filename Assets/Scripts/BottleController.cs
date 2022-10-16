using UnityEngine;

public class BottleController : MonoBehaviour
{
    [SerializeField]
    private GrabableObject _grabable;

    [SerializeField]
    private Rigidbody _rigidbody;

    public GameObject DefaultState;

    public GameObject GrabbedState;

    private void Start()
    {
        _grabable.Grabbed += Grabbed;
        _grabable.Dropped += Dropped;

        DefaultState.SetActive(true);
        GrabbedState.SetActive(false);
    }

    private void OnDestroy()
    {
        _grabable.Grabbed -= Grabbed;
        _grabable.Dropped -= Dropped;
    }

    private void Dropped()
    {
        _rigidbody.isKinematic = false;
        DefaultState.SetActive(true);
        GrabbedState.SetActive(false);
    }


    private void Grabbed()
    {
        _rigidbody.isKinematic = true;
        _rigidbody.velocity = Vector3.zero;

        DefaultState.SetActive(false);
        GrabbedState.SetActive(true);

       var gameController= FindObjectOfType<GameController>();
       gameController.TryProgress();
    }
}