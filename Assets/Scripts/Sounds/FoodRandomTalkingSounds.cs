using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodRandomTalkingSounds : MonoBehaviour
{
    [SerializeField] private SoundPlayer _soundPlayer;
    private AudioPlayer _audioPlayer;

    private Stopwatch _stopWatch;
    private float _soundTimer = 3.0f;
    private float _initialFoodNumber;

    private void Start()
    {
        _initialFoodNumber = FindObjectsOfType<FoodSound>().Length;
        _stopWatch = Stopwatch.StartNew();
    }

    private void Update()
    {
        if (_stopWatch.Elapsed.TotalSeconds > _soundTimer)
        {
            _soundTimer += Random.Range(2, 3) + _initialFoodNumber / FindObjectsOfType<FoodSound>().Length;
            _soundPlayer.PlayRandomSound();
        }
    }
}