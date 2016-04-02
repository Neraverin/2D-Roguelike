using UnityEngine;

namespace Assets.Scripts
{
    public class Enemy : MovingObject
    {
        public int PlayerDamage;

        public AudioClip enemyAttack1;
        public AudioClip enemyAttack2;

        protected override void Start ()
        {
            GameManager.Instance.AddEnemiesToList(this);
            _animator = GetComponent<Animator>();
            _target = GameObject.FindGameObjectWithTag("Player").transform;

            base.Start();
        }

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

        private Animator _animator;
        private Transform _target;
        private bool _skipMove;
    }
}
