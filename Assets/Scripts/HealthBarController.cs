using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarController : MonoBehaviour
{
    public Scrollbar HealthBar;

    public void SetHealth(float health)
    {
        HealthBar.size = health/_maxHealth;
    }

    public void SetMaxHealth(float maxHealth)
    {
        _maxHealth = maxHealth;
    }

    private float _maxHealth = 100;
}
