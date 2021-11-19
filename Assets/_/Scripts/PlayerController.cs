using UnityEngine;
using UnityEngine.InputSystem;

namespace _.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerController : AutoMovement
    {
        [SerializeField] private InputActionMap input;
        private InputAction moveAction;

        [SerializeField] private GameObject prefabBody;
        [SerializeField] private GameObject nextTarget;
        private BodyController body;
        
        [SerializeField] private Transform startPosition;
        [SerializeField] private float rotationSpeed;
        private float direction;
        private bool canRotate;

        private new AudioSource audio;
        
        protected override void DoOnStart()
        {
            moveAction = input.FindAction("Move");
            moveAction.started += OnMoving;
            moveAction.canceled += OnMoving; // Sets the direction vector to 0
            moveAction.Enable();
            input.Enable();

            audio = GetComponent<AudioSource>();

            Initialize();
        }
        
        protected override void DoOnUpdate()
        {
            if (!canRotate) return;
            transform.Rotate(Vector3.up, Time.deltaTime * direction * rotationSpeed);
        }

        private void OnMoving(InputAction.CallbackContext context)
        {
            direction = context.ReadValue<float>();
        }

        public void Initialize()
        {
            var tf = transform;
            tf.position = startPosition.position;
            tf.rotation = startPosition.rotation;
            
            StartMovement();
            canRotate = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            audio.Play();
        }

        private void OnCollisionEnter()
        {
            StopMovement();
            GameManager.Instance.GameOver();
        }

        protected override void DoOnStopMovement()
        {
            canRotate = false;
            if (body != null)
            {
                body.StopMovement();
            }
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
