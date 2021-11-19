using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _.Scripts
{
    public class GameManager : MonoBehaviour
    {
        #region Data Structures

        [Serializable]
        public struct HighScore
        {
            public string name;
            public int score;
        }

        [Serializable]
        public class GameData
        {
            public HighScore[] highScores;
        }

        #endregion
        #region Member

        private const int SCENE_MENU_ID = 0;
        private const int SCENE_GAME_ID = 1;
        private const string TAG_SCORE = "Score";

        [SerializeField] private List<GameObject> foods;
        [SerializeField] private Vector2 foodSpawnBoundary;
        
        public static GameManager Instance { get; private set; }

        private Text textScore = null;

        private List<HighScore> highScores = null;
        public List<HighScore> HighScores
        {
            get
            {
                if (highScores != null) return highScores;
                
                highScores = LoadData();
                
                if (highScores.Count != 0) return highScores;
                
                for (var i = 0; i < 10; i++)
                {
                    HighScores.Add(new HighScore() { name = "NoName", score = 0});
                }

                return highScores;
            }
            private set => highScores = value;
        }

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

        private bool isGameOver;

        #endregion
        #region Singleton
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        #endregion
        #region Menu Handler

        public void LoadGameScene()
        {
            SceneManager.sceneLoaded -= OnMenuSceneInitialized;
            SceneManager.sceneLoaded += OnGameSceneInitialized;
            SceneManager.LoadScene(SCENE_GAME_ID);
        }

        private void OnGameSceneInitialized(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == SCENE_GAME_ID)
            {
                StartGame();
            }
        }


        public void LoadMenuScene()
        {
            SceneManager.sceneLoaded -= OnGameSceneInitialized;
            SceneManager.sceneLoaded += OnMenuSceneInitialized;
            SceneManager.LoadScene(SCENE_MENU_ID);
            textScore = null;
        }

        private void OnMenuSceneInitialized(Scene scene, LoadSceneMode mode)
        {
            GameObject.Find("Canvas").GetComponent<UIMenuHandler>().UpdateHighScores();
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
            var position = new Vector3(Random.Range(-foodSpawnBoundary.x, foodSpawnBoundary.x), 0.7f, Random.Range(-foodSpawnBoundary.y, foodSpawnBoundary.y));
            var p = Random.Range(0.0f, 1.0f);
            GameObject prefab;
            
            if (p < 0.5f)
            {
                prefab = foods[0];
            }
            else if (p < 0.85f)
            {
                prefab = foods[1];
            }
            else
            {
                prefab = foods[2];
            }

            Instantiate(prefab, position, prefab.transform.rotation);
        }

        public void GameOver()
        {
            // prevents colliders of the body to set multiple high scores.
            if (isGameOver) return;
            isGameOver = true;
            
            if (SceneManager.GetActiveScene().buildIndex != SCENE_GAME_ID) return;
            GameObject.Find("Canvas").GetComponent<UIGameHandler>().ShowGameOver(UpdateHighScore());
        }

        private void DestroyAllBodies()
        {
            var bodies = GameObject.FindGameObjectsWithTag("Body");
            foreach (var body in bodies)
            {
                Destroy(body);
            }
        }
        
        private void DestroyAllFood()
        {
            var activeFood = GameObject.FindGameObjectsWithTag("Food");
            foreach (var food in activeFood)
            {
                Destroy(food);
            }
        }

        private bool UpdateHighScore()
        {
            for (var i = 0; i < HighScores.Count; i++)
            {
                if (playerScore <= HighScores[i].score) continue;
                
                var highScore = new HighScore()
                {
                    name = playerName,
                    score = playerScore
                };
                    
                HighScores.Insert(i, highScore);

                if (HighScores.Count == 11)
                {
                    HighScores.RemoveAt(HighScores.Count - 1);
                }
                
                SaveData();
                    
                return true;
            }

            return false;
        }

        public void StartGame()
        {
            isGameOver = false;
            DestroyAllBodies();
            DestroyAllFood();
            
            PlayerScore = 0;

            GameObject.Find("Canvas").GetComponent<UIGameHandler>().HideGameOver();
            GameObject.Find("Player").GetComponent<PlayerController>().Initialize();
            
            SpawnFood();
        }

        #endregion
        #region Data Handler

        private void SaveData()
        {
            var data = new GameData()
            {
                highScores = HighScores.ToArray()
            };

            var json = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + "/highscores.json", json);
        }

        private static List<HighScore> LoadData()
        {
            var newHighScores = new List<HighScore>();
            
            if (!File.Exists(Application.persistentDataPath + "/highscores.json")) return newHighScores;
            
            var json = File.ReadAllText(Application.persistentDataPath + "/highscores.json");
            var data = JsonUtility.FromJson<GameData>(json);

            if (data == null) return newHighScores;

            newHighScores.AddRange(data.highScores);

            return newHighScores;
        }

        #endregion
    }
}
