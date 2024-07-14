using Cinemachine;
using KBCore.Refs;
using UnityEngine;

namespace Platformer {
    public class PlayerController : ValidatedMonoBehaviour {
        [Header("References")]
        [SerializeField, Self] CharacterController controller;
        [SerializeField, Self] Animator animator;
        [SerializeField, Anywhere] CinemachineFreeLook freeLookCam;
        [SerializeField,Anywhere] InputReader input;


        [Header("Settings")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = 0.2f;

        Transform mainCam;

        const float Zerof = 0f;
        float currentSpeed;
        float velocity;

        static readonly int speed = Animator.StringToHash("Speed");

        private void Awake() {
            mainCam = Camera.main.transform;
            freeLookCam.Follow = transform;
            freeLookCam.LookAt = transform;

            freeLookCam.OnTargetObjectWarped(transform, transform.position - freeLookCam.transform.position - Vector3.forward);

        }

        private void Start() {
            input.EnablePlayerActions();
        }

        private void Update() {
            HandleMovements();
            UpdateAnimator();

        }

        void HandleMovements() {
            var movementDir = new Vector3(input.Direction.x, 0f, input.Direction.y).normalized;

            var adjustetdDir = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movementDir;
            if(adjustetdDir.magnitude > Zerof) {
                HandleRotation(adjustetdDir);
                //Move the player
                HandleCharacterController(adjustetdDir);
                SmoothSpeed(adjustetdDir.magnitude);
            }
            else {
                SmoothSpeed(0f);

            }


        }

        void HandleCharacterController(Vector3 adjustedDir) {
            var adjustedMovement = adjustedDir * (moveSpeed * Time.deltaTime);
            controller.Move(adjustedMovement);
        }

        void HandleRotation(Vector3 adjustetdDir) {
            var targetRot = Quaternion.LookRotation(adjustetdDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            //transform.LookAt(transform.position + adjustetdDir);
        }

        void SmoothSpeed(float target) {
            currentSpeed =  Mathf.SmoothDamp(currentSpeed, target, ref velocity, smoothTime);

        }

        void UpdateAnimator() {
            animator.SetFloat(speed, currentSpeed);
        }
    }
}
