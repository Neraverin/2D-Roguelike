using UnityEngine;

namespace Assets.Scripts
{
    public class Enemy : MovingObject
    {
        #region Unity Inspector
        public int PlayerDamage;

        public AudioClip enemyAttack1;
        public AudioClip enemyAttack2;

        public AudioClip Attacked1;
        public AudioClip Attacked2;
        #endregion // Unity Inspector

        #region Unity Methods
        protected override void Start()
        {
            GameManager.Instance.AddEnemiesToList(this);
            _animator = GetComponent<Animator>();
            _target = GameObject.FindGameObjectWithTag("Player").transform;

            base.Start();
        }
        #endregion //Unity Methods

        #region MovingObject
        protected override bool AttemptMove(int xDir, int yDir)
        {
            if (_skipMove)
            {
                _skipMove = false;
                return true;
            }
            if (base.AttemptMove(xDir, yDir))
            {
                _skipMove = true;
                return true;
            }

            return false;
        }

        protected override bool OnCantMove(Transform hitTransform)
        {
            var hitPlayer = hitTransform.GetComponent<Player>();
            if (hitPlayer != null)
            {
                _animator.SetTrigger("enemyAttack");
                SoundManager.Instance.RandomizeSfx(enemyAttack1, enemyAttack2);
                hitPlayer.LostFood(PlayerDamage);
                return true;
            }
            return false;
        }
        #endregion // MovingObject

        #region Public Methods
        public void MoveEnemy()
        {
            var xDir = 0;
            var yDir = 0;
            if (Mathf.Abs(_target.position.x - transform.position.x) > float.Epsilon)
            {
                xDir = _target.position.x > transform.position.x ? 1 : -1;
            }
            else
            {
                yDir = _target.position.y > transform.position.y ? 1 : -1;
            }

            AttemptMove(xDir, yDir);
        }

        public void DamageEnemy(int loss)
        {
            SoundManager.Instance.RandomizeSfx(Attacked1, Attacked2);

            _hp -= loss;
            if (_hp <= 0)
            {
                gameObject.SetActive(false);
            }
        }
        #endregion // Public Methods

        #region Fields
        private Animator _animator;
        private Transform _target;
        private bool _skipMove;
        private int _hp = 3;

        #endregion Fields
    }
}
