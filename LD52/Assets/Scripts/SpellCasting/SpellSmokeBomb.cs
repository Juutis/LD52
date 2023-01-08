using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSmokeBomb : CastableSpell
{
    [SerializeField]
    private Rigidbody bombPrefab;
    private float forceMagnitude = 10f;

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
        ZarguufAnimator.main.Cast();
        base.PerformSpellEffect();
        Debug.Log($"Perform SmokeBomb");
        var bomb = Instantiate(bombPrefab);
        bomb.position = spellTargetEntity.position + spellTargetEntity.forward * 0.5f;
        bomb.velocity = spellTargetLocation.position - spellTargetEntity.position;
        bomb.GetComponent<SmokeBomb>()?.SetTarget(spellTargetLocation.position);
        // Vector3 bombDirection = forceMagnitude * (spellTargetLocation.position - spellTargetEntity.position + Vector3.up * 2.5f).normalized;
        // bomb.AddForce(bombDirection, ForceMode.Impulse);
    }
}
