using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BlinkingEyes : MonoBehaviour
{
    [SerializeField] private GameObject _eyes;
    
    private Stopwatch _stopWatch;
    private float _blinkingTimer;

    private void Start()
    {
        _stopWatch = Stopwatch.StartNew();
        _blinkingTimer = Random.Range(3f, 5f);
    }

    private void Update()
    {
        if (_stopWatch.Elapsed.TotalSeconds > _blinkingTimer)
        {
            _stopWatch = Stopwatch.StartNew();
            _blinkingTimer = Random.Range(3f, 5f);
            Invoke(nameof(CloseEyes),0.5f);    
        }
    }

    private void CloseEyes()
    {
        _eyes.SetActive(true);
        Invoke(nameof(OpenEyes),0.25f);            
    }

    private void OpenEyes()
    {
        _eyes.SetActive(false);
    }
}
