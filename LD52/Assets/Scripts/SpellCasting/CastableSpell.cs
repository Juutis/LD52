using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastableSpell : MonoBehaviour
{
    private float cooldownTimer = 0f;

    [SerializeField]
    private SpellType spellType = SpellType.Shield;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private float cooldownLengthMs = 1f;
    [SerializeField]
    private float castRange = 10f;
    [SerializeField]
    private KeyCode hotKey = KeyCode.Space;
    [SerializeField]
    private SpellCastType castType = SpellCastType.NeedsPreparation; // onkeydown
    [SerializeField]
    private SpellTargeting spellTargeting;
    [SerializeField]
    private Material mouseIndicatorMaterial;

    public SpellType Type { get { return spellType; } }
    public Sprite Icon { get { return icon; } }
    public float CooldownLengthMs { get { return cooldownLengthMs; } }
    public KeyCode HotKey { get { return hotKey; } }
    public SpellCastType CastType { get { return castType; } }

    public bool IsPrepared { get; private set; } = false;
    public bool IsBeingCast { get; private set; } = false;
    public bool IsOnCooldown { get; private set; } = false;
    public float CooldownLeft { get { return cooldownLengthMs - cooldownTimer; } }


    public void Initialize()
    {
        cooldownTimer = 0f;
        IsPrepared = false;
        IsBeingCast = false;
        IsOnCooldown = false;
        transform.localPosition = Vector3.zero;
    }

    public SpellCastResult Cast(bool keyDown = false)
    {
        bool needsPreparation = castType == SpellCastType.NeedsPreparation;
        if (IsOnCooldown)
        {
            //Debug.Log($"[CastableSpell]: Spell '{spellType}' is on cooldown! ({CooldownLeft}ms left)");
            return SpellCastResult.OnCooldown;
        }
        if (IsBeingCast)
        {
            //Debug.Log($"[CastableSpell]: Spell '{spellType}' is already being cast!");
            return SpellCastResult.IsBeingCast;
        }
        if (needsPreparation && keyDown)
        {
            IsPrepared = true;
            return SpellCastResult.Prepared;
        }
        if (needsPreparation && !IsPrepared)
        {
            Debug.LogError($"[CastableSpell]: Bug? '{spellType}' is not prepared? (keyup before keydown)");
            return SpellCastResult.IsNotPrepared;
        }
        BeforeSpellEffect();
        PerformSpellEffect();
        AfterSpellEffect();
        return SpellCastResult.Success;
    }

    private void BeforeSpellEffect()
    {
        IsPrepared = false;
        IsBeingCast = true;
        IsOnCooldown = true;
    }
    private void AfterSpellEffect()
    {
        IsPrepared = false;
        IsBeingCast = false;
    }

    public virtual void PerformSpellEffect()
    {
        Debug.Log($"[CastableSpell]: Perform '{Type}' effect");
    }

    public Material GetMouseMaterial()
    {
        return mouseIndicatorMaterial;
    }

    public float GetCastRange()
    {
        return castRange;
    }

    protected virtual void Update()
    {
        if (IsOnCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer > cooldownLengthMs)
            {
                IsOnCooldown = false;
                cooldownTimer = 0f;
            }
        }
    }
}

public enum SpellType
{
    Shield,
    Teleport,
    Pull
}
public enum SpellCastResult
{
    Success,
    Prepared,
    OnCooldown,
    IsBeingCast,
    IsNotPrepared,
}

public enum SpellCastType
{
    NeedsPreparation,
    InstantCast
}

public enum SpellTargeting
{
    MouseTarget,
    RaycastTarget,
    SelfTarget
}
