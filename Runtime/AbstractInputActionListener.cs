using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sticmac.InputHandler {
    public abstract class AbstractInputActionListener : MonoBehaviour {
        /// <summary>
        /// Player Input component for being plugged to the event system
        /// </summary>
        [SerializeField] protected PlayerInput _playerInput = null;

        [SerializeField] protected string _selectedActionName = null;

        protected virtual void OnEnable() {
            _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            _playerInput.onActionTriggered += HandleInput;
        }
        
        protected virtual void OnDisable() {
            _playerInput.onActionTriggered -= HandleInput;
        }

        protected virtual void Reset() {
            _playerInput = GetComponent<PlayerInput>();
        }

        /// <summary>
        /// Catches an emitted input from the player and triggers the corresponding callback if necessary
        /// </summary>
        /// <param name="context">The whole context of the player input</param>
        protected abstract void HandleInput(InputAction.CallbackContext context);
    }
}