using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class HandsInputController : MonoBehaviour
    {
        [SerializeField]
        private HandMotor _leftMotor;

        [SerializeField]
        private HandMotor _rightMotor;

        [SerializeField]
        private ChopController _chopController;

        [SerializeField]
        private GrabController _grabController;

        private InputActions _actions;

        private InputActions.PlayerActions _playerActions;

        private void Awake()
        {
            _actions = new InputActions();
            _playerActions = _actions.Player;
        }

        private void OnEnable()
        {
            _playerActions.Enable();

            _playerActions.LeftHandMove.Enable();
            _playerActions.LeftHandMove.performed += LeftPerformed;
            _playerActions.LeftHandMove.canceled += LeftCanceled;

            _playerActions.RightHandMove.Enable();
            _playerActions.RightHandMove.performed += RightPerformed;
            _playerActions.RightHandMove.canceled += RightCanceled;

            _playerActions.Chop.Enable();
            _playerActions.Chop.performed += Chop;

            _playerActions.Grab.Enable();
            _playerActions.Grab.performed += StartGrab;
            _playerActions.Grab.canceled += CancelGrab;
        }

        private void Chop(InputAction.CallbackContext obj)
        {
            _chopController.Chop();
        }

        private void StartGrab(InputAction.CallbackContext obj)
        {
            _grabController.Grab(true);
        }

        private void CancelGrab(InputAction.CallbackContext obj)
        {
            _grabController.Grab(false);
        }


        private void OnDisable()
        {
            _playerActions.LeftHandMove.Disable();
            _playerActions.LeftHandMove.performed -= LeftPerformed;

            _playerActions.RightHandMove.Disable();
            _playerActions.RightHandMove.performed -= RightPerformed;
        }

        private void LeftCanceled(InputAction.CallbackContext obj)
        {
            _leftMotor.Move(Vector2.zero);
        }

        private void RightCanceled(InputAction.CallbackContext obj)
        {
            _rightMotor.Move(Vector2.zero);
        }

        private void RightPerformed(InputAction.CallbackContext obj)
        {
            var value = obj.ReadValue<Vector2>();
            _rightMotor.Move(value.normalized);
        }

        private void LeftPerformed(InputAction.CallbackContext obj)
        {
            var value = obj.ReadValue<Vector2>();
            _leftMotor.Move(value.normalized);
        }
    }
}