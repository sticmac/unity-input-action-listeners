using System;
using UnityEngine.Events;

namespace Sticmac.InputActionListeners {
    [Serializable]
    public class <%= Type %>UnityEvent : UnityEvent<<%= TypeGeneric %>> {}

    public class <%= Type %>InputActionListener : ParametrizedInputActionListener<<%= TypeGeneric %>, FloatUnityEvent> {}
}