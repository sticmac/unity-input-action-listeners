using System;
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
        #region C# Events
        /// <summary>
        /// Event to be called when the action has started. C# Action version
        /// </summary>
        public event Action<T> Started;
        /// <summary>
        /// Event to be called when the action has performed. C# Action version
        /// </summary>
        public event Action<T> Performed;
        /// <summary>
        /// Event to be called when the action has canceled. C# Action version
        /// </summary>
        public event Action<T> Canceled;
        #endregion

        #region Unity Events
        /// <summary>
        /// Event to be called when the action has started. Unity Event version
        /// </summary>
        public U StartedUnityEvent;
        /// <summary>
        /// Event to be called when the action has performed. Unity Event version
        /// </summary>
        public U PerformedUnityEvent;
        /// <summary>
        /// Event to be called when the action has canceled. Unity Event version
        /// </summary>
        public U CanceledUnityEvent;
        #endregion

        protected override void HandleInput(InputAction.CallbackContext context)
        {
            if (_selectedActionName == context.action.name) {
                var val = context.ReadValue<T>();
                switch (EventsActivationMode) {
                    case EventsMode.InvokeUnityEvents:
                        if (context.started) {
                            StartedUnityEvent.Invoke(val);
                        }
                        if (context.performed) {
                            PerformedUnityEvent.Invoke(val);
                        }
                        if (context.canceled) {
                            CanceledUnityEvent.Invoke(val);
                        }
                        break;
                    case EventsMode.InvokeCSharpEvents:
                        if (context.started) {
                            Started?.Invoke(val);
                        }
                        if (context.performed) {
                            Performed?.Invoke(val);
                        }
                        if (context.canceled) {
                            Canceled?.Invoke(val);
                        }
                        break;
                }
            }
        }

        public override void Initialize(PlayerInput playerInput)
        {
            base.Initialize(playerInput);

            if (StartedUnityEvent == null || PerformedUnityEvent == null || CanceledUnityEvent == null) {
                StartedUnityEvent = new U();
                PerformedUnityEvent = new U();
                CanceledUnityEvent = new U();
            }
        }
    }
}
