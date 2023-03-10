using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastableSpell : MonoBehaviour
{
    private float cooldownTimer = 0f;
    protected Transform spellTargetEntity;
    protected Transform spellTargetLocation;

    [SerializeField]
    private Color spellColor;
    public Color Color { get { return spellColor; } }

    [SerializeField]
    private SpellType spellType = SpellType.Shield;
    [SerializeField]
    private Sprite icon;

    [SerializeField, TextArea]
    private string unlockMessage;
    public string UnlockMessage { get { return unlockMessage; } }
    [SerializeField]
    private string unlockTitle;
    public string UnlockTitle { get { return unlockTitle; } }
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
    [SerializeField]
    private float mouseIndicatorSize = 1f;

    public SpellType Type { get { return spellType; } }
    public Sprite Icon { get { return icon; } }
    public float CooldownLengthMs { get { return cooldownLengthMs; } }
    public KeyCode HotKey { get { return hotKey; } }
    public SpellCastType CastType { get { return castType; } }

    public bool IsPrepared { get; private set; } = false;
    public bool IsBeingCast { get; private set; } = false;
    public bool IsOnCooldown { get; protected set; } = false;
    public float CooldownLeft { get { return cooldownLengthMs - cooldownTimer; } }

    public bool NeedsPreparation { get { return castType == SpellCastType.NeedsPreparation; } }

    public void Initialize()
    {
        cooldownTimer = 0f;
        IsPrepared = false;
        IsBeingCast = false;
        IsOnCooldown = false;
        transform.localPosition = Vector3.zero;
    }


    public void Unprepare()
    {
        IsPrepared = false;
    }

    public SpellCastResult Prepare()
    {
        if (IsBeingCast)
        {
            //Debug.Log($"[CastableSpell]: Spell '{spellType}' is already being cast!");
            return SpellCastResult.IsBeingCast;
        }
        if (!NeedsPreparation)
        {
            return SpellCastResult.IsNotPrepared;
        }
        IsPrepared = true;
        return SpellCastResult.Prepared;
    }

    public SpellCastResult Cast()
    {
        if (IsOnCooldown)
        {
            //Debug.Log($"[CastableSpell]: Spell '{spellType}' is on cooldown! ({CooldownLeft}ms left)");
            SoundManager.main.PlaySound(GameSoundType.SpellOnCooldown);
            return SpellCastResult.OnCooldown;
        }
        if (IsBeingCast)
        {
            //Debug.Log($"[CastableSpell]: Spell '{spellType}' is already being cast!");
            return SpellCastResult.IsBeingCast;
        }
        if (NeedsPreparation && !IsPrepared)
        {
            Debug.Log($"[CastableSpell]: '{spellType}' is not prepared.");
            return SpellCastResult.IsNotPrepared;
        }
        BeforeSpellEffect();
        PerformSpellEffect();
        AfterSpellEffect();
        return SpellCastResult.Success;
    }

    private void BeforeSpellEffect()
    {
        IsBeingCast = true;
        IsOnCooldown = true;
    }
    private void AfterSpellEffect()
    {
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

    public float GetMouseIndicatorSize()
    {
        return mouseIndicatorSize;
    }

    public float GetCastRange()
    {
        return castRange;
    }

    public SpellTargeting GetSpellTargetType()
    {
        return spellTargeting;
    }

    public void SetTargets(Transform targetEntity, Transform targetLocation)
    {
        spellTargetEntity = targetEntity;
        spellTargetLocation = targetLocation;
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
    Pull,
    Bomb
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
    TargetGroundNoClip,
    TargetGroundClip,
    TargetRaycastNoClip,
    TargetRaycastClip,
    SelfTarget
}
