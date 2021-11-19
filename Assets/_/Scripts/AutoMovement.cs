using UnityEngine;

namespace _.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class AutoMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        private bool canMove;
        private new Rigidbody rigidbody;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
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
            rigidbody.velocity = moveSpeed * transform.forward; // move forward
        }

        public void StartMovement()
        {
            canMove = true;
        }

        public void StopMovement()
        {
            DoOnStopMovement();
            rigidbody.velocity = Vector3.zero; // stop movement
            canMove = false;
        }

        protected virtual void DoOnStart()
        {
            // do nothing by default
        }

        protected virtual void DoOnUpdate()
        {
            // do nothing by default
        }
        
        protected virtual void DoOnStopMovement()
        {
            // do nothing by default
        }
    }
}
