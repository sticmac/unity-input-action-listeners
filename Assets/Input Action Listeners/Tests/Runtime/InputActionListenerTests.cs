using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;

namespace Sticmac.InputActionListeners {
    public class InputActionListenerTests : InputTestFixture {
        private const string Action1Name = "action1";
        private const string Action2Name = "action2";

        private InputAction _action1 = null;
        private InputAction _action2 = null;

        private Keyboard _keyboard = null;
        private Gamepad _gamepad = null;

        private InputActionListener _actionListener;

        // Overrides InputTestFixture setup method
        public override void Setup() {
            base.Setup();
            // Input System config
            _keyboard = InputSystem.AddDevice<Keyboard>();
            _gamepad = InputSystem.AddDevice<Gamepad>();

            GameObject go = new GameObject();

            // Input Action Asset 
            InputActionAsset asset = ScriptableObject.CreateInstance<InputActionAsset>();
            InputActionMap actionMap = asset.AddActionMap("map");
            _action1 = actionMap.AddAction(Action1Name, binding: "<Keyboard>/space");
            _action2 = actionMap.AddAction(Action2Name, binding: "<Gamepad>/leftStick");

            _action1.Enable();
            _action2.Enable();

            // Player Input
            PlayerInput pi = go.AddComponent<PlayerInput>();
            pi.actions = asset;

            _actionListener = go.AddComponent<InputActionListener>();
            _actionListener.PlayerInput = pi;
        }

        public override void TearDown() {
            base.TearDown();
            _action1.Disable();
            _action2.Disable();
        }

        [Test]
        public void TriggerSelectedActionShouldActivateStartedUnityEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.StartedUnityEvent.AddListener(() => called = true);

            Press(_keyboard.spaceKey);

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerSelectedActionShouldActivateStartedCSharpEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Started += () => called = true;

            Press(_keyboard.spaceKey);

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerSelectedActionShouldNotActivateStartedUnityEventCallbackWhenWrongModeIsSelected() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.StartedUnityEvent.AddListener(() => called = true);

            Press(_keyboard.spaceKey);

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerSelectedActionShouldNotActivateStartedCSharpEventCallbackWhenWrongModeIsSelected() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Started += () => called = true;

            Press(_keyboard.spaceKey);

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAnotherActionShouldNotActivateStartedUnityEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.StartedUnityEvent.AddListener(() => called = true);

            Set(_gamepad.leftStick, new Vector2(0.123f, 0.234f));

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAnotherActionShouldNotActivateStartedCSharpEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Started += () => called = true;

            Set(_gamepad.leftStick, new Vector2(0.123f, 0.234f));

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerSelectedActionShouldActivatePerformedUnityEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.PerformedUnityEvent.AddListener(() => called = true);

            Press(_keyboard.spaceKey);

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerSelectedActionShouldActivatePerformedCSharpEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Performed += () => called = true;

            Press(_keyboard.spaceKey);

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerSelectedActionShouldNotActivatePerformedUnityEventCallbackWhenWrongModeIsSelected() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.PerformedUnityEvent.AddListener(() => called = true);

            Press(_keyboard.spaceKey);

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerSelectedActionShouldNotActivatePerformedCSharpEventCallbackWhenWrongModeIsSelected() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Performed += () => called = true;

            Press(_keyboard.spaceKey);

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAnotherActionShouldNotActivatePerformedUnityEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.PerformedUnityEvent.AddListener(() => called = true);

            Set(_gamepad.leftStick, new Vector2(0.123f, 0.234f));

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAnotherActionShouldNotActivatePerformedCSharpEventCallback() {
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Performed += () => called = true;

            Set(_gamepad.leftStick, new Vector2(0.123f, 0.234f));

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAndReleaseSelectedActionShouldActivateCanceledUnityEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.CanceledUnityEvent.AddListener(() => called = true);

            Press(_keyboard.spaceKey);
            Release(_keyboard.spaceKey);

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerAndReleaseSelectedActionShouldActivateCanceledCSharpEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Canceled += () => called = true;

            Press(_keyboard.spaceKey);
            Release(_keyboard.spaceKey);

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerAndReleaseSelectedActionShouldNotActivateCanceledUnityEventCallbackWhenWrongModeIsSelected() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.CanceledUnityEvent.AddListener(() => called = true);

            Press(_keyboard.spaceKey);
            Release(_keyboard.spaceKey);

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAndReleaseSelectedActionShouldNotActivateCanceledCSharpCallbackWhenWrongModeIsSelected() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Canceled += () => called = true;

            Press(_keyboard.spaceKey);
            Release(_keyboard.spaceKey);

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAndReleaseAnotherActionShouldNotActivateCanceledUnityEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.CanceledUnityEvent.AddListener(() => called = true);

            Set(_gamepad.leftStick, new Vector2(0.123f, 0.234f));
            Set(_gamepad.leftStick, new Vector2(0f, 0f));

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAndReleaseAnotherActionShouldNotActivateCanceledCSharpCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Canceled += () => called = true;

            Set(_gamepad.leftStick, new Vector2(0.123f, 0.234f));
            Set(_gamepad.leftStick, new Vector2(0f, 0f));

            Assert.That(called, Is.False);
        }
    } 
}