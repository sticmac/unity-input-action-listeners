using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Sticmac.InputActionListeners {
    public abstract class ParametrizedInputActionListener<T, U> : AbstractInputActionListener
        where T : struct
        where U : UnityEvent<T>, new()
    {
        /// <summary>
        /// Event to be called when the action has started
        /// </summary>
        public U Started;
        /// <summary>
        /// Event to be called when the action has performed
        /// </summary>
        public U Performed;
        /// <summary>
        /// Event to be called when the action has canceled
        /// </summary>
        public U Canceled;

        protected override void HandleInput(InputAction.CallbackContext context)
        {
            if (_selectedActionName == context.action.name) {
                var val = context.ReadValue<T>();
                if (context.started) {
                    Started.Invoke(val);
                }
                if (context.performed) {
                    Performed.Invoke(val);
                }
                if (context.canceled) {
                    Canceled.Invoke(val);
                }
            }
        }

        public override void Initialize(PlayerInput playerInput)
        {
            base.Initialize(playerInput);

            Started = new U();
            Performed = new U();
            Canceled = new U();
        }
    }
}
