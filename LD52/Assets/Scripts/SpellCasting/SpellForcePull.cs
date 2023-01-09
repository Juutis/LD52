using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellForcePull : CastableSpell
{
    private List<Pullable> pullables = new();

    // Start is called before the first frame update
    void Start()
    {
        pullables = FindObjectsOfType<Pullable>().ToList();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (IsPrepared)
        {
            pullables.ForEach(x => x.Highlight());
        }
        else
        {
            pullables.ForEach(x => x.Unhighlight());
        }

        base.Update();
    }

    public override void PerformSpellEffect()
    {
        base.PerformSpellEffect();
        ZarguufAnimator.main.Cast();
        // spellTargetEntity.transform.position = spellTargetLocation.position;
        if (spellTargetEntity != null && spellTargetEntity.TryGetComponent(out Pullable pullable))
        {
            Debug.Log($"Perform Force Pull");
            pullable.GetPullAction().Invoke();
            spellTargetEntity = null;
            spellTargetLocation = null;
            Debug.Log("playPull");
            SoundManager.main.PlaySound(GameSoundType.SpellPull);
        }
        else
        {
            Debug.Log("playFizzle");
            SoundManager.main.PlaySound(GameSoundType.SpellFizzle);
            IsOnCooldown = false;
        }
    }
}
