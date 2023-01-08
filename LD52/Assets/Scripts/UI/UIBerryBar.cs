using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBerryBar : MonoBehaviour
{
    [SerializeField]
    private Image imgBarFill;
    [SerializeField]
    private TextMeshProUGUI txtCount;

    private int max;
    private int currentBerries = 0;
    private int targetBerries = 0;
    private int cachedBerries = 0;
    private float animBerries = 0;

    private float animTimer = 0f;
    [SerializeField]
    private float animDuration = 0.2f;
    private bool isAnimating = false;
    private bool initialized = false;

    public void Initialize(int maxBerries)
    {
        max = maxBerries;
        DisplayBerries(currentBerries);
        initialized = true;
    }

    public void AddBerry(int count)
    {
        currentBerries += count;
    }

    private void DisplayBerries(int berries)
    {
        imgBarFill.fillAmount = Mathf.Clamp((float)berries / (float)max, 0, 1);
        txtCount.text = $"{berries}";
    }

    void Update()
    {
        if (!initialized)
        {
            // not initialized
            return;
        }
        CheckAnimation();
        HandleAnimation();
    }

    private void CheckAnimation()
    {
        if (!isAnimating && currentBerries != cachedBerries)
        {
            isAnimating = true;
            animTimer = 0f;
            targetBerries = currentBerries;
        }
    }

    private void HandleAnimation()
    {
        if (isAnimating)
        {
            animTimer += Time.deltaTime;
            animBerries = Mathf.Lerp(cachedBerries, targetBerries, animTimer / animDuration);
            DisplayBerries((int)animBerries);
            if (animTimer >= animDuration)
            {
                DisplayBerries(targetBerries);
                animBerries = targetBerries;
                cachedBerries = targetBerries;
                isAnimating = false;
            }
        }
    }

}
