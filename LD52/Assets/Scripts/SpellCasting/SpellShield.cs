using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShield : CastableSpell
{
    [SerializeField]
    private GameObject effectContainer;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void PerformSpellEffect()
    {
        ZarguufAnimator.main.Cast();
        base.PerformSpellEffect();
        animator.Play("spellShieldEffect");
        SoundManager.main.PlaySound(GameSoundType.Shield);
    }

    public void ShieldEnd()
    {
        //SoundManager.main.PauseLoop(GameSoundType.Shield);
    }
}
