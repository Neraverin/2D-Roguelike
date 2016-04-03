using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class MovingObject : MonoBehaviour
    {
        public float MoveTime = 0.1f;
        public LayerMask BlockingLayer;

        private BoxCollider2D _boxCollider;
        private Rigidbody2D _rdb2D;
        private float _inverseMoveTime;

        protected virtual void Start () 
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _rdb2D = GetComponent<Rigidbody2D>();
            _inverseMoveTime = 1 / MoveTime;
        }

        protected virtual bool Move (int xDir, int yDir, out RaycastHit2D hit)
        {
            Vector2 start = transform.position;
            var end = start + new Vector2(xDir, yDir);

            _boxCollider.enabled = false;
            hit = Physics2D.Linecast(start, end, BlockingLayer);
            _boxCollider.enabled = true;

            if (hit.transform != null) return false;
            StartCoroutine(SmoothMovement (end));
            return true;
        }

        protected IEnumerator SmoothMovement(Vector3 end)
        {
            var sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            while (sqrRemainingDistance > float.Epsilon)
            {
                var newPosition = Vector3.MoveTowards(_rdb2D.position, end, _inverseMoveTime* Time.deltaTime);
                _rdb2D.MovePosition(newPosition);
                sqrRemainingDistance = (transform.position - end).sqrMagnitude;
                yield return null;
            }
        }

        protected virtual bool AttemptMove(int xDir, int yDir)
        {
            RaycastHit2D hit;
            var canMove = Move(xDir, yDir, out hit);

            if (hit.transform == null)
            {
                return true;
            }

            if (!canMove && hit.transform != null)
            {
                return OnCantMove(hit.transform);
            }
            return true;
        }

        protected abstract bool OnCantMove (Transform hitTransform);
    }
}
