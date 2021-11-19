using UnityEngine;

namespace _.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class AutoMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        private bool canMove;
        private  Rigidbody rb; // 'new' to override the obsolete member rigidbody

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            DoOnStart();
        }

        private void Update()
        {
            DoOnUpdate();
            Move();
        }

        private void Move()
        {
            if (!canMove) return;
            rb.velocity = moveSpeed * transform.forward; // move forward
        }

        protected void StartMovement()
        {
            canMove = true;
        }

        public void StopMovement()
        {
            rb.velocity = Vector3.zero; // stop movement
            canMove = false;
            DoOnStopMovement();
        }

        protected virtual void DoOnStart() { /*do nothing by default*/ } // POLYMORPHISM
        protected virtual void DoOnUpdate() { /*do nothing by default*/ } // POLYMORPHISM
        protected virtual void DoOnStopMovement() { /*do nothing by default*/ } // POLYMORPHISM
    }
}
