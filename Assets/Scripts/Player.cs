using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Player : MovingObject
    {
        #region Unity Inspector
        public AudioClip MoveClip1;
        public AudioClip MoveClip2;
        public AudioClip EatClip1;
        public AudioClip EatClip2;
        public AudioClip DrinkClip1;
        public AudioClip DrinkClip2;
        public AudioClip GameOverClip;

        public Text FoodText;
        #endregion Unity Inspector

        #region Unity Methods
        void Update()
        {
            if (!GameManager.Instance.PlayerTurn)
            {
                return;
            }

            var horizontal = GetHorizontalDirection();
            var vertical = GetVerticalDirection();

            var skipTurn = ((int)Input.GetAxisRaw("SkipTurn")) > 0;

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
            _hp--;
            UpdateHealthPoints();
            GameManager.Instance.PlayerTurn = false;
            CheckIfGameOver();
        }
        #endregion Unity Methods

        #region override MovingObject

        protected override bool Move(int xDir, int yDir, out RaycastHit2D hit)
        {
            SoundManager.Instance.RandomizeSfx(MoveClip1, MoveClip2);
            return base.Move(xDir, yDir, out hit);
        }

        protected override void Start ()
        {
            _animator = GetComponent<Animator>();

            _healthBar = GetComponent<HealthBarController>();
            _healthBar.SetMaxHealth(_startHealth);

            _hp = GameManager.Instance.PlayerHealthPoints;
            UpdateHealthPoints();
        
            base.Start();
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
            var hitEnemy = hitTransform.GetComponent<Enemy>();
            if (hitEnemy != null)
            {
                hitEnemy.DamageEnemy(1);
                _animator.SetTrigger("playerChop");
                return true;
            }
            return false;
        }
        #endregion override MovingObject

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

        #region Pubilc Methods
        public void LostFood(int loss)
        {
            _animator.SetTrigger("playerHit");

            _hp -= loss;
            UpdateHealthPoints();

            CheckIfGameOver();
        }

        private void CheckIfGameOver()
        {
            if (_hp > 0) return;
            SoundManager.Instance.PlaySingle(GameOverClip);
            SoundManager.Instance.MusicSource.Stop();
            GameManager.Instance.GameOver();
        }
        #endregion // Public Methods

        #region Private Methods
        private void OnTriggerEnter2D(Component other)
        {
            if (other.tag == "Exit")
            {
                if (_hp > 0)
                {
                    GameManager.Instance.PlayerHealthPoints = _hp;
                }
                Invoke("Restart", _restartLevelDelay);
                enabled = false;
            }
            if (other.tag == "Food")
            {
                _hp += PointsForFood;
                UpdateHealthPoints();
                SoundManager.Instance.RandomizeSfx(EatClip1, EatClip2);
                other.gameObject.SetActive(false);
            }
            if (other.tag == "Soda")
            {
                _hp += _pointsForSoda;
                UpdateHealthPoints();
                SoundManager.Instance.RandomizeSfx(DrinkClip1, DrinkClip2);
                other.gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            GameManager.Instance.PlayerHealthPoints = _hp;
        }

        private void Restart()
        {
            GameManager.Instance.IncrementLevel();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        private void UpdateHealthPoints()
        {
            FoodText.text = " Food:" + _hp;
            _healthBar.SetHealth(_hp);
        }
        #endregion // Private Methods

        #region Fields
        private Animator _animator;
        private int _hp;
        private int _pointsForFood = 10;
        private int _pointsForSoda = 20;
        private int _startHealth = 100;
        private HealthBarController _healthBar;

        private float _restartLevelDelay = 1f;
        #endregion // Fields
    }
}
