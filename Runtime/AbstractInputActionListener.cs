using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sticmac.InputActionListeners {
    public abstract class AbstractInputActionListener : MonoBehaviour {
        /// <summary>
        /// Player Input component for being plugged to the event system
        /// </summary>
        [SerializeField] protected PlayerInput _playerInput = null;
        public PlayerInput PlayerInput {
            get => _playerInput;
            set => Initialize(value);
        }

        /// <summary>
        /// Name of the selected action
        /// </summary>
        [SerializeField] protected string _selectedActionName = null;
        public string SelectedActionName { get => _selectedActionName; set => _selectedActionName = value; }

        [Serializable]
        public enum EventsMode {
            InvokeUnityEvents,
            InvokeCSharpEvents
        }
        [SerializeField] private EventsMode _eventsActivationMode;
        public EventsMode EventsActivationMode { get => _eventsActivationMode; set => _eventsActivationMode = value; }

        #region Initialization methods
        protected virtual void OnEnable() {
            // Launch init logic on enable
            if (_playerInput != null) { 
                Initialize(_playerInput);
            }
        }
        
        protected virtual void OnDisable() {
            // Unsubscribe on disable
            if (_playerInput != null) { 
                _playerInput.onActionTriggered -= HandleInput;
            }
        }

        protected virtual void Reset() {
            var playerInput = GetComponent<PlayerInput>();
            if (playerInput != null) {
                Initialize(playerInput);
            }
        }

        public virtual void Initialize(PlayerInput playerInput) {
            // The method doesn't accept a null playerInput
            if (playerInput == null) {
                throw new ArgumentNullException("playerInput");
            }

            _playerInput = playerInput;
            _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            _playerInput.onActionTriggered += HandleInput;
        }
        #endregion

        /// <summary>
        /// Catches an emitted input from the player and triggers the corresponding callback if necessary
        /// </summary>
        /// <param name="context">The whole context of the player input</param>
        protected abstract void HandleInput(InputAction.CallbackContext context);
    }
}