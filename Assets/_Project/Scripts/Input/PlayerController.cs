using Cinemachine;
using KBCore.Refs;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Platformer {


    public class PlayerController : ValidatedMonoBehaviour {
        [Header("References")]
        [SerializeField,Self] Rigidbody rb;
        [SerializeField,Self] GroundChecker groundChecker;


        [SerializeField, Self] Animator animator;
        [SerializeField, Anywhere] CinemachineFreeLook freeLookCam;
        [SerializeField,Anywhere] InputReader input;


        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = 0.2f;


        [Header("Jump Settings")]
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float jumpCooldown = 0.0f;
        [SerializeField] float jumpDuration = 0f;
        [SerializeField] float jumpMaxHeight = 2f;
        [SerializeField] float gravityMultiplier = 3f;

        Transform mainCam;

        const float Zerof = 0f;
        float currentSpeed;
        float velocity;
        float jumpVelocity;
        Vector3 movement;

        List<Timer> timers;
        CountdownTimer jumpTimer;
        CountdownTimer jumpCooldownTimer;

        static readonly int speed = Animator.StringToHash("Speed");

        private void Awake() {
            mainCam = Camera.main.transform;
            freeLookCam.Follow = transform;
            freeLookCam.LookAt = transform;

            freeLookCam.OnTargetObjectWarped(transform, transform.position - freeLookCam.transform.position - Vector3.forward);


            rb.freezeRotation = true;

            jumpTimer = new CountdownTimer(jumpDuration);
            jumpCooldownTimer = new CountdownTimer(jumpCooldown);

            timers = new List<Timer>(2) {
                jumpTimer,
                jumpCooldownTimer
            };

            jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();
        }

        private void Start() {
            input.EnablePlayerActions();
        }


        private void OnEnable() {
            input.Jump += Jump;
        }

        private void OnDisable() {
            input.Jump -= Jump;

        }

        void Jump(bool performed) {
            if(performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded) {
                jumpTimer.Start();
            }
            else if (!performed && jumpTimer.IsRunning) {
                jumpTimer.Stop();
            }
        }

        private void Update() {

            movement = new Vector3(input.Direction.x, 0f, input.Direction.y);
            HandleTimer();
            UpdateAnimator();

        }

        private void FixedUpdate() {
            HandleJump();
            HandleMovements();
        }


        void HandleJump() {
            if (!jumpTimer.IsRunning && groundChecker.IsGrounded) {
                jumpVelocity = Zerof;
                jumpTimer.Stop();
                return;
            }

            if(jumpTimer.IsRunning) {
                float launchPoint = 0.9f;

                if(jumpTimer.Progress > launchPoint) {
                    jumpVelocity = Mathf.Sqrt(2*jumpMaxHeight * Mathf.Abs(Physics.gravity.y));
                }
                else {
                    jumpVelocity = (1 - jumpTimer.Progress) * jumpForce * Time.fixedDeltaTime;
                }
            }

            else {
                jumpVelocity = Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            }


            rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.y);


        }

        void HandleTimer() {
            foreach (var timer in timers) {
                timer.Tick(Time.deltaTime);
            }
        }

        void HandleMovements() {
            var adjustetdDir = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement.normalized;
            if(adjustetdDir.magnitude > Zerof) {
                HandleRotation(adjustetdDir);
                //Move the player
                HandleHorizontalMovement(adjustetdDir);
                SmoothSpeed(adjustetdDir.magnitude);
            }
            else {
                SmoothSpeed(0f);
                rb.velocity = new(Zerof, rb.velocity.y, Zerof);
            }


        }

        void HandleHorizontalMovement(Vector3 adjustedDir) {
            var adjustedMovement = adjustedDir * (moveSpeed * Time.fixedDeltaTime);
            rb.velocity = adjustedMovement;
        }

        void HandleRotation(Vector3 adjustetdDir) {
            var targetRotation = Quaternion.LookRotation(adjustetdDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
