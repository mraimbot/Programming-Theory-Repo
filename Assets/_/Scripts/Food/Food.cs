using System;
using UnityEngine;

namespace _.Scripts.Food
{
    public abstract class Food : MonoBehaviour
    {
        private const string TAG_PLAYER = "Player";

        [SerializeField] private float rotationSpeed;
        [SerializeField] private int score;

        protected abstract void DoOnCollision(PlayerController player);

        private void Update()
        {
            transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(TAG_PLAYER)) return;
            
            GameManager.Instance.PlayerScore += score;
            DoOnCollision(other.gameObject.GetComponent<PlayerController>());
            GameManager.Instance.SpawnFood();
            Destroy(gameObject);
        }
    }
}
