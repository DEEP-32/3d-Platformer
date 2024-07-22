using System;
using UnityEngine;
using Utils;

namespace Platformer {
    public class GroundChecker : MonoBehaviour {
        [SerializeField] float groundDistance = 0.08f;

        [SerializeField] LayerMask groundLayers;

        public bool IsGrounded {
            get;
            private set;
        }

        private void Update() {
            IsGrounded = Physics.CheckSphere(transform.position, groundDistance, groundLayers);
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.black;

            Gizmos.DrawWireSphere(transform.position, groundDistance);
        }

    }

}

