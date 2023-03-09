using System;
using UnityEngine;
using UnityEngine.Events;

namespace Sticmac.InputActionListeners {
    public class FloatInputActionListener : ParametrizedInputActionListener<float, FloatInputActionListener.UnityEvent> {
        [Serializable]
        public class UnityEvent : UnityEvent<float> {}
    }
}