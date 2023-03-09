using System;
using UnityEngine;
using UnityEngine.Events;

namespace Sticmac.InputActionListeners {
    public class Vector3InputActionListener : ParametrizedInputActionListener<Vector3, Vector3InputActionListener.UnityEvent> {
        [Serializable]
        public class UnityEvent : UnityEvent<Vector3> {}
    }
}