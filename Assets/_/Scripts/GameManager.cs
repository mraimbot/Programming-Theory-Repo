using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

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
        
        public static GameManager Instance { get; private set; }
        
        public List<HighScore> HighScores { get; private set; }

        private string playerName = "";

        public string PlayerName
        {
            get => playerName;
            set
            {
                var playerNameTrimmed = value.Trim();
                playerName = (playerNameTrimmed.Length <= 1 ? "NoName" : playerNameTrimmed);
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

        public static void LoadGameScene()
        {
            SceneManager.LoadScene(SCENE_GAME_ID);
        }

        public static void LoadMenuScene()
        {
            SceneManager.LoadScene(SCENE_MENU_ID);
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
