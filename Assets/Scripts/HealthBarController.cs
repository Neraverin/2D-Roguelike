using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarController : MonoBehaviour
{
    public Scrollbar HealthBar;
    public Text HealthText;

    public void SetHealth(float health)
    {
        HealthBar.size = health/_maxHealth;
        HealthText.text = (int)health + "/" + (int)_maxHealth;
    }

    public void SetMaxHealth(float maxHealth)
    {
        _maxHealth = maxHealth;
    }

    private float _maxHealth = 100;
}
