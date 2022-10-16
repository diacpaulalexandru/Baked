using System.Linq;
using UnityEngine;

public class BlinkBehaviour : MonoBehaviour
{
    [SerializeField]
    private GrabableObject _grabable;

    [SerializeField] private SoundPlayer _soundPlayer;
    private AudioPlayer _audioPlayer;
    
    public float Chance = 0.4f;

    private void Start()
    {
        _grabable.CanPick += CanPick;
    }

    private void OnDestroy()
    {
        _grabable.CanPick -= CanPick;
    }

    private bool CanPick()
    {
        if (Random.value > Chance)
        {
            var target = RunFoodState.GetTarget(transform);
            var rgd = GetComponent<Rigidbody>();
            rgd.velocity = Vector3.zero;
            transform.position = target.position + new Vector3(0, 1, 0);
            _soundPlayer.PlayRandomSound();
            return false;
        }

        return true;
    }
}