using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using System.Collections;
using NUnit.Framework.Constraints;

namespace Sticmac.InputActionListeners {
    public class Vector3InputActionListenerTests : ParametrizedInputActionListenerTests<Vector3, Vector3InputActionListener.UnityEvent, Vector3InputActionListener>
    {
        protected override InputAction CreateSelectedAction() =>
            _actionMap.AddAction(SelectedActionName, binding: "<XRController>/devicePosition");

        public override void TriggerSelectedAction() => Set(_xrController.devicePosition, new Vector3(0.5f, 0.5f, 0.5f));
        public override void CancelSelectedAction() => Set(_xrController.devicePosition, Vector3.zero);
        public override IResolveConstraint IsValid() => Is.EqualTo(new Vector3(0.5f, 0.5f, 0.5f))
            .Using((Vector3 v1, Vector3 v2) => { // comparison through magnitude
                float distance = Vector3.Distance(v1, v2);
                if (distance <= 0.05f) {
                    return 0;
                } else {
                    return 1; // Comparison between vectors makes non sense, so we just put 1 to mark the difference
                }
            });
    }
}
