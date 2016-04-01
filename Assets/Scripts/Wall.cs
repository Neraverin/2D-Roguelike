using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite DamageSprite;
    public int Hp = 4;

    public AudioClip ChopClip1;
    public AudioClip ChopClip2;

    // Use this for initialization
    void Awake ()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss)
    {
        _spriteRenderer.sprite = DamageSprite;
        Hp -= loss;
        SoundManager.Instance.RandomizeSfx(ChopClip1, ChopClip2);

        if (Hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private SpriteRenderer _spriteRenderer;
}
