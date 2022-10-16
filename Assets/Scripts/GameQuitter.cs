using UnityEngine;
using UnityEngine.InputSystem;

public class GameQuitter : MonoBehaviour
{
    private InputActions _actions;
    private InputActions.UIActions _uiActions;

    private void Awake()
    {
        _actions = new InputActions();
        _uiActions = _actions.UI;
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        _uiActions.Enable();

        _uiActions.QuitGame.Enable();
        _uiActions.QuitGame.performed += QuitGame;
    }

    private void OnDisable()
    {
        _uiActions.Disable();

        _uiActions.QuitGame.Disable();
        _uiActions.QuitGame.performed -= QuitGame;
    }

    private void QuitGame(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }
}