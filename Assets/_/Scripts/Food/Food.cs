using System;
using UnityEngine;

namespace _.Scripts.Food
{
    public abstract class Food : MonoBehaviour
    {
        private const string TAG_PLAYER = "Player";
        
        [SerializeField] private int score;

        protected abstract void DoOnCollision(PlayerController player);

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
