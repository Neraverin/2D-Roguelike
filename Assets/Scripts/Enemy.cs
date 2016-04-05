using System;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class Enemy : MovingObject, IPointerEnterHandler, IPointerDownHandler
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

        public void OnPointerEnter(PointerEventData data)
        {
            var gameManager = GetComponent<GameManager>();
            gameManager.BoardScript.ShowTooltip(this.GetComponent<RectTransform>().localPosition, this);
        }
        #endregion //Unity Methods

        #region MovingObject
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

        public void OnMouseEnter()
        {
            GameManager.Instance.BoardScript.ShowTooltip(transform.localPosition, this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }
        #endregion // Public Methods

        #region Fields
        private Animator _animator;
        private Transform _target;
        private int _hp = 3;

        #endregion Fields
    }
}
