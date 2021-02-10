﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Sticmac.InputActionListeners {
    public class InputActionListener : AbstractInputActionListener
    {
        /// <summary>
        /// Event to be called when the action has started
        /// </summary>
        public UnityEvent Started;
        /// <summary>
        /// Event to be called when the action has performed
        /// </summary>
        public UnityEvent Performed;
        /// <summary>
        /// Event to be called when the action has canceled
        /// </summary>
        public UnityEvent Canceled;

        public override void Initialize(PlayerInput playerInput)
        {
            base.Initialize(playerInput);

            Started = new UnityEvent();
            Performed = new UnityEvent();
            Canceled = new UnityEvent();
        }

        /// <summary>
        /// Catches an emitted input from the player and triggers the corresponding callback if necessary
        /// </summary>
        /// <param name="context">The whole context of the player input</param>
        protected override void HandleInput(InputAction.CallbackContext context) {
            if (_selectedActionName == context.action.name) {
                if (context.started) {
                    Started.Invoke();
                }
                if (context.performed) {
                    Performed.Invoke();
                }
                if (context.canceled) {
                    Canceled.Invoke();
                }
            }
        }
    }
}