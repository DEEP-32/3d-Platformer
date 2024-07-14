using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInputActions;

namespace Platformer {

    [CreateAssetMenu(fileName ="InputReader",menuName ="Platformer/InputReader")]

    public class InputReader : ScriptableObject,IPlayerActions
    {
        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<Vector2,bool> Look = delegate { };
        public event UnityAction EnableMouseControlCamera = delegate { };
        public event UnityAction DisableMouseControlCamera = delegate { };


        PlayerInputActions inputActions;

        public Vector3 Direction => inputActions.Player.Move.ReadValue<Vector2>();

        void OnEnable() {
            if(inputActions == null) {
                inputActions = new PlayerInputActions();
                inputActions.Player.SetCallbacks(this);
            }
           
        }

        public void EnablePlayerActions() {
            inputActions.Enable();
        }

        public void OnFire(InputAction.CallbackContext context) {
            
        }

        public void OnJump(InputAction.CallbackContext context) {
            
        }

        public void OnLook(InputAction.CallbackContext context) {
            Look.Invoke(context.action.ReadValue<Vector2>(), IsDeviceMouse(context));
        }

        public void OnMouseControlCamera(InputAction.CallbackContext context) {
            switch (context.phase) {
                case InputActionPhase.Started:
                    EnableMouseControlCamera.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    DisableMouseControlCamera.Invoke();
                    break;
            }
        }

        public void OnMove(InputAction.CallbackContext context) {
            Move.Invoke(context.ReadValue<Vector2>());
        }

        public void OnRun(InputAction.CallbackContext context) {
            
        }

        bool IsDeviceMouse(InputAction.CallbackContext context) {
            return context.control.name == "Mouse";
        }

    }
}
