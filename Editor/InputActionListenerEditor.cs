using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEditor;

namespace Sticmac.InputActionListeners {
    
    [CustomEditor(typeof(AbstractInputActionListener), true)]
    public class InputListenerEditor : Editor {
        private SerializedObject _so = null;

        private SerializedProperty _playerInputProperty = null;
        private SerializedProperty _selectedActionNameProperty = null;
        private SerializedProperty _eventsModeProperty = null;
        private SerializedProperty _startedEventProperty = null;
        private SerializedProperty _performedEventProperty = null;
        private SerializedProperty _canceledEventProperty = null;

        private AbstractInputActionListener _target = null;

        private string[] _actionsNames = null;
        private int _lastSelectedActionIndex = 0;

        private void OnEnable() {
            _so = serializedObject;

            _playerInputProperty = _so.FindProperty("_playerInput");
            _selectedActionNameProperty = _so.FindProperty("_selectedActionName");

            _eventsModeProperty = _so.FindProperty("_eventsMode");

            _startedEventProperty = _so.FindProperty("StartedUnityEvent");
            _performedEventProperty = _so.FindProperty("PerformedUnityEvent");
            _canceledEventProperty = _so.FindProperty("CanceledUnityEvent");

            PlayerInput playerInput = _playerInputProperty.objectReferenceValue as PlayerInput;
            if (playerInput != null) {
                _actionsNames = playerInput.actions.FindActionMap(playerInput.defaultActionMap, false).Select(a => a.name).ToArray();
            } else {
                _actionsNames = new string[]{};
            }

            // The last selected action index is updated on load
            // If the current selected action name isn't found, we want the index to be set at zero (so the first action is selected instead)
            _lastSelectedActionIndex = Mathf.Max(0, Array.IndexOf(_actionsNames, _selectedActionNameProperty.stringValue));

            _target = target as AbstractInputActionListener;
        }

        public override void OnInspectorGUI() {
            _so.Update();

            EditorGUILayout.PropertyField(_playerInputProperty);
            PlayerInput pi = _playerInputProperty.objectReferenceValue as PlayerInput;
            if (_target.PlayerInput != pi) {
                _target.PlayerInput = pi;
            }

            if (_playerInputProperty.objectReferenceValue != null) {
                EditorGUILayout.Space(10);

                // Selected action
                _lastSelectedActionIndex = EditorGUILayout.Popup("Selected Action", _lastSelectedActionIndex, _actionsNames);
                _selectedActionNameProperty.stringValue = _actionsNames[_lastSelectedActionIndex];

                EditorGUILayout.PropertyField(_eventsModeProperty);

                EditorGUILayout.Space(5);

                // Events (displayed only if the unity events mode is selected)
                if ((AbstractInputActionListener.EventsMode)_eventsModeProperty.enumValueIndex == AbstractInputActionListener.EventsMode.InvokeUnityEvents) {
                    EditorGUILayout.PropertyField(_startedEventProperty, new GUIContent("Started"));
                    EditorGUILayout.PropertyField(_performedEventProperty, new GUIContent("Performed"));
                    EditorGUILayout.PropertyField(_canceledEventProperty, new GUIContent("Canceled"));
                }
            } else {
                EditorGUILayout.HelpBox("Please assign a Player Input to this script.", MessageType.Error);
            }

            _so.ApplyModifiedProperties();
        }
    }
}