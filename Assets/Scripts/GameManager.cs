using System;
using System.Collections;
using System.Collections.Generic;
using SmartLocalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public BoardManager BoardScript;

        public int PlayerHealthPoints = 100;

        public float TurnDelay = 0.1f;

        public float LevelStartDelay = 2f;

        [HideInInspector] public bool PlayerTurn = true;

        public GameObject Tooltip;

        void Awake () 
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
                GameManager.Instance.InitGame();
                return;
            }
            DontDestroyOnLoad(gameObject);

            _enemies = new List<Enemy>();

            BoardScript = GetComponent<BoardManager>();
            InitGame();
        }

        void InitGame()
        {
            LanguageManager.Instance.ChangeLanguage("ru");
            _doingSetup = true;

            if (_levelImage == null)
                _levelImage = GameObject.Find("LevelImage");
            if (_levelText == null)
                _levelText = GameObject.Find("LevelText").GetComponent<Text>();
            if (Tooltip == null)
                Tooltip = GameObject.Find("Tooltip");
            _levelText.text = LanguageManager.Instance.GetTextValue("Day") + _level;
            _levelImage.SetActive(true);
            Invoke("HideLevelImage", LevelStartDelay);

            if (_enemies == null)
                _enemies = new List<Enemy>();

            _enemies.Clear();
            BoardScript.SetupScene(_level);
        }

        public void GameOver()
        {
            _levelText.text = String.Format(LanguageManager.Instance.GetTextValue("GameOver"), _level);
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
                if (enemy.isActiveAndEnabled)
                {
                    enemy.MoveEnemy();
                }
                yield return new WaitForSeconds(enemy.MoveTime);
            }

            PlayerTurn = true;
            _enemiesMoving = false;
        }

        public void IncrementLevel()
        {
            _level++;
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
