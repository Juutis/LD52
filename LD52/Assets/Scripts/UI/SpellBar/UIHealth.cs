using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHealth : MonoBehaviour
{
    private EntityHealth health;
    private int cachedHealth = 0;
    private float animHealth = 0;
    private int targetHealth = 0;
    private Animator animator;

    private bool isAnimating = false;
    private float animTimer = 0f;
    [SerializeField]
    private float animDuration = 1f;
    [SerializeField]
    private TextMeshProUGUI txtHealth;
    [SerializeField]
    private Image imgHealth;

    public void Initialize(EntityHealth entityHealth)
    {
        health = entityHealth;
        cachedHealth = health.Current;
        animator = GetComponent<Animator>();
        DisplayHealth(cachedHealth);
    }

    void Update()
    {
        if (health == null)
        {
            // not initialized
            return;
        }
        CheckAnimation();
        HandleAnimation();
    }

    private void CheckAnimation()
    {
        if (!isAnimating && health.Current != cachedHealth)
        {
            isAnimating = true;
            animTimer = 0f;
            targetHealth = health.Current;
        }
    }

    private void HandleAnimation()
    {
        if (isAnimating)
        {
            animTimer += Time.deltaTime;
            animHealth = Mathf.Lerp(cachedHealth, targetHealth, animTimer / animDuration);
            DisplayHealth((int)animHealth);
            if (animTimer >= animDuration)
            {
                DisplayHealth(targetHealth);
                animHealth = targetHealth;
                cachedHealth = targetHealth;
                isAnimating = false;
            }
        }
    }

    private void DisplayHealth(int newHealth)
    {
        txtHealth.text = $"{newHealth}/{health.Max}";
        imgHealth.fillAmount = (float)newHealth / (float)health.Max;
    }
}
