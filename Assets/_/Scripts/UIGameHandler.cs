using UnityEngine;
using UnityEngine.UI;

namespace _.Scripts
{
    public class UIGameHandler : MonoBehaviour
    {
        [SerializeField] private Text textScore;
        [SerializeField] private GameObject panelGameOver;
        [SerializeField] private GameObject textHighScore;

        private void Start()
        {
            HideGameOver();
        }

        public void SetTextScore(int score)
        {
            textScore.text = score.ToString();
        }

        public void ShowGameOver(bool newHighScore)
        {
            panelGameOver.SetActive(true);
            textHighScore.SetActive(newHighScore);
        }
        
        public void HideGameOver()
        {
            panelGameOver.SetActive(false);
            textHighScore.SetActive(false);
        }
        
        public void OnBackToMenuClicked()
        {
            GameManager.Instance.LoadMenuScene();
        }

        public void OnRestartClicked()
        {
            GameManager.Instance.StartGame();
        }
    }
}
