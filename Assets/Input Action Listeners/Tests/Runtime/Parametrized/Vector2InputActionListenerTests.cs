using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using System.Collections;
using NUnit.Framework.Constraints;

namespace Sticmac.InputActionListeners {
    public class Vector2InputActionListenerTests : ParametrizedInputActionListenerTests<Vector2, Vector2InputActionListener.UnityEvent, Vector2InputActionListener>
    {
        protected override InputAction CreateSelectedAction() =>
            _actionMap.AddAction(SelectedActionName, binding: "<Gamepad>/leftStick");

        public override void TriggerSelectedAction() => Set(_gamepad.leftStick, new Vector2(0.5f, 0.5f));
        public override void CancelSelectedAction() => Set(_gamepad.leftStick, Vector2.zero);
        public override IResolveConstraint IsValid() => Is.EqualTo(new Vector2(0.5f, 0.5f))
            .Using((Vector2 v1, Vector2 v2) => { // comparison through magnitude
                float distance = Vector2.Distance(v1, v2);
                if (distance <= 0.05f) {
                    return 0;
                } else {
                    return 1; // Comparison between vectors makes non sense, so we just put 1 to mark the difference
                }
            });
    }
}
