using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

namespace Sticmac.InputActionListeners {
    public abstract class ParametrizedInputActionListenerTests<T, U, V> : InputTestFixture
        where T : struct
        where U : UnityEvent<T>, new()
        where V : ParametrizedInputActionListener<T, U>
    {
        protected const string SelectedActionName = "selectedAction";
        protected const string OtherActionName = "otherAction";

        protected InputActionMap _actionMap = null;

        protected InputAction _selectedAction = null;
        protected InputAction _otherAction = null;

        protected bool _runCancelTests = true;

        protected Keyboard _keyboard = null;
        protected Gamepad _gamepad = null;
        protected XRController _xrController = null;

        protected ParametrizedInputActionListener<T, U> _actionListener;

        // Overrides InputTestFixture setup method
        public override void Setup() {
            base.Setup();
            // Input System config
            _keyboard = InputSystem.AddDevice<Keyboard>();
            _gamepad = InputSystem.AddDevice<Gamepad>();
            _xrController = InputSystem.AddDevice<XRController>();

            GameObject go = new GameObject();

            // Input Action Asset 
            InputActionAsset asset = ScriptableObject.CreateInstance<InputActionAsset>();
            _actionMap = asset.AddActionMap("map");

            _selectedAction = CreateSelectedAction();
            _otherAction = _actionMap.AddAction(OtherActionName, binding: "<Keyboard>/spaceKey");

            _selectedAction.Enable();
            _otherAction.Enable();

            // Player Input
            PlayerInput pi = go.AddComponent<PlayerInput>();
            pi.actions = asset;

            _actionListener = go.AddComponent<V>();
            _actionListener.PlayerInput = pi;
        }
        
        protected abstract InputAction CreateSelectedAction();

        public override void TearDown() {
            base.TearDown();
            _selectedAction.Disable();
            _otherAction.Disable();
        }

        public abstract void TriggerSelectedAction();
        public abstract void CancelSelectedAction();

        [Test]
        public void TriggerSelectedActionShouldActivateStartedUnityEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = SelectedActionName;

            bool called = false;
            _actionListener.StartedUnityEvent.AddListener((v) => called = true);

            TriggerSelectedAction();

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerSelectedActionShouldActivateStartedCSharpEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = SelectedActionName;

            bool called = false;
            _actionListener.Started += (v) => called = true;

            TriggerSelectedAction();

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerSelectedActionShouldNotActivateStartedUnityEventCallbackWhenWrongModeIsSelected() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = SelectedActionName;

            bool called = false;
            _actionListener.StartedUnityEvent.AddListener((v) => called = true);

            TriggerSelectedAction();

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerSelectedActionNotShouldActivateStartedCSharpEventCallbackWhenWrongModeIsSelected() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = SelectedActionName;

            bool called = false;
            _actionListener.Started += (v) => called = true;

            TriggerSelectedAction();

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerSelectedActionShouldActivatePerformedUnityEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = SelectedActionName;

            bool called = false;
            _actionListener.PerformedUnityEvent.AddListener((v) => called = true);

            TriggerSelectedAction();

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerSelectedActionShouldActivatePerformedCSharpEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = SelectedActionName;

            bool called = false;
            _actionListener.Performed += (v) => called = true;

            TriggerSelectedAction();

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerSelectedActionShouldNotActivatePerformedUnityEventCallbackWhenWrongModeIsSelected() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = SelectedActionName;

            bool called = false;
            _actionListener.PerformedUnityEvent.AddListener((v) => called = true);

            TriggerSelectedAction();

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerSelectedActionShouldNotActivatePerformedCSharpEventCallbackWhenWrongModeIsSelected() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = SelectedActionName;

            bool called = false;
            _actionListener.Performed += (v) => called = true;

            TriggerSelectedAction();

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAndReleaseSelectedActionShouldActivateCanceledUnityEventCallback() {
            if (!_runCancelTests) Assert.Ignore("Tests for cancel state are set to be ignored in this context.");

            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = SelectedActionName;

            bool called = false;
            _actionListener.CanceledUnityEvent.AddListener((v) => called = true);

            TriggerSelectedAction();
            CancelSelectedAction();

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerAndReleaseSelectedActionShouldActivateCanceledCSharpEventCallback() {
            if (!_runCancelTests) Assert.Ignore("Tests for cancel state are set to be ignored in this context.");

            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = SelectedActionName;

            bool called = false;
            _actionListener.Canceled += (v) => called = true;

            TriggerSelectedAction();
            CancelSelectedAction();

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerAndReleaseSelectedActionShouldNotActivateCanceledUnityEventCallbackWhenWrongModeIsSelected() {
            if (!_runCancelTests) Assert.Ignore("Tests for cancel state are set to be ignored in this context.");

            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = SelectedActionName;

            bool called = false;
            _actionListener.CanceledUnityEvent.AddListener((v) => called = true);

            TriggerSelectedAction();
            CancelSelectedAction();

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAndReleaseSelectedActionShouldNotActivateCanceledCSharpEventCallbackWhenWrongModeIsSelected() {
            if (!_runCancelTests) Assert.Ignore("Tests for cancel state are set to be ignored in this context.");

            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = SelectedActionName;

            bool called = false;
            _actionListener.Canceled += (v) => called = true;

            TriggerSelectedAction();
            CancelSelectedAction();

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAnotherActionShouldNotActivateStartedUnityEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = OtherActionName;

            bool called = false;
            _actionListener.StartedUnityEvent.AddListener((v) => called = true);

            Press(_keyboard.spaceKey);

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAnotherActionShouldNotActivateStartedCSharpEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = OtherActionName;

            bool called = false;
            _actionListener.Started += (v) => called = true;

            Press(_keyboard.spaceKey);

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAnotherActionShouldNotActivatePerformedUnityEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = OtherActionName;

            bool called = false;
            _actionListener.PerformedUnityEvent.AddListener((v) => called = true);

            Press(_keyboard.spaceKey);

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAnotherActionShouldNotActivatePerformedCSharpEventCallback() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = OtherActionName;

            bool called = false;
            _actionListener.Performed += (v) => called = true;

            Press(_keyboard.spaceKey);

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAndReleaseAnotherActionShouldNotActivateCanceledUnityEventCallback() {
            if (!_runCancelTests) Assert.Ignore("Tests for cancel state are set to be ignored.");

            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = OtherActionName;

            bool called = false;
            _actionListener.CanceledUnityEvent.AddListener((v) => called = true);

            Press(_keyboard.spaceKey);
            Release(_keyboard.spaceKey);

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAndReleaseAnotherActionShouldNotActivateCanceledCSharpEventCallback() {
            if (!_runCancelTests) Assert.Ignore("Tests for cancel state are set to be ignored.");

            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = OtherActionName;

            bool called = false;
            _actionListener.Canceled += (v) => called = true;

            Press(_keyboard.spaceKey);
            Release(_keyboard.spaceKey);

            Assert.That(called, Is.False);
        }

        public abstract IResolveConstraint IsValid();

        [Test]
        public void TriggerSelectedActionShouldActivateStartedUnityEventCallbackWithGoodValue() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = SelectedActionName;

            T value = default(T);
            _actionListener.StartedUnityEvent.AddListener((v) => value = v);

            TriggerSelectedAction();

            Assert.That(value, IsValid());
        }

        [Test]
        public void TriggerSelectedActionShouldActivateStartedCSharpEventCallbackWithGoodValue() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = SelectedActionName;

            T value = default(T);
            _actionListener.Started += (v) => value = v;

            TriggerSelectedAction();

            Assert.That(value, IsValid());
        }

        [Test]
        public void TriggerSelectedActionShouldActivatePerformedUnityEventCallbackWithGoodValue() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeUnityEvents;
            _actionListener.SelectedActionName = SelectedActionName;

            T value = default(T);
            _actionListener.PerformedUnityEvent.AddListener((v) => value = v);

            TriggerSelectedAction();

            Assert.That(value, IsValid());
        }

        [Test]
        public void TriggerSelectedActionShouldActivatePerformedCSharpEventCallbackWithGoodValue() {
            _actionListener.EventsActivationMode = AbstractInputActionListener.EventsMode.InvokeCSharpEvents;
            _actionListener.SelectedActionName = SelectedActionName;

            T value = default(T);
            _actionListener.Performed += (v) => value = v;

            TriggerSelectedAction();

            Assert.That(value, IsValid());
        }
    } 
}