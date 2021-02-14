using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using System.Collections;
using NUnit.Framework.Constraints;

namespace Sticmac.InputActionListeners {
    public class FloatInputActionListenerTests : ParametrizedInputActionListenerTests<float, FloatInputActionListener.UnityEvent, FloatInputActionListener>
    {
        protected override InputAction CreateSelectedAction() =>
            _actionMap.AddAction(SelectedActionName, binding: "<Gamepad>/leftStick/x");

        public override void TriggerSelectedAction() => Set(_gamepad.leftStick, new Vector2(0.5f, 0.5f));
        public override void CancelSelectedAction() => Set(_gamepad.leftStick, Vector2.zero);
        public override IResolveConstraint IsValid() => Is.EqualTo(0.5f).Within(0.05f);
    }
}
