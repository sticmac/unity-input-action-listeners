using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Sticmac.InputActionListeners {
    public class QuaternionInputActionListener : ParametrizedInputActionListener<Quaternion, QuaternionInputActionListener.UnityEvent> {
        [Serializable]
        public class UnityEvent : UnityEvent<Quaternion> {}
    }
}