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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Initialize(CastableSpell castableSpell)
    {
        spell = castableSpell;
        imgIcon.sprite = spell.Icon;
        txtHotkey.text = spell.HotKey.ToString();
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
            preparedIndicator.SetActive(true);
        }
        else if (!IsPrepared && preparedIndicator.activeSelf)
        {
            preparedIndicator.SetActive(false);
        }
    }

    private void DisplayCasting()
    {
        if (IsBeingCast)
        {
            animator.Play(castAnimation);
        }
    }

}
