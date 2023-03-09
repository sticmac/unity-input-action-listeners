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
        private SerializedProperty _selectedActionMapNameProperty = null;
        private SerializedProperty _selectedActionNameProperty = null;
        private SerializedProperty _eventsModeProperty = null;
        private SerializedProperty _startedEventProperty = null;
        private SerializedProperty _performedEventProperty = null;
        private SerializedProperty _canceledEventProperty = null;

        private AbstractInputActionListener _target = null;

        private string[] _actionsNames = null;
        private string[] _actionMapsNames = null;
        private int _lastSelectedActionIndex = 0;
        private int _lastSelectedActionMapIndex = 0;

        private void UpdateActionMapNames(PlayerInput playerInput) {
            if (playerInput != null) {
                _actionMapsNames = playerInput.actions.actionMaps.Select(m => m.name).ToArray();
            } else {
                _actionMapsNames = new string[]{};
            }
        }

        private void UpdateActionNames(PlayerInput playerInput, string actionMap) {
            if (playerInput != null) {
                _actionsNames = playerInput.actions.FindActionMap(actionMap, false)
                    .Select(a => a.name).ToArray();
            } else {
                _actionsNames = new string[]{};
            }
        }

        private void OnEnable() {
            _so = serializedObject;

            _playerInputProperty = _so.FindProperty("_playerInput");
            _selectedActionMapNameProperty = _so.FindProperty("_selectedActionMapName");
            _selectedActionNameProperty = _so.FindProperty("_selectedActionName");

            _eventsModeProperty = _so.FindProperty("_eventsActivationMode");

            _startedEventProperty = _so.FindProperty("StartedUnityEvent");
            _performedEventProperty = _so.FindProperty("PerformedUnityEvent");
            _canceledEventProperty = _so.FindProperty("CanceledUnityEvent");

            PlayerInput playerInput = _playerInputProperty.objectReferenceValue as PlayerInput;
            UpdateActionMapNames(playerInput);
            UpdateActionNames(playerInput, _selectedActionMapNameProperty.stringValue);

            // The last selected action and action map indexes are updated on load
            // If the current selected action name isn't found, we want the index to be set at zero (so the first action is selected instead)
            _lastSelectedActionMapIndex = Mathf.Max(0, Array.IndexOf(_actionMapsNames, _selectedActionMapNameProperty.stringValue));
            _lastSelectedActionIndex = Mathf.Max(0, Array.IndexOf(_actionsNames, _selectedActionNameProperty.stringValue));

            _target = target as AbstractInputActionListener;
        }

        public override void OnInspectorGUI() {
            _so.Update();

            EditorGUILayout.PropertyField(_playerInputProperty);
            PlayerInput pi = _playerInputProperty.objectReferenceValue as PlayerInput;
            if (_target.PlayerInput != pi) {
                UpdateActionMapNames(pi);
                UpdateActionNames(pi, pi.defaultActionMap);
                _target.PlayerInput = pi;
            }

            if (_playerInputProperty.objectReferenceValue != null) {
                EditorGUILayout.Space(10);

                // Selected action map
                _lastSelectedActionMapIndex = EditorGUILayout.Popup("Selected Action Map", _lastSelectedActionMapIndex, _actionMapsNames);
                _selectedActionMapNameProperty.stringValue = _actionMapsNames[_lastSelectedActionMapIndex];

                // Update actions names
                if (_selectedActionMapNameProperty.stringValue != _target.SelectedActionMapName) {
                    UpdateActionNames(pi, _selectedActionMapNameProperty.stringValue);
                }

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