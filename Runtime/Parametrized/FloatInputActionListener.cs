using System;
using UnityEngine.Events;

namespace Sticmac.InputActionListeners {
    [Serializable]
    public class FloatUnityEvent : UnityEvent<float> {}

    public class FloatInputActionListener : ParametrizedInputActionListener<float, FloatUnityEvent> {}
}