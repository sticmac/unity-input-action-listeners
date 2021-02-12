using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using System.Collections;

namespace Sticmac.InputActionListeners {
    public class FloatInputActionListenerTests : ParametrizedInputActionListenerTests<float, FloatUnityEvent, FloatInputActionListener>
    {
        private static string Action3Name = "action3";
        private InputAction _action3 = null;

        public override void Setup() {
            base.Setup();

            _action3.Enable();
        }

        protected override void InitializeActions() {
            base.InitializeActions();

            _action3 = _actionMap.AddAction(Action3Name, binding: "<Gamepad>/leftStick/x");
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

            Set(_gamepad.leftStick, new Vector2(0.5f, 0.5f));

            Assert.That(value, Is.EqualTo(0.5f).Within(0.05f));
        }

        [Test]
        public void TriggerSelectedActionShouldActivatePerformedCallbackWithGoodValue() {
            _actionListener.SelectedActionName = Action3Name;

            float value = default(float);
            _actionListener.Performed.AddListener((v) => value = v);

            Set(_gamepad.leftStick, new Vector2(0.5f, 0.5f));

            Assert.That(value, Is.EqualTo(0.5f).Within(0.05f));
        }
    }
}
