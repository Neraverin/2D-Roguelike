using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MovingObject
{
    public int WallDamage = 1;

    public int PointsForFood = 10;
    public int PointsForSoda = 20;

    public float RestartLevelDelay = 1f;

    public Text FoodText;

    public AudioClip MoveClip1;
    public AudioClip MoveClip2;
    public AudioClip EatClip1;
    public AudioClip EatClip2;
    public AudioClip DrinkClip1;
    public AudioClip DrinkClip2;
    public AudioClip GameOverClip;

    // Use this for initialization
    protected  override void Start ()
    {
        _animator = GetComponent<Animator>();

        _food = GameManager.Instance.PlayerFoodPoints;

        FoodText.text = "Food: " + _food;
        
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        _food--;
        FoodText.text = "Food: " + _food;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        if (Move(xDir, yDir, out hit))
        {
            SoundManager.Instance.RandomizeSfx(MoveClip1, MoveClip2);
        }

        CheckIfGameOver();

        GameManager.Instance.PlayerTurn = false;
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerFoodPoints = _food;
    }
    
    void Update ()
    {
        if (!GameManager.Instance.PlayerTurn)
        {
            return;
        }

        var horizontal = 0;
        var vertical = 0;

        horizontal = (int) Input.GetAxisRaw("Horizontal");
        vertical = (int) Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        var hitWall = component as Wall;
        hitWall.DamageWall(WallDamage);

        _animator.SetTrigger("playerChop");
    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LostFood(int loss)
    {
        _animator.SetTrigger("playerHit");

        _food -= loss;
        FoodText.text = "-" + loss + " Food:" + _food;

        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (_food <= 0)
        {
            SoundManager.Instance.PlaySingle(GameOverClip);
            SoundManager.Instance.MusicSource.Stop();
            GameManager.Instance.GameOver();
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", RestartLevelDelay);
            enabled = false;
        }
        if (other.tag == "Food")
        {
            _food += PointsForFood;
            FoodText.text =  "+" + PointsForFood + " Food:" + _food;
            SoundManager.Instance.RandomizeSfx(EatClip1, EatClip2);
            other.gameObject.SetActive(false);
        }
        if (other.tag == "Soda")
        {
            _food += PointsForSoda;
            FoodText.text = "+" + PointsForSoda + " Food:" + _food;
            SoundManager.Instance.RandomizeSfx(DrinkClip1, DrinkClip2);
            other.gameObject.SetActive(false);
        }
    }

    private Animator _animator;
    private int _food;
}
