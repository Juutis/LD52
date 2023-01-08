using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellForcePull : CastableSpell
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void PerformSpellEffect()
    {
        base.PerformSpellEffect();
        // spellTargetEntity.transform.position = spellTargetLocation.position;
        if (spellTargetEntity != null && spellTargetEntity.TryGetComponent(out Pullable pullable))
        {
            Debug.Log($"Perform Force Pull");
            pullable.GetPullAction().Invoke();
        }
    }
}
