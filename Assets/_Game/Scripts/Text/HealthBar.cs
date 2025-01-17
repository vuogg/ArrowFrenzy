using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Coroutine healthLerpCoroutine;

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Canvas healthBar;

    public void SetHealth(int health)
    {
        if(healthLerpCoroutine != null)
        {
            StopCoroutine(healthLerpCoroutine);
        }
        healthLerpCoroutine = StartCoroutine(IELerpHealth(health));
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    private IEnumerator IELerpHealth(int targetHealth)
    {
        float startHealth = slider.value;
        float duration = 0.2f;
        float elapsed = 0f;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            slider.value = Mathf.Lerp(startHealth, targetHealth, elapsed / duration);
            fill.color = gradient.Evaluate(slider.normalizedValue);
            yield return null;
        }

        slider.value = targetHealth;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
