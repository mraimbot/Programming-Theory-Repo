using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _.Scripts
{
    public class PlayerController : AutoMovement
    {
        [SerializeField] private InputActionMap input;
        private InputAction moveAction;

        [SerializeField] private GameObject prefabBody;
        [SerializeField] private GameObject nextTarget;
        private BodyController body = null;
        
        [SerializeField] private Transform startPosition;
        [SerializeField] private float rotationSpeed;
        private float direction;
        private bool canRotate;
        
        protected override void DoOnStart()
        {
            moveAction = input.FindAction("Move");
            moveAction.started += OnMoving;
            moveAction.canceled += OnMoving; // Sets the direction vector to 0
            moveAction.Enable();
            input.Enable();

            Initialize();
        }

        protected override void DoOnUpdate()
        {
            if (!canRotate) return;
            transform.Rotate(Vector3.up, Time.deltaTime * direction * rotationSpeed);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Border") && !other.gameObject.CompareTag("Body")) return;
            GameOver();
        }

        private void OnMoving(InputAction.CallbackContext context)
        {
            direction = context.ReadValue<float>();
        }

        private void GameOver()
        {
            canRotate = false;
            StopMovement();
            if (body != null)
            {
                body.StopMovement();
            }
            GameManager.Instance.GameOver();
        }

        public void Initialize()
        {
            direction = 0.0f;
            
            var tf = transform;
            tf.position = startPosition.position;
            tf.rotation = startPosition.rotation;
            
            StartMovement();
            canRotate = true;
        }

        public void AddBody()
        {
            if (body == null)
            {
                body = Instantiate(prefabBody, nextTarget.transform.position, prefabBody.transform.rotation).GetComponent<BodyController>();
            }
            else
            {
                body.AddBody();
            }
        }

        public void RemoveBody()
        {
            if (body == null) return;
            body.RemoveBody();
        }
    }
}
