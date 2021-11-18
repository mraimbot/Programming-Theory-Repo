using System;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _.Scripts
{
    public class GameManager : MonoBehaviour
    {
        #region Data Structures

        public struct HighScore
        {
            public string name;
            public int score;
        }

        [Serializable]
        public class GameData
        {
            public List<HighScore> highScores;
        }

        #endregion
        #region Member

        private const int SCENE_MENU_ID = 0;
        private const int SCENE_GAME_ID = 1;
        private const string TAG_SCORE = "Score";
        
        public static GameManager Instance { get; private set; }

        private Text textScore = null;
        
        public List<HighScore> HighScores { get; private set; }

        private string playerName = "";
        public string PlayerName
        {
            get => playerName;
            set
            {
                var playerNameTrimmed = value.Trim();
                playerName = playerNameTrimmed.Length <= 1 ? "NoName" : playerNameTrimmed;
            }
        }
        
        private int playerScore;
        public int PlayerScore
        {
            get => playerScore;
            set
            {
                playerScore = value >= 0 ? value : 0;
                UpdateUIPlayerScore();
            }
        }

        #endregion
        #region Singleton
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Initialize();
            
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        private void Initialize()
        {
            LoadData();
        }
        
        #endregion
        #region Menu Handler

        public void LoadGameScene()
        {
            SceneManager.LoadScene(SCENE_GAME_ID);
        }

        public void LoadMenuScene()
        {
            SceneManager.LoadScene(SCENE_MENU_ID);
            textScore = null;
        }

        public static void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        #endregion
        #region Game Handler

        private void UpdateUIPlayerScore()
        {
            if (SceneManager.GetActiveScene().buildIndex != SCENE_GAME_ID) return;
            
            if (textScore == null)
            {
                textScore = GameObject.FindWithTag(TAG_SCORE).GetComponent<Text>();
            }
            
            textScore.text = playerScore.ToString();
        }

        public void SpawnFood()
        {
            
        }

        #endregion
        #region Data Handler

        public void SaveData()
        {
            // TODO GameManager.SaveData
        }

        public bool LoadData()
        {
            // TODO GameManager.LoadData
            return false;
        }

        #endregion
    }
}
