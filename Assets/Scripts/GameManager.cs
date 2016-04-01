using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public BoardManager BoardScript;

        public int PlayerFoodPoints = 100;

        public float TurnDelay = 0.1f;

        public float LevelStartDelay = 2f;

        [HideInInspector] public bool PlayerTurn = true;

        void Awake () 
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);

            _enemies = new List<Enemy>();

            BoardScript = GetComponent<BoardManager>();
            InitGame();
        }

        void InitGame()
        {
            _doingSetup = true;

            _levelImage = GameObject.Find("LevelImage");
            _levelText = GameObject.Find("LevelText").GetComponent<Text>();
            _levelText.text = "Day: " + _level;
            _levelImage.SetActive(true);
            Invoke("HideLevelImage", LevelStartDelay);

            _enemies.Clear();
            BoardScript.SetupScene(_level);
        }

        public void GameOver()
        {
            _levelText.text = "After " + _level + " number of days you died.";
            _levelImage.SetActive(true);
            enabled = false;
        }

        public void AddEnemiesToList(Enemy script)
        {
            _enemies.Add(script);
        }

        IEnumerator MoveEnemies()
        {
            _enemiesMoving = true;
            yield return new WaitForSeconds(TurnDelay);
            if (_enemies.Count == 0)
            {
                yield return new WaitForSeconds(TurnDelay);
            }
            foreach (var enemy in _enemies)
            {
                enemy.MoveEnemy();
                yield return new WaitForSeconds(enemy.MoveTime);
            }

            PlayerTurn = true;
            _enemiesMoving = false;
        }

        private void OnLevelWasLoaded(int index)
        {
            _level++;

            InitGame();
        }

        private void HideLevelImage()
        {
            _levelImage.SetActive(false);
            _doingSetup = false;
        }

        void Update ()
        {
            if (PlayerTurn || _enemiesMoving || _doingSetup)
            {
                return;
            }
            StartCoroutine(MoveEnemies());
        }

        private int _level = 1;
        private List<Enemy> _enemies;
        private bool _enemiesMoving;
        private Text _levelText;
        private GameObject _levelImage;
        private bool _doingSetup;
    }
}
