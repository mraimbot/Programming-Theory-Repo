using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace _.Scripts
{
    public class GameManager : MonoBehaviour
    {
        #region Data Structures

        private enum Food { Banana, Apple, Potion }
        
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

        [SerializeField] private List<GameObject> foods;
        [SerializeField] private Vector2 foodSpawnBoundary;
        
        public static GameManager Instance { get; private set; }
        
        private UIMenuHandler uiMenuHandler;
        private UIGameHandler uiGameHandler;
        private PlayerController player;
        private bool isGameOver;


        private List<HighScore> highScores;
        private List<HighScore> HighScores
        {
            get
            {
                if (highScores != null) return highScores;
                
                highScores = LoadData();
                
                if (highScores.Count != 0) return highScores;
                
                // set default values if no highscore.json is available
                for (var i = 0; i < 10; i++)
                {
                    HighScores.Add(new HighScore() { name = "NoName", score = 0});
                }

                return highScores;
            }
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
                if (uiGameHandler != null) uiGameHandler.SetTextScore(playerScore);
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

            DontDestroyOnLoad(gameObject);
            Instance = this;

#if UNITY_EDITOR
            if (SceneManager.GetActiveScene().buildIndex == SCENE_MENU_ID)
            {
#endif
                uiMenuHandler = GameObject.Find("Canvas").GetComponent<UIMenuHandler>();
                uiMenuHandler.SetTextHighScores(HighScores);
#if UNITY_EDITOR
            }
            else
            {
                uiGameHandler = GameObject.Find("Canvas").GetComponent<UIGameHandler>();
            }
#endif
        }

        #endregion
        #region Menu Handler

        public void LoadMenuScene()
        {
            uiGameHandler = null;
            player = null;
            
            SceneManager.sceneLoaded -= OnGameSceneInitialized;
            SceneManager.sceneLoaded += OnMenuSceneInitialized;
            SceneManager.LoadScene(SCENE_MENU_ID);
        }
        
        private void OnMenuSceneInitialized(Scene scene, LoadSceneMode mode)
        {
            uiMenuHandler = GameObject.Find("Canvas").GetComponent<UIMenuHandler>();
            uiMenuHandler.SetTextPlayerName(playerName);
            uiMenuHandler.SetTextHighScores(HighScores);
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
        
        public void LoadGameScene()
        {
            uiMenuHandler = null;
            
            SceneManager.sceneLoaded -= OnMenuSceneInitialized;
            SceneManager.sceneLoaded += OnGameSceneInitialized;
            SceneManager.LoadScene(SCENE_GAME_ID);
        }
        
        private void OnGameSceneInitialized(Scene scene, LoadSceneMode mode)
        {
            uiGameHandler = GameObject.Find("Canvas").GetComponent<UIGameHandler>();
            player = GameObject.Find("Player").GetComponent<PlayerController>();
            StartGame();
        }
        
        public void StartGame()
        {
            DestroyAllBodies();
            DestroyAllFood();
            
            isGameOver = false;
            PlayerScore = 0;

            uiGameHandler.HideGameOver();
            player.Initialize();
            
            SpawnFood();
        }
        
        private static void DestroyAllBodies()
        {
            var bodies = GameObject.FindGameObjectsWithTag("Body");
            foreach (var body in bodies)
            {
                Destroy(body);
            }
        }
        
        private static void DestroyAllFood()
        {
            var food = GameObject.FindGameObjectsWithTag("Food");
            foreach (var foo in food)
            {
                Destroy(foo);
            }
        }

        public void SpawnFood()
        {
            Instantiate(GetRandomPrefab(), GetRandomPosition(), Quaternion.identity);
        }

        private Vector3 GetRandomPosition()
        {
            return new Vector3(Random.Range(-foodSpawnBoundary.x, foodSpawnBoundary.x),
                0.7f,
                Random.Range(-foodSpawnBoundary.y, foodSpawnBoundary.y));
        }

        private GameObject GetRandomPrefab()
        {
            var p = Random.Range(0.0f, 1.0f);
            
            if (p < 0.5f) // 50% banana
            {
                return foods[(int)Food.Banana];
            }
            
            return p < 0.85f
                ? foods[(int)Food.Apple]   // 35% apples
                : foods[(int)Food.Potion]; // 15% potions
        }

        public void GameOver()
        {
            // it's possible that the player hits multiple parts of the body in the end.
            // isGameOver prevents multiple calls of this method.
            if (isGameOver) return;
            isGameOver = true;
            uiGameHandler.ShowGameOver(UpdateHighScores()/*true: new high score*/);
        }
        
        private bool UpdateHighScores()
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
        
        #endregion
        #region Data Handler

        private const string FILEPATH_HIGH_SCORE = "/highscores.json";

        private void SaveData()
        {
            var data = new GameData()
            {
                highScores = HighScores.ToArray()
            };

            var json = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + FILEPATH_HIGH_SCORE, json);
        }

        private static List<HighScore> LoadData()
        {
            var newHighScores = new List<HighScore>();
            
            if (!File.Exists(Application.persistentDataPath + FILEPATH_HIGH_SCORE)) return newHighScores;
            
            var json = File.ReadAllText(Application.persistentDataPath + FILEPATH_HIGH_SCORE);
            var data = JsonUtility.FromJson<GameData>(json);

            if (data == null) return newHighScores;

            newHighScores.AddRange(data.highScores);

            return newHighScores;
        }

        #endregion
    }
}
