using UnityEngine;
using UnityEngine.UI;

namespace _.Scripts
{
    public class UIGameHandler : MonoBehaviour
    {
        [SerializeField] private Text textScore;
        
        public void OnBackToMenuClicked()
        {
            GameManager.Instance.LoadMenuScene();
        }

        public void UpdateScoreText(int newScore)
        {
            textScore.text = newScore.ToString();
        }
    }
}
