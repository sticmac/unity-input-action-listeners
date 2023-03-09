using System;
using UnityEngine;
using UnityEngine.Events;

namespace Sticmac.InputActionListeners {
    public class Vector2InputActionListener : ParametrizedInputActionListener<Vector2, Vector2InputActionListener.UnityEvent> {
        [Serializable]
        public class UnityEvent : UnityEvent<Vector2> {}
    }
}