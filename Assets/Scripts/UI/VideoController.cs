using System.Diagnostics;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour
{
    [SerializeField] private GameObject _xButton;
    [SerializeField] private TextMeshProUGUI _skipIn;
    private string baseTextMessage = "Ad is skippable in...";
    private int secondsCounter;
    private int skippableIn = 5;
    private int nextSceneIndex = 2;
    private bool skipPressed;
    private Stopwatch _stopWatch;

    private InputActions _actions;

    private InputActions.UIActions _uiActions;
    
    private void Awake()
    {
        _actions = new InputActions();
        _uiActions = _actions.UI;
        _stopWatch = Stopwatch.StartNew();
    }

    private void OnEnable()
    {
        _uiActions.Enable();

        _uiActions.Submit.Enable();
        _uiActions.Submit.performed += StartToLoadScene;
    }

    private void OnDisable()
    {
        _uiActions.Disable();

        _uiActions.Submit.Disable();
        _uiActions.Submit.performed -= StartToLoadScene;
    }

    private void Update()
    {
        if (_stopWatch.Elapsed.TotalSeconds <= secondsCounter || skipPressed)
            return;
        
        secondsCounter++;
        if (secondsCounter <= 5)
        {
            _skipIn.text = $"{baseTextMessage} {skippableIn}";
            skippableIn -= 1;
        }
        else
        {
            _xButton.SetActive(true);
            _skipIn.gameObject.SetActive(false);
        }

        if (secondsCounter >= 20)
        {
            StartSceneSingleAsync();
        }
    }
    
    
    private void StartToLoadScene(InputAction.CallbackContext obj)
    {
        if (secondsCounter < 5)
            return;
        
        skipPressed = true;
        StartSceneSingleAsync();
    }
    
    private void StartSceneSingleAsync()
    {
        LoadSceneAsync().ConfigureAwait(true);
    }
    
    private async Task LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Single);
        while (asyncLoad.isDone == false)
            await Task.Yield();
    }
}
