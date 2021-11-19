using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputActionMap input;
        private InputAction moveAction;

        [SerializeField] private GameObject prefabBody;
        private List<GameObject> bodyTiles;
        
        [SerializeField] private Transform startPosition;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;
        private float direction;
        

        private new Rigidbody rigidbody;

        private bool canMove;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            
            moveAction = input.FindAction("Move");
            moveAction.started += OnMoving;
            moveAction.canceled += OnMoving; // Sets the direction vector to 0
            moveAction.Enable();
            input.Enable();

            Initialize();
        }

        private void Update()
        {
            if (!canMove) return;
        
            rigidbody.velocity = moveSpeed * transform.forward; // move forward
            transform.Rotate(Vector3.up, Time.deltaTime * direction * rotationSpeed);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Deadly")) return;

            canMove = false;
            GameManager.Instance.GameOver();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Deadly")) return;
            GameOver();
        }

        private void OnMoving(InputAction.CallbackContext context)
        {
            direction = context.ReadValue<float>();
        }

        private void GameOver()
        {
            rigidbody.velocity = Vector3.zero; // stop movement
            canMove = false;
            GameManager.Instance.GameOver();
        }

        public void Initialize()
        {
            canMove = true;
            direction = 0.0f;
            
            var tf = transform;
            
            tf.position = startPosition.position;
            tf.rotation = startPosition.rotation;
        }

        public void AddBodyTile()
        {
            var body = Instantiate(prefabBody, Vector3.positiveInfinity, prefabBody.transform.rotation);
            var bodyController = body.GetComponent<BodyController>();

            bodyController.Target = bodyTiles.Count == 0 ? gameObject : bodyTiles[bodyTiles.Count - 1];
        }

        public void RemoveBodyTile()
        {
        
        }
    }
}
