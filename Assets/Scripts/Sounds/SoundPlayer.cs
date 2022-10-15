using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "SoundPlayer.asset", menuName = "Sounds/SoundPlayer")]
public class SoundPlayer : ScriptableObject
{
    [SerializeField] private SoundsContainer soundContainer;

    [SerializeField] private AudioPlayerScriptable soundScriptable;
    
    private List<AudioClip> _usedClips = new List<AudioClip>();

    public AudioPlayer PlaySoundInOrder()
    {
        if (soundContainer.soundsClip.Count == 0)
        {
            foreach (var clip in _usedClips)
                soundContainer.soundsClip.Add(clip);
            
            _usedClips.Clear();
        }

        var soundClip = soundContainer.soundsClip[0];
        var audioPlayer = soundScriptable.Play(soundClip);

        _usedClips.Add(soundClip);
        soundContainer.soundsClip.Remove(soundClip);

        return audioPlayer;
    }
    
    public AudioPlayer PlayRandomSound()
    {
        if (soundContainer.soundsClip.Count == 0)
        {
            foreach (var clip in _usedClips)
                soundContainer.soundsClip.Add(clip);

            _usedClips.Clear();
        }

        var randomNumber = 0;
        if (soundContainer.soundsClip.Count > 1)
            randomNumber = Random.Range(0, soundContainer.soundsClip.Count - 1);

        var soundClip = soundContainer.soundsClip[randomNumber];
        var audioPlayer = soundScriptable.Play(soundClip);

        _usedClips.Add(soundClip);
        soundContainer.soundsClip.Remove(soundClip);

        return audioPlayer;
    }
    
    private void OnDestroy()
    {
        if (_usedClips.Count == 0)
            return;

        foreach (var clip in _usedClips)
            soundContainer.soundsClip.Add(clip);
    }
}
