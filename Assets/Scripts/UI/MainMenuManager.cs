using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> mainMenuScreens;
    [SerializeField] private AudioPlayerScriptable soundManagerScriptable;
    [SerializeField] private AudioClip clickSound;
    private AudioPlayer mainMenuSoundSource;
    private int nextSceneIndex = 1;

    [SerializeField] private SoundPlayer _mainMusicPlayer;
    private AudioPlayer _audioPlayer;
    
    private void Start()
    {
        mainMenuSoundSource = _mainMusicPlayer.PlayRandomSound();
        mainMenuSoundSource.Source.loop = true;
    }

    public void SetScreen(int index)
    {
        for (int i = 0; i < mainMenuScreens.Count; i++)
        {
            if (i == index)
            {
                mainMenuScreens[i].SetActive(true);
                soundManagerScriptable.Play(clickSound);
                continue;
            }

            mainMenuScreens[i].SetActive(false);
            soundManagerScriptable.Play(clickSound);
        }
    }

    public void StartSceneSingleAsync()
    {
        LoadSceneAsync().ConfigureAwait(true);
    }

    private async Task LoadSceneAsync()
    {
        Destroy(this);
        
        soundManagerScriptable.Play(clickSound);
        Destroy(mainMenuSoundSource.gameObject);


        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Single);
        while (asyncLoad.isDone == false)
            await Task.Yield();
    }

    public void QuitGame()
    {
        soundManagerScriptable.Play(clickSound);
        Application.Quit();
    }
}