using Cinemachine;
using KBCore.Refs;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Platformer {
    public class CameraManagers : ValidatedMonoBehaviour {
        [SerializeField, Anywhere] InputReader input;
        [SerializeField, Anywhere] CinemachineFreeLook freeLookCam;


        [Header("Settings")]
        [SerializeField, Range(0f, 3f)] float speedMultiplier;


        bool isRMBPressed;
        bool cameraMovementLock;

        private void OnEnable() {
            input.Look += OnLook;
            input.EnableMouseControlCamera += OnEnableMouseControlCamera;
            input.DisableMouseControlCamera += OnDisableMouseControlCamera;
        }

        private void OnDisable() {
            input.Look -= OnLook;
            input.EnableMouseControlCamera -= OnEnableMouseControlCamera;
            input.DisableMouseControlCamera -= OnDisableMouseControlCamera;
        }

        private void OnDisableMouseControlCamera() {
            isRMBPressed = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;

            freeLookCam.m_XAxis.m_InputAxisValue = 0f;
            freeLookCam.m_YAxis.m_InputAxisValue = 0f;
        }

        private void OnEnableMouseControlCamera() {
            isRMBPressed = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(DisableMouseForFrame());

        }


        IEnumerator DisableMouseForFrame() {
            cameraMovementLock = true;
            yield return new WaitForEndOfFrame();
            cameraMovementLock = false;
        }

        private void OnLook(Vector2 cameraMovement, bool isDeviceMouse) {
            if (cameraMovementLock) return;

            if (isDeviceMouse && !isRMBPressed) return;

            float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;

            freeLookCam.m_XAxis.m_InputAxisValue = cameraMovement.x * speedMultiplier * deviceMultiplier;
            freeLookCam.m_YAxis.m_InputAxisValue = cameraMovement.y * speedMultiplier * deviceMultiplier;


        }
    }
}
