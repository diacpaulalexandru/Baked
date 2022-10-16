using System.Collections;
using System.Collections.Generic;
using ScriptableValues;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    [SerializeField] private ScriptableFloatValue scriptableFloatValue;

    public void SetVolumeLevel(float sliderValue)
    {
        scriptableFloatValue.Value = sliderValue;
    }
}
