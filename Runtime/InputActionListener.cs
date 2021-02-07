using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Sticmac.InputHandler {
    public class InputActionListener : AbstractInputActionListener
    {
        /// <summary>
        /// Event to be called when the action started
        /// </summary>
        public UnityEvent Started;
        public UnityEvent Performed;
        public UnityEvent Canceled;

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