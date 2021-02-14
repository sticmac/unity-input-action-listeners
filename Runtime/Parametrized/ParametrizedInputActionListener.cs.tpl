using System;
using UnityEngine;
using UnityEngine.Events;

namespace Sticmac.InputActionListeners {
    public class <%= Type %>InputActionListener : ParametrizedInputActionListener<<%= TypeGeneric %>, <%= Type %>InputActionListener.UnityEvent> {
        [Serializable]
        public class UnityEvent : UnityEvent<<%= TypeGeneric %>> {}
    }
}