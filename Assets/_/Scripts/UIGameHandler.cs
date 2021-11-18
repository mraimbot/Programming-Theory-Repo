using System;
using UnityEngine;
using UnityEngine.UI;

namespace _.Scripts
{
    public class UIGameHandler : MonoBehaviour
    {
        [SerializeField] private GameObject panelGameOver;

        private void Start()
        {
            HideGameOver();
        }

        public void OnBackToMenuClicked()
        {
            GameManager.Instance.LoadMenuScene();
        }

        public void OnRestartClicked()
        {
            GameManager.Instance.InitializeGame();
        }

        public void ShowGameOver()
        {
            panelGameOver.SetActive(true);
        }
        
        public void HideGameOver()
        {
            panelGameOver.SetActive(false);
        }
    }
}
