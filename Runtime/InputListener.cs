using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Sticmac.InputHandler {
    public class InputListener : MonoBehaviour
    {
        [SerializeField] PlayerInput _playerInput = null;

        [SerializeField] string _selectedActionName = null;

        public UnityEvent Started;
        public UnityEvent Performed;
        public UnityEvent Canceled;

        private void OnEnable() {
            _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            _playerInput.onActionTriggered += HandleInput;
        }
        
        private void OnDisable() {
            _playerInput.onActionTriggered -= HandleInput;
        }

        private void Reset() {
            _playerInput = GetComponent<PlayerInput>();
        }

        /// <summary>
        /// Catches an emitted input from the player and triggers the corresponding callback if necessary
        /// </summary>
        /// <param name="context">The whole context of the player input</param>
        private void HandleInput(InputAction.CallbackContext context) {
            if (_selectedActionName == context.action.name) {
                if (context.started) {
                    Started.Invoke();
                } else if (context.performed) {
                    Performed.Invoke();
                } else if (context.canceled) {
                    Canceled.Invoke();
                }
            }
        }
    }
}