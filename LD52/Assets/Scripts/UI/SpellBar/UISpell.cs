using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISpell : MonoBehaviour
{

    [SerializeField]
    private Image imgIcon;
    [SerializeField]
    private Image imgCooldownIndicator;
    [SerializeField]
    private GameObject preparedIndicator;
    [SerializeField]
    private Image imgOverlay;
    [SerializeField]
    private TextMeshProUGUI txtHotkey;

    private CastableSpell spell;

    private Animator animator;

    private string castAnimation = "cast";

    [SerializeField, ReadOnly]
    private bool IsOnCoolDown;
    [SerializeField, ReadOnly]
    private float CooldownLeft;
    [SerializeField, ReadOnly]
    private bool IsBeingCast;
    [SerializeField, ReadOnly]
    private bool IsPrepared;

    [SerializeField]
    private Color preparedColor;
    private Color originalColor;

    void Start()
    {
        animator = GetComponent<Animator>();
        originalColor = txtHotkey.color;
    }

    public void Initialize(CastableSpell castableSpell)
    {
        spell = castableSpell;
        imgIcon.sprite = spell.Icon;
        imgOverlay.color = castableSpell.Color;
        string hotkey = spell.HotKey.ToString();
        if (hotkey == "Alpha1")
        {
            hotkey = "1";
        }
        if (hotkey == "Alpha2")
        {
            hotkey = "2";
        }
        if (hotkey == "Alpha3")
        {
            hotkey = "3";
        }
        if (hotkey == "Alpha4")
        {
            hotkey = "4";
        }
        txtHotkey.text = hotkey;
    }

    void Update()
    {
        UpdateValues();
        DisplayCooldown();
        DisplayPreparation();
        DisplayCasting();
    }

    private void UpdateValues()
    {
        IsOnCoolDown = spell.IsOnCooldown;
        CooldownLeft = spell.CooldownLeft;
        IsBeingCast = spell.IsBeingCast;
        IsPrepared = spell.IsPrepared;
    }

    private void DisplayCooldown()
    {
        if (IsOnCoolDown)
        {
            imgCooldownIndicator.enabled = true;
            imgCooldownIndicator.fillAmount = CooldownLeft / spell.CooldownLengthMs;
        }
        else if (imgCooldownIndicator.fillAmount < 1f)
        {
            imgCooldownIndicator.enabled = false;
            imgCooldownIndicator.fillAmount = 1f;
        }
    }
    private void DisplayPreparation()
    {
        if (spell.CastType == SpellCastType.InstantCast)
        {
            return;
        }
        if (IsPrepared && !preparedIndicator.activeSelf)
        {
            txtHotkey.color = preparedColor;
            preparedIndicator.SetActive(true);
        }
        else if (!IsPrepared && preparedIndicator.activeSelf)
        {
            txtHotkey.color = originalColor;
            preparedIndicator.SetActive(false);
        }
    }

    private void DisplayCasting()
    {
        if (IsBeingCast)
        {
            if (animator != null)
            {
                animator.Play(castAnimation);
            }
        }
    }

}
