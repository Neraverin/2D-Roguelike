using UnityEngine;

namespace Assets.Scripts
{
    public class Wall : MonoBehaviour
    {
        public Sprite DamageSprite;
        public int Hp = 4;

        public AudioClip ChopClip1;
        public AudioClip ChopClip2;

        void Awake ()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void DamageWall(int loss)
        {
            _spriteRenderer.sprite = DamageSprite;
            SoundManager.Instance.RandomizeSfx(ChopClip1, ChopClip2);

            Hp -= loss;
            if (Hp <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        private SpriteRenderer _spriteRenderer;
    }
} // namespace
