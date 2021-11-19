using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _.Scripts
{
        public class UIMenuHandler : MonoBehaviour
        {
                [SerializeField] private InputField textPlayerName;
                [SerializeField] private List<Text> textHighScores;

                private void Start()
                {
                        textPlayerName.text = GameManager.Instance.PlayerName;
                        UpdateHighScores();
                }

                public void OnPlayClicked()
                {
                        GameManager.Instance.PlayerName = textPlayerName.text;
                        GameManager.Instance.LoadGameScene();
                }

                public void OnExitClicked()
                {
                        GameManager.QuitGame();
                }

                public void UpdateHighScores()
                {
                        var highScores = GameManager.Instance.HighScores;
                        for (var i = 0; i < highScores.Count; i++)
                        {
                                var highScore = highScores[i];
                                textHighScores[i].text = i+1 + ". " + highScore.name + " (" + highScore.score + ")";
                        }
                }
        }
}
