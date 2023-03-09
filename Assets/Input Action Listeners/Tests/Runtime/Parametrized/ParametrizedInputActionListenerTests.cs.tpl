using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using System.Collections;
using NUnit.Framework.Constraints;

namespace Sticmac.InputActionListeners {
    public class <%= Type %>InputActionListenerTests : ParametrizedInputActionListenerTests<<%= TypeGeneric %>, <%= Type %>InputActionListener.UnityEvent, <%= Type %>InputActionListener>
    {
        protected override InputAction CreateSelectedAction() =>
            _actionMap.AddAction(SelectedActionName, binding: "<%= Binding %>");

        public override void TriggerSelectedAction() => <%= Trigger %>;
        public override void CancelSelectedAction() => <%= Cancel %>;
        public override IResolveConstraint IsValid() => <%= Valid %>;
    }
}
