using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;

namespace Sticmac.InputActionListeners {
    public class <%= Type %>InputActionListenerTests : ParametrizedInputActionListenerTests<<%= TypeGeneric %>, <%= Type %>UnityEvent, <%= Type %>InputActionListener>
    {
        private static string Action3Name = "action3";
        private InputAction _action3 = null;

        public override void Setup() {
            base.Setup();

            _action3.Enable();
        }

        protected override void InitializeActions() {
            base.InitializeActions();

            _action3 = _actionMap.AddAction(Action3Name, binding: "<%= Binding %>");
        }

        public override void TearDown() {
            base.TearDown();

            _action3.Disable();
        }

        [Test]
        public void TriggerSelectedActionShouldActivateStartedCallbackWithGoodValue() {
            _actionListener.SelectedActionName = Action3Name;

            float value = default(float);
            _actionListener.Started.AddListener((v) => value = v);

            // ACT

            // ASSERT
        }

        [Test]
        public void TriggerSelectedActionShouldActivatePerformedCallbackWithGoodValue() {
            _actionListener.SelectedActionName = Action3Name;

            float value = default(float);
            _actionListener.Performed.AddListener((v) => value = v);

            // ACT

            // ASSERT
        }
    }
}
