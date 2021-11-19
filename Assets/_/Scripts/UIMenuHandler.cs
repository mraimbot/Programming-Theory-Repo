using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _.Scripts
{
    public class UIMenuHandler : MonoBehaviour
    {
        [SerializeField] private InputField textPlayerName;
        [SerializeField] private List<Text> textHighScores;

        public void SetTextPlayerName(string playerName)
        {
            textPlayerName.text = playerName;
        }

        public void SetTextHighScores(List<GameManager.HighScore> highScores)
        {
            for (var i = 0; i < highScores.Count; i++)
            {
                var highScore = highScores[i];
                textHighScores[i].text = i+1 + ". " + highScore.name + " (" + highScore.score + ")";
            }
        }
                
        private void OnPlayClicked()
        {
            GameManager.Instance.PlayerName = textPlayerName.text;
            GameManager.Instance.LoadGameScene();
        }

        private void OnExitClicked()
        {
            GameManager.QuitGame();
        }
    }
}
