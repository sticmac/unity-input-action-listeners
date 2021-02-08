using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEditor;

namespace Sticmac.InputActionListeners {
    
    [CustomEditor(typeof(InputActionListener))]
    public class InputListenerEditor : Editor {
        private SerializedObject _so = null;

        private SerializedProperty _playerInputProperty = null;
        private SerializedProperty _selectedActionNameProperty = null;
        private SerializedProperty _startedEventProperty = null;
        private SerializedProperty _performedEventProperty = null;
        private SerializedProperty _canceledEventProperty = null;

        private string[] _actionsNames = null;
        private int _lastSelectedActionIndex = 0;

        private void OnEnable() {
            _so = serializedObject;

            _playerInputProperty = _so.FindProperty("_playerInput");
            _selectedActionNameProperty = _so.FindProperty("_selectedActionName");
            _startedEventProperty = _so.FindProperty("Started");
            _performedEventProperty = _so.FindProperty("Performed");
            _canceledEventProperty = _so.FindProperty("Canceled");

            PlayerInput playerInput = _playerInputProperty.objectReferenceValue as PlayerInput;
            if (playerInput != null) {
                _actionsNames = playerInput.actions.FindActionMap(playerInput.defaultActionMap, false).Select(a => a.name).ToArray();
            } else {
                _actionsNames = new string[]{};
            }

            // The last selected action index is updated on load
            // If the current selected action name isn't found, we want the index to be set at zero (so the first action is selected instead)
            _lastSelectedActionIndex = Mathf.Max(0, Array.IndexOf(_actionsNames, _selectedActionNameProperty.stringValue));
        }

        public override void OnInspectorGUI() {
            _so.Update();

            EditorGUILayout.PropertyField(_playerInputProperty);

            if (_playerInputProperty.objectReferenceValue != null) {
                EditorGUILayout.Space(5);

                // Selected action
                _lastSelectedActionIndex = EditorGUILayout.Popup("Selected Action", _lastSelectedActionIndex, _actionsNames);
                _selectedActionNameProperty.stringValue = _actionsNames[_lastSelectedActionIndex];

                // Events
                EditorGUILayout.PropertyField(_startedEventProperty);
                EditorGUILayout.PropertyField(_performedEventProperty);
                EditorGUILayout.PropertyField(_canceledEventProperty);
            } else {
                EditorGUILayout.HelpBox("Please assign a Player Input to this script.", MessageType.Error);
            }

            _so.ApplyModifiedProperties();
        }
    }
}