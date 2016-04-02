﻿using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Player : MovingObject
    {
        public AudioClip MoveClip1;
        public AudioClip MoveClip2;
        public AudioClip EatClip1;
        public AudioClip EatClip2;
        public AudioClip DrinkClip1;
        public AudioClip DrinkClip2;
        public AudioClip GameOverClip;

        public Text FoodText;

        #region override MovingObject
        protected override void Start ()
        {
            _animator = GetComponent<Animator>();

            _food = GameManager.Instance.PlayerFoodPoints;

            FoodText.text = "Food: " + _food;
        
            base.Start();
        }

        protected override bool AttemptMove(int xDir, int yDir)
        {
            if (!base.AttemptMove(xDir, yDir)) return false;

            SoundManager.Instance.RandomizeSfx(MoveClip1, MoveClip2);
            return true;
        }

        protected override bool OnCantMove(Transform hitTransform)
        {
            var hitWall = hitTransform.GetComponent<Wall>();
            if (hitWall != null)
            {
                hitWall.DamageWall(1);
                _animator.SetTrigger("playerChop");
                return true;
            }
            return false;
        }

        #endregion // override MovingObject

        #region Proprties

        public int PointsForFood
        {
            get { return _pointsForFood; }
            set { _pointsForFood = value; }
        }

        public int PointsForSoda
        {
            get { return _pointsForSoda; }
            set { _pointsForSoda = value; }
        }

        public float RestartLevelDelay
        {
            get { return _restartLevelDelay; }
            set { _restartLevelDelay = value; }
        }
        
        #endregion // Properties

        #region Unity Methods

        void Update()
        {
            if (!GameManager.Instance.PlayerTurn)
            {
                return;
            }
            var horizontal = 0;
            var vertical = 0;
            var skipTurn = false;

            horizontal = GetHorizontalDirection();
            vertical = GetVerticalDirection();

            skipTurn = ((int) Input.GetAxisRaw("SkipTurn")) > 0;

            if (skipTurn)
            {
                DoEveryTurnActions();
                return;
            }

            if (horizontal == 0 && vertical == 0) return;
            if (AttemptMove(horizontal, vertical))
            {
                DoEveryTurnActions();
            }
        }

        private void DoEveryTurnActions()
        {
            _food--;
            FoodText.text = "Food: " + _food;
            GameManager.Instance.PlayerTurn = false;
            CheckIfGameOver();
        }

        #endregion // Unity Methods

        #region Pubilc Methods

        public void LostFood(int loss)
        {
            _animator.SetTrigger("playerHit");

            _food -= loss;
            FoodText.text = "-" + loss + " Food:" + _food;

            CheckIfGameOver();
        }

        private void CheckIfGameOver()
        {
            if (_food > 0) return;
            SoundManager.Instance.PlaySingle(GameOverClip);
            SoundManager.Instance.MusicSource.Stop();
            GameManager.Instance.GameOver();
        }

        #endregion // Public Methods

        #region Private Methods

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Exit")
            {
                Invoke("Restart", _restartLevelDelay);
                enabled = false;
            }
            if (other.tag == "Food")
            {
                _food += PointsForFood;
                FoodText.text = "+" + PointsForFood + " Food:" + _food;
                SoundManager.Instance.RandomizeSfx(EatClip1, EatClip2);
                other.gameObject.SetActive(false);
            }
            if (other.tag == "Soda")
            {
                _food += _pointsForSoda;
                FoodText.text = "+" + _pointsForSoda + " Food:" + _food;
                SoundManager.Instance.RandomizeSfx(DrinkClip1, DrinkClip2);
                other.gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            GameManager.Instance.PlayerFoodPoints = _food;
        }

        private void Restart()
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        private int GetHorizontalDirection()
        {
            var horizontal = 0;
            if ((int)Input.GetAxisRaw("Horizontal") > 0 ||
                (int)Input.GetAxisRaw("UpAndRight") > 0 ||
                (int)Input.GetAxisRaw("UpAndLeft") < 0)
            {
                horizontal = 1;
            }
            if ((int)Input.GetAxisRaw("Horizontal") < 0 ||
                (int)Input.GetAxisRaw("UpAndLeft") > 0 ||
                (int)Input.GetAxisRaw("UpAndRight") < 0)
            {
                horizontal = -1;
            }
            return horizontal;
        }

        private int GetVerticalDirection()
        {
            var vertical = 0;
            if ((int)Input.GetAxisRaw("Vertical") > 0 ||
                (int)Input.GetAxisRaw("UpAndRight") > 0 ||
                (int)Input.GetAxisRaw("UpAndLeft") > 0)
            {
                vertical = 1;
            }
            if ((int)Input.GetAxisRaw("Vertical") < 0 ||
                (int)Input.GetAxisRaw("UpAndRight") < 0 ||
                (int)Input.GetAxisRaw("UpAndLeft") < 0)
            {
                vertical = -1;
            }
            return vertical;
        }

        #endregion // Private Methods

        #region Fields

        private Animator _animator;
        private int _food;
        private int _pointsForFood = 10;
        private int _pointsForSoda = 20;

        private float _restartLevelDelay = 1f;

        #endregion // Fields
    }
}