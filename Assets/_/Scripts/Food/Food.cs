using System;
using UnityEngine;

namespace _.Scripts.Food
{
    public abstract class Food : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;
        [SerializeField] private int score;
            
        protected abstract void DoOnCollision(PlayerController player); // POLYMORPHISM

        private void Update()
        {
            transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
        }

        private void OnTriggerEnter(Collider other)
        {
            DoOnCollision(other.gameObject.GetComponent<PlayerController>());
            
            GameManager.Instance.PlayerScore += score;
            GameManager.Instance.SpawnFood();
            
            Destroy(gameObject);
        }
    }
}
