using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Sticmac.InputActionListeners {
    public class InputActionListener : AbstractInputActionListener
    {
        #region C# Events
        /// <summary>
        /// Event to be called when the action has started. C# Action version
        /// </summary>
        public event Action Started;
        /// <summary>
        /// Event to be called when the action has performed. C# Action version
        /// </summary>
        public event Action Performed;
        /// <summary>
        /// Event to be called when the action has canceled. C# Action version
        /// </summary>
        public event Action Canceled;
        #endregion

        #region Unity Events
        /// <summary>
        /// Event to be called when the action has started
        /// </summary>
        public UnityEvent StartedUnityEvent;
        /// <summary>
        /// Event to be called when the action has performed
        /// </summary>
        public UnityEvent PerformedUnityEvent;
        /// <summary>
        /// Event to be called when the action has canceled
        /// </summary>
        public UnityEvent CanceledUnityEvent;
        #endregion

        public override void Initialize(PlayerInput playerInput)
        {
            base.Initialize(playerInput);

            StartedUnityEvent = new UnityEvent();
            PerformedUnityEvent = new UnityEvent();
            CanceledUnityEvent = new UnityEvent();
        }

        /// <summary>
        /// Catches an emitted input from the player and triggers the corresponding callback if necessary
        /// </summary>
        /// <param name="context">The whole context of the player input</param>
        protected override void HandleInput(InputAction.CallbackContext context) {
            if (_selectedActionName == context.action.name) {
                switch (_eventsMode) {
                    case EventsMode.InvokeUnityEvents:
                        if (context.started) {
                            StartedUnityEvent.Invoke();
                        }
                        if (context.performed) {
                            PerformedUnityEvent.Invoke();
                        }
                        if (context.canceled) {
                            CanceledUnityEvent.Invoke();
                        }
                        break;

                    case EventsMode.InvokeCSharpEvents:
                        if (context.started) {
                            Started?.Invoke();
                        }
                        if (context.performed) {
                            Performed?.Invoke();
                        }
                        if (context.canceled) {
                            Canceled?.Invoke();
                        }
                        break;
                }
            }
        }
    }
}