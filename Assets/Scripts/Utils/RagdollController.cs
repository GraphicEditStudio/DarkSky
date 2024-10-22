using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class RagdollController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private IEnumerable<Rigidbody> _ragdollRigidBodies;
        private void Start()
        {
            _ragdollRigidBodies = GetComponentsInChildren<Rigidbody>();
            DisableRagdoll();
        }

        public void EnableRagdoll()
        {
            // todo: apply gun bullet force?
            SetRagdollKinematic(false);
            animator.enabled = false;
        }

        public void DisableRagdoll()
        {
            SetRagdollKinematic(true);
            animator.enabled = true;
        }

        private void SetRagdollKinematic(bool isKinematic)
        {
            foreach (var rigidbody in _ragdollRigidBodies)
            {
                rigidbody.isKinematic = isKinematic;
            }
        }
    }
}