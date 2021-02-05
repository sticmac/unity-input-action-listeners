using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Sticmac.InputHandler {
    public class InputListener : MonoBehaviour
    {
        [SerializeField] PlayerInput _playerInput = null;

        [SerializeField] string _selectedActionName = null;

        public UnityEvent Started;
        public UnityEvent Performed;
        public UnityEvent Canceled;

        
    }
}