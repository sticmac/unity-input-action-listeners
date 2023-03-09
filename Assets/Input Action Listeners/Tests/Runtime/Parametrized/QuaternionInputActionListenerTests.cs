using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using System.Collections;
using NUnit.Framework.Constraints;

namespace Sticmac.InputActionListeners {
    public class QuaternionInputActionListenerTests : ParametrizedInputActionListenerTests<Quaternion, QuaternionInputActionListener.UnityEvent, QuaternionInputActionListener>
    {
        public override void Setup() {
            base.Setup();

            _runCancelTests = false;
        }

        protected override InputAction CreateSelectedAction() =>
            _actionMap.AddAction(SelectedActionName, binding: "<XRController>/deviceRotation");

        public override void TriggerSelectedAction() => Set(_xrController.deviceRotation, Quaternion.Euler(15, 15, 15));
        public override void CancelSelectedAction() => Set(_xrController.deviceRotation, Quaternion.identity);
        public override IResolveConstraint IsValid() => Is.EqualTo(Quaternion.Euler(15, 15, 15));
    }
}
