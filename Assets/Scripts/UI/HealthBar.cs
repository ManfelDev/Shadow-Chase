using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    // Set the max health of the player
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        // Set the color of the health bar
        fill.color = gradient.Evaluate(1f);
    }
    // Set the health of the player
    public void SetHealth(int health)
    {
        slider.value = health;

        // Set the color of the heal bar
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
