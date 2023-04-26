using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    // Set the max heal of the player
    public void SetMaxHeal(int heal)
    {
        slider.maxValue = heal;
        slider.value = heal;

        // Set the color of the heal bar
        fill.color = gradient.Evaluate(1f);
    }
    // Set the heal of the player
    public void SetHeal(int heal)
    {
        slider.value = heal;

        // Set the color of the heal bar
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
