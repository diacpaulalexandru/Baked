using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundsContainer.asset", menuName = "Sounds/SoundsContainer")]
public class SoundsContainer: ScriptableObject
{
    public List<AudioClip> soundsClip;
}