using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Sticmac.InputActionListeners {
    public class ParametrizedInputActionListenerTests<T, U, V> : InputTestFixture
        where T : struct
        where U : UnityEvent<T>, new()
        where V : ParametrizedInputActionListener<T, U>
    {
        protected const string Action1Name = "action1";
        protected const string Action2Name = "action2";

        protected InputActionMap _actionMap = null;

        protected InputAction _action1 = null;
        protected InputAction _action2 = null;

        protected Keyboard _keyboard = null;
        protected Gamepad _gamepad = null;

        protected ParametrizedInputActionListener<T, U> _actionListener;

        // Overrides InputTestFixture setup method
        public override void Setup() {
            base.Setup();
            // Input System config
            _keyboard = InputSystem.AddDevice<Keyboard>();
            _gamepad = InputSystem.AddDevice<Gamepad>();

            GameObject go = new GameObject();

            // Input Action Asset 
            InputActionAsset asset = ScriptableObject.CreateInstance<InputActionAsset>();
            _actionMap = asset.AddActionMap("map");

            InitializeActions();

            _action1.Enable();
            _action2.Enable();

            // Player Input
            PlayerInput pi = go.AddComponent<PlayerInput>();
            pi.actions = asset;

            _actionListener = go.AddComponent<V>();
            _actionListener.PlayerInput = pi;
        }
        
        protected virtual void InitializeActions() {
            _action1 = _actionMap.AddAction(Action1Name, binding: "<Keyboard>/space");
            _action2 = _actionMap.AddAction(Action2Name, binding: "<Gamepad>/leftStick");
        }

        public override void TearDown() {
            base.TearDown();
            _action1.Disable();
            _action2.Disable();
        }

        [Test]
        public void TriggerSelectedActionShouldActivateStartedCallback() {
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Started.AddListener((v) => called = true);

            Press(_keyboard.spaceKey);

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerAnotherActionShouldNotActivateStartedCallback() {
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Started.AddListener((v) => called = true);

            Set(_gamepad.leftStick, new Vector2(0.1f, 0.234f));

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerSelectedActionShouldActivatePerformedCallback() {
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Performed.AddListener((v) => called = true);

            Press(_keyboard.spaceKey);

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerAnotherActionShouldNotActivatePerformedCallback() {
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Performed.AddListener((v) => called = true);

            Set(_gamepad.leftStick, new Vector2(0.123f, 0.234f));

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAndReleaseSelectedActionShouldActivateCanceledCallback() {
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Canceled.AddListener((v) => called = true);

            Press(_keyboard.spaceKey);
            Release(_keyboard.spaceKey);

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerAndReleaseAnotherActionShouldNotActivateCanceledCallback() {
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Canceled.AddListener((v) => called = true);

            Set(_gamepad.leftStick, new Vector2(0.123f, 0.234f));
            Set(_gamepad.leftStick, new Vector2(0f, 0f));

            Assert.That(called, Is.False);
        }
    } 
}