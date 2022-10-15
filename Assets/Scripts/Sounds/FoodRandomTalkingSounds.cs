using UnityEngine;
using Random = UnityEngine.Random;

public class FoodRandomTalkingSounds : MonoBehaviour
{
    [SerializeField] private Transform _foodCounter;
    [SerializeField] private SoundPlayer _soundPlayer;
    private AudioPlayer _audioPlayer;

    private float _soundTimer = 3.0f;
    private float _initialFoodNumber;

    private void Start()
    {
        _initialFoodNumber = _foodCounter.childCount;
    }

    private void Update()
    {
        if (Time.fixedTime > _soundTimer)
        {
            _soundTimer += Random.Range(2, 3) + _foodCounter.childCount / _initialFoodNumber;
            _soundPlayer.PlayRandomSound();
        }
    }
}