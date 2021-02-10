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
            _action1.Disable();
            _action2.Disable();
        }

        [Test]
        public void TriggerSelectedActionShouldActivateStartedCallback() {
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Started.AddListener(() => called = true);

            Press(_keyboard.spaceKey);

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerAnotherActionShouldNotActivateStartedCallback() {
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Started.AddListener(() => called = true);

            Set(_gamepad.leftStick, new Vector2(0.123f, 0.234f));

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerSelectedActionShouldActivatePerformedCallback() {
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Performed.AddListener(() => called = true);

            Press(_keyboard.spaceKey);

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerAnotherActionShouldNotActivatePerformedCallback() {
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Performed.AddListener(() => called = true);

            Set(_gamepad.leftStick, new Vector2(0.123f, 0.234f));

            Assert.That(called, Is.False);
        }

        [Test]
        public void TriggerAndReleaseSelectedActionShouldActivateCanceledCallback() {
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Canceled.AddListener(() => called = true);

            Press(_keyboard.spaceKey);
            Release(_keyboard.spaceKey);

            Assert.That(called, Is.True);
        }

        [Test]
        public void TriggerAndReleaseAnotherActionShouldNotActivateCanceledCallback() {
            _actionListener.SelectedActionName = Action1Name;

            bool called = false;
            _actionListener.Canceled.AddListener(() => called = true);

            Set(_gamepad.leftStick, new Vector2(0.123f, 0.234f));
            Set(_gamepad.leftStick, new Vector2(0f, 0f));

            Assert.That(called, Is.False);
        }
    } 
}